using Domain.Entities;

namespace Business.DTO
{
    public class ForeignKeyManagerDTO<T> where T : BaseEntity
    {
        public T Entity { get; set; }
        public string DesiredFk { get; set; }
        public Guid? FkValue { get; set; }

        #region Constructors

        public ForeignKeyManagerDTO()
        {
            
        }

        public ForeignKeyManagerDTO(T entity, Guid? fkValue, string desiredFk)
        {
            Entity = entity;
            DesiredFk = desiredFk;
            FkValue = fkValue ?? Guid.Empty;
        }

        #endregion
    }
}