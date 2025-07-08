using Bazario.AspNetCore.Shared.ArchitectureTests.Application;
using Bazario.Identity.ArchitectureTests.Storage;

namespace Bazario.Identity.ArchitectureTests.Application
{
    public sealed class ValidatorsTests : ValidatorsTestsBase
    {
        public ValidatorsTests()
        {
            SetTestAssembly(
               TestAssembliesStorage.TestAssemblies.ApplicationAssembly);
        }
    }
}
