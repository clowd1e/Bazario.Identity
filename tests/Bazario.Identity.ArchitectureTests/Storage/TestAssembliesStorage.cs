using Bazario.AspNetCore.Shared.ArchitectureTests.CleanArchitecture;

namespace Bazario.Identity.ArchitectureTests.Storage
{
    internal static class TestAssembliesStorage
    {
        public static TestBaseAsseblies TestAssemblies =
            new TestBaseAsseblies(
                domainAssembly: typeof(Identity.Domain.AssemblyMarker).Assembly,
                applicationAssembly: typeof(Identity.Application.AssemblyMarker).Assembly,
                infrastructureAssembly: typeof(Identity.Infrastructure.AssemblyMarker).Assembly,
                presentationAssembly: typeof(Identity.WebAPI.AssemblyMarker).Assembly
            );
    }
}
