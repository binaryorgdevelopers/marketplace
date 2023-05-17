namespace Ordering.Application;

public class Program
{
    public static readonly string Namespace = typeof(Program).Namespace;
    public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}