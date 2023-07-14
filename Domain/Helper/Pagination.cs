using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helper
{
    public class Pagination<T>
    {
        public IList<T> Entities { get; set; }
        public int NumberOfPages { get; set; }
        public int CurrentPage { get; set; }
        public int NumberOfEntities { get; set; }

        public Pagination()
        {

        }
        public Pagination(IList<T> entities, int numberOfPages, int currentPage, int numberOfEntities)
        {
            Entities = entities;
            NumberOfPages = numberOfPages;
            CurrentPage = currentPage;
            NumberOfEntities = numberOfEntities;
        }
    }
}
