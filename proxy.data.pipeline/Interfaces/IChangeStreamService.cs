using System.Threading;
using System.Threading.Tasks;

namespace proxy.data.pipeline.Interfaces
{
    public interface IChangeStreamService<T>
    {
        Task CreateAChangeStreamForAsync(CancellationToken token);
    }
}