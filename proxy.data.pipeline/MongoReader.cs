using System;
using System.Collections.Generic;
using MongoDB.Driver;
using proxy.data.pipeline.Interfaces;

namespace proxy.data.pipeline
{
    public class MongoReader : IMongoReader
    {
        private readonly IMongoDbSettings _mongoDbSettings;

        public MongoReader(IMongoDbSettings mongoDbSettings)
        {
            _mongoDbSettings = mongoDbSettings;
        }

        public MongoClientSettings ReadSettings()
        {
            var settings = new MongoClientSettings()
            {
                Servers = new List<MongoServerAddress>() { new MongoServerAddress(_mongoDbSettings.Host, _mongoDbSettings.Port) },
                ConnectTimeout = new TimeSpan(0, 0, 0, 300),
                SocketTimeout = new TimeSpan(0, 0, 0, 300),
                ServerSelectionTimeout = new TimeSpan(0, 0, 0, 300),
                ReplicaSetName = "rs0",
                ReadPreference = ReadPreference.SecondaryPreferred,
                UseTls = true,
                AllowInsecureTls = false,
                RetryWrites = false,
                SslSettings = new SslSettings()
                {
                    CheckCertificateRevocation = true,
                    ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true
                },
                Credential = MongoCredential.CreateCredential(_mongoDbSettings.DatabaseName, _mongoDbSettings.Username, _mongoDbSettings.Password)
            };

            return settings;
        }
    }
}