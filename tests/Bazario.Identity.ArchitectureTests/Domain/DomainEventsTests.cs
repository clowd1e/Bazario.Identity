using Bazario.AspNetCore.Shared.ArchitectureTests.Domain;
using Bazario.Identity.ArchitectureTests.Storage;

namespace Bazario.Identity.ArchitectureTests.Domain
{
    public sealed class DomainEventsTests : DomainEventsTestsBase
    {
        public DomainEventsTests()
        {
            SetTestAssembly(
                TestAssembliesStorage.TestAssemblies.DomainAssembly);
        }
    }
}
