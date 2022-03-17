using System;
using System.IO;
using System.Linq;
using System.Reflection;

using PluginBase;

namespace AppWithPlugin
{
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
                    _instance = Activator.CreateInstance(type) as ICommand;
                    return;
                }
            }
        }

        public string Name => _instance.Name;

        public string Description => _instance.Description;

        public void Dispose()
        {
            _context.Unload();
        }

        public int Execute()
        {
            return _instance.Execute();
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                if (args.Length == 1 && args[0] == "/d")
                {
                    Console.WriteLine("Waiting for any key...");
                    Console.ReadLine();
                }

                const string tfm = "net5.0";
                string[] pluginPaths = new[]
                {
                    $@"OldJsonPlugin\bin\Debug\{tfm}\OldJsonPlugin.dll",
                    $@"NewJsonPlugin\bin\Debug\{tfm}\NewJsonPlugin.dll",
                };

                var commands = pluginPaths.Select(pluginPath => new Proxy(pluginPath)).ToList();

                if (args.Length == 0)
                {
                    Console.WriteLine("Commands: ");
                    foreach (ICommand command in commands)
                    {
                        Console.WriteLine($"{command.Name}\t - {command.Description}");
                        command.Execute();
                    }
                }
                else
                {
                    foreach (string commandName in args)
                    {
                        Console.WriteLine($"-- {commandName} --");
                        ICommand command = commands.FirstOrDefault(c => c.Name == commandName);
                        if (command == null)
                        {
                            Console.WriteLine("No such command is known.");
                            return;
                        }

                        command.Execute();
                        Console.WriteLine();
                    }
                }

                commands.ForEach(command => command.Dispose());
                Console.WriteLine("Count: " + S.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
