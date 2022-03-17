using Newtonsoft.Json;
using PluginBase;
using System;
using System.Reflection;
using System.Runtime.Loader;

namespace JsonPlugin
{
    public class OldJsonPlugin : ICommand
    {
        public string Name => "oldjson";

        public string Description => "Outputs JSON value.";

        private struct Info
        {
            public string JsonVersion;
            public string JsonLocation;
            public string Machine;
            public DateTime Date;
        }

        public int Execute()
        {
            Assembly jsonAssembly = typeof(JsonConvert).Assembly;
            Info info = new()
            {
                JsonVersion = jsonAssembly.FullName,
                JsonLocation = jsonAssembly.Location,
                Machine = Environment.MachineName,
                Date = DateTime.Now
            };

            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine(" Displaying assemblies loaded in the context of NewJsonPlugin ");
            Console.WriteLine("--------------------------------------------------------------");
            foreach (var item in AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly()).Assemblies)
            {
                Console.WriteLine(item.ToString() + Environment.NewLine + item.Location);
            }

            Console.WriteLine();
            Console.WriteLine("Serialized content:");
            Console.WriteLine(JsonConvert.SerializeObject(info, Formatting.Indented));

            return 0;
        }
    }
}
