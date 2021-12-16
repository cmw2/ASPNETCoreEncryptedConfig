# ASPNETCoreEncryptedConfig

An attempt to use ASP.NET Core's DataProtection tools to encrypt appsettings values.
The use case is real values are placed into the appsettings.json file during deployment and then the EncryptorTool is run to encrypt certain keys that should be protected.

When the website runs it decrypts the encrypted values as part of the configuration system using the EncryptedConfig custom configuration provider.

We use a certificate in Local Machine (with appropriate ACL on it's private keys) to protect the DP keys which we just store in a local file folder for this demo.

## To Test
1. Publish the website to an IIS server.
2. Browse to the website.  It should show the unencrypted values since we haven't done any en/decryption yet.
3. Create a folder on the server, like C:\DataProtectionKeys
   1. make sure  both you and the App Pool have permissions to it.
4. Either create a new self signed certificate and put it in Local Computer store or use one that's already there.
   1. Make sure both you and the App Pool have permissions to it's private key.
   1. Get it's thumbprint to be used below.
6. Publish the EncryptorTool and copy it to the server
7. Edit the appsettings.json file for the EncryptorTool.
   1. Fill in the path to the key folder
   2. Fill in the certificate thumbprint
   3. Fill in the path to the website's appsettings.json file
8. Run the EncryptorTool
9. Restart the website's app pool.
10. Browse to the website.  It should show the encrypted values.
11. Edit the website's appsettings.json file
    1. Fill in the path to the key folder
    2. Fill in the certificate thumbprint
    3. Change the EncryptionOptions:Enabled setting to true and save.
12. Restart the website's app pool.
13. Browse to the website.  It should show the decrypted values.
