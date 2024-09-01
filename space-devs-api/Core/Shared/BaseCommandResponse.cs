namespace Core.Shared
{
    public abstract class BaseCommandResponse
    {
        public virtual bool Success { get; set; }
        public virtual string Error { get; set; }
    }
}