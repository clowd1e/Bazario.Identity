using Bazario.AspNetCore.Shared.ArchitectureTests.Application;
using Bazario.Identity.ArchitectureTests.Storage;

namespace Bazario.Identity.ArchitectureTests.Application
{
    public sealed class CommandsAndQueriesTests
        : CommandsAndQueriesTestsBase
    {
        public CommandsAndQueriesTests()
        {
            SetTestAssembly(
                TestAssembliesStorage.TestAssemblies.ApplicationAssembly);
        }
    }
}
