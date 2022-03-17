using Contract;

using System;

namespace ClientV2
{
    public class MyCommandV2 : ICommand
    {
        public int MyMethod()
        {
            Console.WriteLine("ClientV2.MyMethod called.");
            return 0;
        }

        public int MyMethodThatWillBeRemoved()
        {
            Console.WriteLine("ClientV2.MyMethodThatWillBeRemoved called.");
            return 0;
        }

        public int MyNewMethod(object arg)
        {
            Console.WriteLine("ClientV2.MyNewMethod called.");
            return 0;
        }

        public int MethodWithPayload(IPayload payload)
        {
            Console.WriteLine("ClientV2.MethodWithPayload called.");
            Console.WriteLine("Received payload named: {0}", payload.Name);
            payload.PropertyThatWillBeRemoved = 2;
            payload.NewProperty = "v2";
            payload.Callback(this);
            return 0;
        }
    }
}