namespace TFA.Application;

public interface IMomentProvider
{
    DateTimeOffset Now { get; }
}

public class MomentProvider : IMomentProvider
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}