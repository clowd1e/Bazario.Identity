using Bazario.AspNetCore.Shared.ArchitectureTests.Application;
using Bazario.Identity.ArchitectureTests.Storage;

namespace Bazario.Identity.ArchitectureTests.Application
{
    public sealed class MappersTests : MappersTestsBase
    {
        public MappersTests()
        {
            SetTestAssembly(
                TestAssembliesStorage.TestAssemblies.ApplicationAssembly);
        }
    }
}
