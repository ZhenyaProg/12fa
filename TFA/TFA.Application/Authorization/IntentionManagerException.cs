namespace TFA.Application.Authorization;

public class IntentionManagerException : Exception
{
    public IntentionManagerException() : base("Action is not allowed")
    {
        
    }
}