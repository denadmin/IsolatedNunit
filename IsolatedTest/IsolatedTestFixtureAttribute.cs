using System.Reflection;
using System.Runtime.Loader;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace IsolatedTest;

public class IsolatedTestFixtureAttribute : TestFixtureAttribute, IFixtureBuilder2
{
    public new IEnumerable<TestSuite> BuildFrom(ITypeInfo typeInfo) =>
        base.BuildFrom(GetIsolated(typeInfo));

    public new IEnumerable<TestSuite> BuildFrom(ITypeInfo typeInfo, IPreFilter filter) =>
        base.BuildFrom(GetIsolated(typeInfo), filter);

    private static ITypeInfo GetIsolated(ITypeInfo typeInfo)
    {
        var asmLocation = typeInfo.Type.Assembly.Location;
        var alc = new IsolatedTestLoadContext(asmLocation);
        var asm = alc.LoadFromAssemblyPath(asmLocation);
        var isolatedType = asm.GetType(typeInfo.Type.FullName!, true, false)!;

        return new TypeWrapper(isolatedType);
    }

    private sealed class IsolatedTestLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver resolver;

        internal IsolatedTestLoadContext(string assemblyLocation)
            : base($"{nameof(IsolatedTestLoadContext)}.{Guid.NewGuid()}")
        {
            resolver = new AssemblyDependencyResolver(assemblyLocation);
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            if (assemblyName.Name == "nunit.framework")
            {
                return null;
            }

            var assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);
            return assemblyPath != null ? LoadFromAssemblyPath(assemblyPath) : null;
        }
    }
}