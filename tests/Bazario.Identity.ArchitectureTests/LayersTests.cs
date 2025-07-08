using Bazario.AspNetCore.Shared.ArchitectureTests.CleanArchitecture;
using Bazario.Identity.ArchitectureTests.Storage;

namespace Bazario.Identity.ArchitectureTests;

public sealed class LayersTests : LayersTestsBase
{
    public LayersTests()
    {
        SetTestAssemblies(TestAssembliesStorage.TestAssemblies);
    }
}
