using System;
using System.Configuration;

namespace ThinkUp.Sdk.Data.Configuration
{
    public class DataSection : ConfigurationSection, IDataSection
    {
        private static readonly Lazy<IDataSection> instance;

        static DataSection()
        {
            instance = new Lazy<IDataSection>(() =>
            {
                return ConfigurationManager.GetSection("data") as IDataSection;
            });
        }

        public static IDataSection Instance()
        {
            return instance.Value;
        }

        [ConfigurationProperty("connectionString", IsRequired = true, DefaultValue = "")]
        public string ConnectionString
        {
            get { return (string)this["connectionString"]; }
            set { this["connectionString"] = value; }
        }

        [ConfigurationProperty("databaseName", IsRequired = true, DefaultValue = "")]
        public string DatabaseName
        {
            get { return (string)this["databaseName"]; }
            set { this["databaseName"] = value; }
        }
    }
}
