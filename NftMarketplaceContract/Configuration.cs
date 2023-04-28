using System;
using Newtonsoft.Json;

namespace NFTMarketplaceContract
{
    public class Configuration
    {
        private const string ConfigFilePath = "ContractConnectionConfig.json";

        public string Url { get; set; }
        public string PrivateKey { get; set; }
        public string ContractAddress { get; set; }

        public static Configuration ReadConfig()
        {
            Configuration config = new Configuration();
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    string json = File.ReadAllText(ConfigFilePath);
                    config = JsonConvert.DeserializeObject<Configuration>(json);
                }
                else
                {
                    Console.WriteLine("Configuration file not found. Creating a new one.");
                    config.SaveConfig();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to read configuration file: " + ex.Message);
            }
            return config;
        }

        public void SaveConfig()
        {
            try
            {
                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(ConfigFilePath, json);
                Console.WriteLine("Configuration file updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to save configuration file: " + ex.Message);
            }
        }

        public void WriteContractAddressToConfig(string contractAddress)
        {
            this.ContractAddress = contractAddress;
            SaveConfig();
        }
    }
}

