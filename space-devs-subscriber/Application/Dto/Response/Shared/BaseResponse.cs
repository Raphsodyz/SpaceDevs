namespace Application.Dto.Response.Shared
{
    public abstract class BaseResponse<TDado>
    {
        public string Mensagem { get; init; }
        public bool Sucesso { get; init; }
        public TDado Dado { get; init; }
    }
}