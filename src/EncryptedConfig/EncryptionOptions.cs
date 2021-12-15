using System;
using System.Collections.Generic;
using System.Text;

namespace EncryptedConfig
{
    public class EncryptionOptions
    {
        public bool Enabled { get; set; } = true;
        public string[] EncryptedKeys { get; set; } = new string[] { };
        public string KeyRingFolder { get; set; }
        public string CertificateThumbprint { get; set; }
        public string ApplicationName { get; set; }
        public string Purpose { get; set; }
        public string[] SubPurposes { get; set; } = new string[] { }; // May want to associate these with particular keys as well?
    }
}
