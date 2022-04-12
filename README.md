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
   1. There's a pipeline now to build and push as a Universal Package to Azure Artifacts.  The CD pipeline for the website then downloads the package so you don't have to preinstall the tool on the server.  This assumes using an agent that is running on the target IIS server, in our case it is a VM enrolled in a YAML Environment.
8. Edit the appsettings.json file for the EncryptorTool.
   1. Fill in the certificate thumbprint
   2. Fill in the path to the website's appsettings.json file
   3. NOTE: If running via the CD pipeline provide these values as command line args instead.
9. Run the EncryptorTool
10. Restart the website's app pool. 
15. Browse to the website.  It should show the decrypted values.

Note the settings for the EncryptorTool specify which keys should be encrypted/decrypted.
