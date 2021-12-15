using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EncryptorTool
{
    public class SettingsHelper
    {
        private readonly string _appSettingsPath;
        private dynamic _jsonObj;

        public void ReadFromFile(string appSettingsPath)
        {
            string json = File.ReadAllText(appSettingsPath);
            _jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
        }

        public void WriteToFile(string appSettingsPath)
        {
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(_jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(appSettingsPath, output);
        }

        public T Get<T>(string sectionPathKey) 
        {
            return GetValueRecursively<T>(sectionPathKey, _jsonObj);
        }

        public void AddOrUpdateAppSetting<T>(string sectionPathKey, T value)
        {
            SetValueRecursively<T>(sectionPathKey, _jsonObj, value);
        }

        private void SetValueRecursively<T>(string sectionPathKey, dynamic jsonObj, T value)
        {
            // split the string at the first ':' character
            var remainingSections = sectionPathKey.Split(":", 2);

            var currentSection = remainingSections[0];
            if (remainingSections.Length > 1)
            {
                // continue with the procress, moving down the tree
                var nextSection = remainingSections[1];
                SetValueRecursively(nextSection, jsonObj[currentSection], value);
            }
            else
            {
                // we've got to the end of the tree, set the value
                jsonObj[currentSection] = value;
            }
        }

        private T GetValueRecursively<T>(string sectionPathKey, dynamic localJsonObj)
        {
            // split the string at the first ':' character
            var remainingSections = sectionPathKey.Split(":", 2);

            var currentSection = remainingSections[0];
            if (remainingSections.Length > 1)
            {
                // continue with the procress, moving down the tree
                var nextSection = remainingSections[1];
                return GetValueRecursively<T>(nextSection, localJsonObj[currentSection]);
            }
            else
            {
                // we've got to the end of the tree, set the value
                return localJsonObj[currentSection];
            }
        }
    }
}