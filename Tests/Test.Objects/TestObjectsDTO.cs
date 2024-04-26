namespace Tests.Test.Objects
{
    public static class TestObjectsDTO
    {
        public class OnlyIdAndDateDTO
        {
            public Guid Id { get; set; }
            public DateTime WindowStart { get; set; }

            #region Constructors

            public OnlyIdAndDateDTO()
            {
                
            }

            public OnlyIdAndDateDTO(Guid id, DateTime windowsStart)
            {
                Id = id;
                WindowStart = windowsStart;
            }

            #endregion
        }

        public class OnlyIdAndPadNameDTO
        {
            public Guid IdPad { get; set; }
            public string PadName { get; set; }
        }
    }  
}