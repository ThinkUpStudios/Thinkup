using ThinkUp.Sdk.Data.Entities;

namespace ThinkUp.Sdk.Tests.TestModels
{
    public class TestEntity : DataEntity
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsValid { get; set; }
    }
}
