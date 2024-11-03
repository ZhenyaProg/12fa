namespace TFA.Application;

public interface IGuidFactory
{
    Guid Create();
}

public class GuidFactory : IGuidFactory
{
    public Guid Create()
    {
        return Guid.NewGuid();
    }
}