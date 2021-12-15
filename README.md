# ASPNETCoreEncryptedConfig

An attempt to use ASP.NET Core's DataProtection tools to encrypt appsettings values.
The use case is real values are placed into the appsettings.json file during deployment and then the EncryptorTool is run to encrypt certain keys that should be protected.

When the website runs it decrypts the encrypted values as part of the configuration system using the EncryptedConfig custom configuration provider.

We use a certificate in Local Machine (with appropriate ACL on it's private keys) to protect the DP keys which we just store in a local file folder for this demo.
