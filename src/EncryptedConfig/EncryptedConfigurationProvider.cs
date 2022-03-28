using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Pitchfork.Cryptography.CngDpapi;

namespace EncryptedConfig
{
    public class EncryptedConfigurationProvider : ConfigurationProvider
    {
        private readonly EncryptionOptions _options;
        private readonly IConfiguration _configuration;
        //private ProtectionDescriptor _protector;

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
                    string ciphertext = _configuration[key];
                    var ciphertextBlob = System.Convert.FromBase64String(ciphertext);
                    byte[] plaintextBlob = ProtectionDescriptor.UnprotectSecret(ciphertextBlob);
                    string plaintext = Encoding.UTF8.GetString(plaintextBlob);
                    settings[key] = plaintext;
                }
                settings[ranKey] = true.ToString();
            } else
            {
                settings[ranKey] = false.ToString();
            }

            Data = settings;
        }

        //private ProtectionDescriptor BuildDataProtector()
        //{
        //    var cert = GetCertificate(StoreLocation.LocalMachine, _options.CertificateThumbprint);  // specify store in settings?

        //    string descriptor = "Certificate = CERTBLOB:" + Convert.ToBase64String(cert.Export(X509ContentType.Cert));

        //    var protector = new ProtectionDescriptor(descriptor);
        //    return protector;
        //}

        //private X509Certificate2 GetCertificate(StoreLocation location, string thumprint)
        //{
        //    using X509Store store = new X509Store(location);
        //    store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
        //    var cert = store.Certificates.Find(X509FindType.FindByThumbprint, thumprint, validOnly: false)[0];
        //    store.Close();

        //    return cert;
        //}

        //private ProtectionDescriptor Protector
        //{
        //    get
        //    {
        //        if (_protector == null)
        //        {
        //            _protector = BuildDataProtector();
        //        }
        //        return _protector;
        //    }
        //}
    }
}
