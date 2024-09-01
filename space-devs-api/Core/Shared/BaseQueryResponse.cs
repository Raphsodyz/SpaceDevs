namespace Core.Shared
{
    public abstract class BaseQueryResponse<TData>
    {
        public virtual bool Success { get; set; }
        public virtual string Error { get; set; }
        public virtual TData Data { get; set; }
    }
}