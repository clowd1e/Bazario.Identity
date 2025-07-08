using Bazario.AspNetCore.Shared.ArchitectureTests.Infrastructure;
using Bazario.Identity.ArchitectureTests.Storage;

namespace Bazario.Identity.ArchitectureTests.Infrastructure
{
    public sealed class EventConsumersTests : EventConsumersTestsBase
    {
        public EventConsumersTests()
        {
            SetTestAssembly(
                TestAssembliesStorage.TestAssemblies.InfrastructureAssembly);
        }
    }
}
