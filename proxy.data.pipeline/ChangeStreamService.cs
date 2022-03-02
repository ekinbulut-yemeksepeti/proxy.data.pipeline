using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using proxy.data.pipeline.Interfaces;

namespace proxy.data.pipeline
{
    public abstract class ChangeStreamService<T> : IChangeStreamService<T> where T : IDocument
    {
        private readonly IMongoContext _context;
        public ConcurrentQueue<T> Queue { get; } = new ConcurrentQueue<T>();
        protected ChangeStreamService(IMongoContext context)
        {
            _context = context;
            //Queue = new ConcurrentQueue<T>();
        }

        public async Task CreateAChangeStreamForAsync(CancellationToken token)
        {
            var pipeline = CreatePipeline<T>();
            var options = CreateChangeStreamOptions();

            var name = typeof(T).Name.ToLower();
            var collection = _context.GetCollection<T>(name);

            using var cursor = await collection.WatchAsync(pipeline,options, cancellationToken: token);
            await cursor.ForEachAsync(async change =>
            {
                await SendToQueueAsync(change);
            }, cancellationToken: token);
        }
        private EmptyPipelineDefinition<ChangeStreamDocument<T>> CreatePipeline<T>()
        {
            var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<T>>();
            pipeline.Match(x => 
                x.OperationType == ChangeStreamOperationType.Insert | 
                x.OperationType == ChangeStreamOperationType.Update |
                x.OperationType == ChangeStreamOperationType.Replace);

            return pipeline;
        }
        private ChangeStreamOptions CreateChangeStreamOptions()
        {
            var options = new ChangeStreamOptions
            {
                FullDocument = ChangeStreamFullDocumentOption.UpdateLookup
            };
            return options;
        }
        private Task SendToQueueAsync(ChangeStreamDocument<T> change)
        {
            switch (change.OperationType)
            {
                case ChangeStreamOperationType.Insert:
                    change.FullDocument.Status = ChangeStreamStatusEnum.Insert;
                    break;
                case ChangeStreamOperationType.Update:
                case ChangeStreamOperationType.Replace:
                    change.FullDocument.Status = ChangeStreamStatusEnum.Update;
                    break;
                case ChangeStreamOperationType.Delete:
                    change.FullDocument.Status = ChangeStreamStatusEnum.Delete;
                    break;
                case ChangeStreamOperationType.Invalidate:
                case ChangeStreamOperationType.Rename:
                case ChangeStreamOperationType.Drop:
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Queue.Enqueue(change.FullDocument);
            return Task.CompletedTask;
        }

    }
}