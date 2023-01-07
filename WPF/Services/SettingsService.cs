using ApplicationCore.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Interfaces;
using EasyEncryption;

namespace WPF.Services
{
    public class SettingsService : ISettingsService
    {
        private string _file = "settings.json";
        private string _secretKey = "IGwRDzLAr0BCQ6jvIGwRDzLAr0BCQ6jv";

        public Settings GetSettings()
        {
            if (!File.Exists(_file))
            {
                var settings = new Settings();
                SetSettings(settings);
                return settings;
            }

            using (StreamReader stream = new StreamReader(_file))
            {
                var read = stream.ReadToEnd();
                if(read != null)
                {
                    var dec = EasyEncryption.AesThenHmac.SimpleDecryptWithPassword(read, _secretKey);
                    return JsonConvert.DeserializeObject<Settings>(dec)!;
                }    
            }

            throw new FileNotFoundException();
        }

        public void SetSettings(Settings settings)
        {
            using(StreamWriter stream = new StreamWriter(_file))
            {
                var write = JsonConvert.SerializeObject(settings);
                var enc = EasyEncryption.AesThenHmac.SimpleEncryptWithPassword(write, _secretKey);
                stream.Write(enc);
            }
        }
    }
}
