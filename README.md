# ASPNETCoreEncryptedConfig

An ASP.NET Core ConfigurationProvider that decrypts values that were encrypted with DPAPI:NG.  Also a corresponding console app to encrypt config values with DPAPI:NG.  (We use a library from https://github.com/GrabYourPitchforks/Pitchfork.Cryptography.CngDpapi for the calls to DPAPI:NG.)

The use case is real values are placed into the appsettings.json file during deployment and then the EncryptorTool is run to encrypt certain keys that should be protected.  This would typically be done by a pipeline job.

When the website runs it decrypts the encrypted values as part of the configuration system using the EncryptedConfig custom configuration provider.

We use a certificate in Local Machine (with appropriate ACL on it's private keys) to protect the secrets.

## To Test
1. Publish the website to an IIS server.
2. Browse to the website.  It should show the unencrypted values since we haven't done any en/decryption yet.
3. Either create a new self signed certificate and put it in Local Computer store or use one that's already there.
   1. Make sure both you and the App Pool have permissions to its private key.  (When automating, make sure the pipeline agent identity also has permissions.)
   1. Get it's thumbprint to be used below.
6. Publish the EncryptorTool and copy it to the server
7. Edit the appsettings.json file for the EncryptorTool.
   1. Fill in the certificate thumbprint
   2. Fill in the path to the website's appsettings.json file
8. Run the EncryptorTool
9. Restart the website's app pool.
10. Browse to the website.  It should show the encrypted values.
11. Edit the website's appsettings.json file
    1. Change the EncryptionOptions:Enabled setting to true and save.
12. Restart the website's app pool.
13. Browse to the website.  It should show the decrypted values.

Note the settings for the EncryptorTool and the EncryptedConfig specify which keys should be encrypted/decrypted.
