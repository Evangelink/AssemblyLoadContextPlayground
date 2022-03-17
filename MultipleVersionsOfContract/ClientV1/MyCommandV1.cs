using Contract;

using System;

namespace ClientV1
{
    public class MyCommandV1 : ICommand
    {
        public int MyMethod()
        {
            Console.WriteLine("ClientV1.MyMethod called.");
            return 0;
        }

        public int MyMethodThatWillBeRemoved()
        {
            Console.WriteLine("ClientV1.MyMethodThatWillBeRemoved called.");
            return 0;
        }

        public int MethodWithPayload(IPayload payload)
        {
            Console.WriteLine("ClientV1.MethodWithPayload called.");
            Console.WriteLine("Received payload named: {0}", payload.Name);
            payload.PropertyThatWillBeRemoved = 2;
            payload.Callback(this);
            return 0;
        }
    }
}