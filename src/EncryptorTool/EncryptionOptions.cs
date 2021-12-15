using System;
using System.Collections.Generic;
using System.Text;

namespace EncryptorTool
{
    public class EncryptionOptions
    {
        public string ReadFromSettingsPath { get; set; }
        public string WriteToSettingsPath { get; set; }
        public string[] EncryptedKeys { get; set; } = new string[] { };
        public string KeyRingFolder { get; set; }
        public string CertificateThumbprint { get; set; }
        public string ApplicationName { get; set; }
        public string Purpose { get; set; }
        public string[] SubPurposes { get; set; } = new string[] { }; // May want to associate these with particular keys as well?
    }
}
