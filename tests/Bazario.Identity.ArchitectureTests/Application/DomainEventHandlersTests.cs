using Bazario.AspNetCore.Shared.ArchitectureTests.Application;
using Bazario.Identity.ArchitectureTests.Storage;

namespace Bazario.Identity.ArchitectureTests.Application
{
    public sealed class DomainEventHandlersTests
        : DomainEventHandlersTestsBase
    {
        public DomainEventHandlersTests()
        {
            SetTestAssembly(
                TestAssembliesStorage.TestAssemblies.ApplicationAssembly);
        }
    }
}
