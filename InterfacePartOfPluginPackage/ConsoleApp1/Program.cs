using Lib;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

class Proxy //: IA, IDisposable
{
    private readonly PluginLoadContext _context;
    private readonly Assembly _assembly;

    public Proxy(string dllPath)
    {
        _context = new PluginLoadContext(dllPath);
        _assembly = _context.LoadFromAssemblyName(AssemblyName.GetAssemblyName(dllPath));
    }

    public Out MyMethod(In @in)
    {
        foreach (Type type in _assembly.GetTypes())
        {
            if (typeof(IA).IsAssignableFrom(type))
            {
                Console.WriteLine("Type is assignable to IA, creating an instance and calling MyMethod.");
                var instance = Activator.CreateInstance(type) as IA;
                return instance!.MyMethod(@in);
            }
        }

        Console.WriteLine("Could not find any type assignable to IA.");
        return null!;
    }

    public void Dispose()
    {
        _context.Unload();
    }
}

internal class PluginLoadContext : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver _resolver;

    public PluginLoadContext(string pluginPath) : base(isCollectible: true)
    {
        _resolver = new AssemblyDependencyResolver(pluginPath);
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        var assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
        if (assemblyPath != null)
        {
            return LoadFromAssemblyPath(assemblyPath);
        }

        return null;
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        var libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        if (libraryPath != null)
        {
            return LoadUnmanagedDllFromPath(libraryPath);
        }

        return IntPtr.Zero;
    }
}

public class Program
{
    public static void Main()
    {
        var currentLocation = new DirectoryInfo(typeof(Program).Assembly.Location);
        var rootDir = currentLocation.Parent!.Parent!.Parent!.Parent!.Parent;
        // TODO: After the first build, copy Lib.dll to the root folder. After, you can run the
        // app and see that the types are not compatible.
        var x = new Proxy(Path.Combine(rootDir!.FullName, "Lib.dll"));
        x.MyMethod(new In());
        (x as IDisposable)?.Dispose();
    }
}