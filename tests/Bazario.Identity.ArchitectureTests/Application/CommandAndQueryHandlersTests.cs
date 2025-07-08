using Bazario.AspNetCore.Shared.ArchitectureTests.Application;
using Bazario.Identity.ArchitectureTests.Storage;

namespace Bazario.Identity.ArchitectureTests.Application
{
    public sealed class CommandAndQueryHandlersTests 
        : CommandAndQueryHandlersTestsBase
    {
        public CommandAndQueryHandlersTests()
        {
            SetTestAssembly(
                TestAssembliesStorage.TestAssemblies.ApplicationAssembly);
        }
    }
}
