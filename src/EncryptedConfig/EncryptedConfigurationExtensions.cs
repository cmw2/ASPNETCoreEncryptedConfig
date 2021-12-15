using System;
using System.Collections.Generic;
using System.Text;
using EncryptedConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.Configuration
{
    public static class EncryptedConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddEncryptedConfiguration(
            this IConfigurationBuilder builder)
        {            
            var tempConfig = builder.Build();
            var options = tempConfig.GetSection("EncryptionOptions").Get<EncryptionOptions>();

            return builder.Add(new EntityConfigurationSource(options, tempConfig));
        }
    }
}