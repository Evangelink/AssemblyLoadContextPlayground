using Contract;

using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string tfm = "netcoreapp3.0";
            var pluginPaths = new[]
            {
                $@"ClientV1\bin\debug\{tfm}\ClientV1.dll",
                $@"ClientV2\bin\debug\{tfm}\ClientV2.dll",
                $@"ClientV3\bin\debug\{tfm}\ClientV3.dll",
            };

            var commands = pluginPaths.Select(pluginPath => new Proxy(pluginPath)).ToList();

            commands.ForEach(x =>
            {
                try
                {
                    x.MyMethod();
                    var payload = new HostPayload();
                    x.MethodWithPayload(payload);
                    x.MyNewMethod(0);
                }
                catch (NotImplementedException)
                {
                }
            });

            Console.ReadKey();
        }
    }

    internal class Proxy : ICommand, IDisposable
    {
        private readonly PluginLoadContext _context;
        private readonly ICommand _instance;

        public Proxy(string dllPath)
        {
            var currentLocation = new DirectoryInfo(typeof(Program).Assembly.Location);
            string root = currentLocation.Parent!.Parent!.Parent!.Parent!.Parent!.FullName;

            string pluginLocation = Path.GetFullPath(Path.Combine(root, dllPath.Replace('\\', Path.DirectorySeparatorChar)));
            _context = new PluginLoadContext(pluginLocation);
            var assembly = _context.LoadFromAssemblyName(AssemblyName.GetAssemblyName(pluginLocation));

            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(ICommand).IsAssignableFrom(type))
                {
                    if (Activator.CreateInstance(type) is not ICommand instance)
                    {
                        throw new InvalidOperationException($"Cannot create instance of ICommand for type '{type}'.");
                    }

                    _instance = instance;
                    return;
                }
            }

            throw new InvalidOperationException($"Could not find any type compatible with ICommand in '{dllPath}'.");
        }

        public void Dispose()
        {
            _context.Unload();
        }

        public int MethodWithPayload(IPayload payload) => _instance.MethodWithPayload(payload);
        public int MyMethod() => _instance.MyMethod();
        public int MyNewMethod(object arg) => _instance.MyNewMethod(arg);
    }

    internal class HostPayload : IPayload
    {
        public string Name => "HostPayload";
        public int PropertyThatWillBeRemoved { get; set; }
        public string NewProperty { get; set; } = "Prop";

        public void Callback(ICommand command)
        {
            command.MyMethod();
        }
    }
}