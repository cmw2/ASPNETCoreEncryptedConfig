using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace EncryptedConfig
{
    public class EncryptedConfigurationProvider : ConfigurationProvider
    {
        private readonly EncryptionOptions _options;
        private readonly IConfiguration _configuration;
        private IDataProtector _protector;

        public EncryptedConfigurationProvider(EncryptionOptions options, IConfiguration configuration)
        {
            _options = options;
            _configuration = configuration;
        }

        public override void Load()
        {
            var settings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            const string ranKey = "RanDecryption";

            if (_options.Enabled)
            {
                foreach (string key in _options.EncryptedKeys)
                {
                    settings[key] = Protector.Unprotect(_configuration[key]);
                }
                settings[ranKey] = true.ToString();
            } else
            {
                settings[ranKey] = false.ToString();
            }

            Data = settings;
        }

        private IDataProtector BuildDataProtector()
        {
            var provider = DataProtectionProvider.Create(
                new DirectoryInfo(_options.KeyRingFolder),
                configuration =>
                {
                    X509Store store = new X509Store(StoreLocation.LocalMachine);
                    store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                    var cert = store.Certificates.Find(X509FindType.FindByThumbprint, _options.CertificateThumbprint, validOnly: false)[0];
                    store.Close();

                    configuration.SetApplicationName(_options.ApplicationName);
                    configuration.ProtectKeysWithCertificate(cert);
                });
            var protector = provider.CreateProtector(_options.Purpose, _options.SubPurposes);
            return protector;
        }

        private IDataProtector Protector
        {
            get
            {
                if (_protector == null)
                {
                    _protector = BuildDataProtector();
                }
                return _protector;
            }
        }
    }
}
