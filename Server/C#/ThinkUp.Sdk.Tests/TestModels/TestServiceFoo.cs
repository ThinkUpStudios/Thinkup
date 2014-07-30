namespace ThinkUp.Sdk.Tests.TestModels
{
    public class TestServiceFoo : ITestServiceFoo
    {
        public ITestServiceBar TestServiceBar { get; private set; }

        public TestServiceFoo(ITestServiceBar testServiceBar)
        {
            this.TestServiceBar = testServiceBar;
        }

        public void TestFoo()
        {
            return;
        }
    }
}
