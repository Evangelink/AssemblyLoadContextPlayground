namespace Lib
{
    public interface IA
    {
        Out MyMethod(In @in);
    }

    public class In { }
    public class Out { }

    public class A : IA
    {
        public Out MyMethod(In @in)
        {
            throw new NotImplementedException();
        }
    }
}