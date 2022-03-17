using System;

namespace Contract
{
    public interface IPayload
    {
        string Name { get; }

        // Removing (commenting) this causes an exception at runtime because it is used
        // by ClientV1 and ClientV2.
        // We cannot remove something that might be used by the client.
        int PropertyThatWillBeRemoved { get; set; }

        // Uncomment below when building ClientV2
        string NewProperty { get; set; }

        void Callback(ICommand command)
        {
            throw new NotImplementedException("Default interface implementation");
        }
    }

    public interface ICommand
    {
        int MethodWithPayload(IPayload payload)
        {
            throw new NotImplementedException("Default interface implementation");
        }

        int MyMethod()
        {
            throw new NotImplementedException("Default interface implementation");
        }

        // Comment below when building ClientV3
        //int MyMethodThatWillBeRemoved()
        //{
        //    throw new NotImplementedException("Default interface implementation");
        //}

        // Uncomment below when building ClientV2
        int MyNewMethod(object arg)
        {
            throw new NotImplementedException("Default interface implementation");
        }
    }
}