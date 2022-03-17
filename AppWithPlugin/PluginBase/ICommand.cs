namespace PluginBase
{
    public static class S
    {
        public static int Count = 0;
    }

    public interface ICommand
    {
        string Name { get; }
        string Description { get; }

        int Execute();
    }
}
