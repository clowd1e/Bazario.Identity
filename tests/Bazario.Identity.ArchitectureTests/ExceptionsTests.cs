using Bazario.AspNetCore.Shared.ArchitectureTests;
using Bazario.Identity.ArchitectureTests.Storage;

namespace Bazario.Identity.ArchitectureTests
{
    public sealed class ExceptionsTests : ExceptionsTestsBase
    {
        public ExceptionsTests()
        {
            SetTestAssemblies(
                TestAssembliesStorage.TestAssemblies.ApplicationAssembly,
                TestAssembliesStorage.TestAssemblies.InfrastructureAssembly,
                TestAssembliesStorage.TestAssemblies.PresentationAssembly);
        }
    }
}
