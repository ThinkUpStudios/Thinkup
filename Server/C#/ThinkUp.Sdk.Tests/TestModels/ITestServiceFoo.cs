namespace ThinkUp.Sdk.Tests.TestModels
{
    public interface ITestServiceFoo
    {
        ITestServiceBar TestServiceBar { get; }

        void TestFoo();
    }
}
