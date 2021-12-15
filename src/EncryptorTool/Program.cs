using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;

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
                string encryptedVal = protector.Protect(settingsHelper.Get<string>(key));
                //string encryptedVal = $"encrypted: {settingsHelper.Get<string>(key)}";
                settingsHelper.AddOrUpdateAppSetting<string>(
                    key,
                    encryptedVal);
                Console.WriteLine($"Encrypted {key}");
            }

            settingsHelper.WriteToFile(options.WriteToSettingsPath);
            Console.WriteLine("Encryption Complete");
        }

        static private IDataProtector BuildDataProtector(EncryptionOptions options)
        {
            var provider = DataProtectionProvider.Create(
                new DirectoryInfo(options.KeyRingFolder),
                configuration =>
                {
                    X509Store store = new X509Store(StoreLocation.LocalMachine);
                    store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                    var cert = store.Certificates.Find(X509FindType.FindByThumbprint, options.CertificateThumbprint, validOnly: false)[0];
                    store.Close();

                    configuration.SetApplicationName(options.ApplicationName);
                    configuration.ProtectKeysWithCertificate(cert);
                });
            var protector = provider.CreateProtector(options.Purpose, options.SubPurposes);
            return protector;
        }
    }
}
