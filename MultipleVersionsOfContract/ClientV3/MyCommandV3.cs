using Contract;

using System;

namespace ClientV3
{
    public class MyCommandV3 : ICommand
    {
        public int MyMethod()
        {
            Console.WriteLine("ClientV3.MyMethod called.");
            return 0;
        }

        public int MyNewMethod(object arg)
        {
            Console.WriteLine("ClientV3.MyNewMethod called.");
            return 0;
        }

        public int MethodWithPayload(IPayload payload)
        {
            Console.WriteLine("ClientV3.MethodWithPayload called.");
            Console.WriteLine("Received payload named: {0}", payload.Name);
            payload.NewProperty = "v3";
            payload.Callback(this);
            return 0;
        }
    }
}