namespace Domain.Shared
{
    public abstract class BaseResponse<TData>
    {
        public virtual bool Success { get; set; }
        public virtual string Error { get; set; }
        public virtual TData Data { get; set; }
    }
}