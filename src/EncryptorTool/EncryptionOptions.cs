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
        public string CertificateThumbprint { get; set; }
    }
}
