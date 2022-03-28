using System;
using System.Collections.Generic;
using System.Text;

namespace EncryptedConfig
{
    public class EncryptionOptions
    {
        public bool Enabled { get; set; } = true;
        public string[] EncryptedKeys { get; set; } = new string[] { };
    }
}
