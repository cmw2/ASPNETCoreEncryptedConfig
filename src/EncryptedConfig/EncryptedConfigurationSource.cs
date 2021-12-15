using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EncryptedConfig
{
    public class EntityConfigurationSource : IConfigurationSource
    {
        private readonly EncryptionOptions _options;
        private readonly IConfiguration _configuration;

        public EntityConfigurationSource(EncryptionOptions options, IConfiguration configuration)
        {
            _options = options;
            _configuration = configuration;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder) =>
            new EncryptedConfigurationProvider(_options, _configuration);
    }
}