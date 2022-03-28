using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Extensions.Configuration;
using Pitchfork.Cryptography.CngDpapi;

namespace EncryptorTool
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            var options = config.GetSection("EncryptionOptions").Get<EncryptionOptions>();
            
            var protector = BuildDataProtector(options);

            var settingsHelper = new SettingsHelper();
            settingsHelper.ReadFromFile(options.ReadFromSettingsPath);

            // Protect the payload
            foreach (string key in options.EncryptedKeys)
            {
                string plaintext = settingsHelper.Get<string>(key);
                byte[] plaintextBlob = Encoding.UTF8.GetBytes(plaintext);
                byte[] ciphertextBlob = protector.ProtectSecret(plaintextBlob);
                string ciphertext = Encoding.UTF8.GetString(ciphertextBlob);
                //string encryptedVal = $"encrypted: {settingsHelper.Get<string>(key)}";
                settingsHelper.AddOrUpdateAppSetting<string>(
                    key,
                    ciphertext);
                Console.WriteLine($"Encrypted {key}");
            }

            settingsHelper.WriteToFile(options.WriteToSettingsPath);
            Console.WriteLine("Encryption Complete");
        }

        static private ProtectionDescriptor BuildDataProtector(EncryptionOptions options)
        {
            var cert = GetCertificate(StoreLocation.LocalMachine, options.CertificateThumbprint);  // specify store in settings?

            string descriptor = "Certificate = CERTBLOB:" + Convert.ToBase64String(cert.Export(X509ContentType.Cert));

            var protector = new ProtectionDescriptor(descriptor);
            return protector;
        }

        static private X509Certificate2 GetCertificate(StoreLocation location, string thumprint)
        {
            using X509Store store = new X509Store(location);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            var cert = store.Certificates.Find(X509FindType.FindByThumbprint, thumprint, validOnly: false)[0];
            store.Close();

            return cert;
        }
    }
}
