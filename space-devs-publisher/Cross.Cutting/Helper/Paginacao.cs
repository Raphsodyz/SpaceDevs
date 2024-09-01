using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cross.Cutting.Helper
{
    public class Paginacao<T>
    {
        public IList<T> Entidades { get; set; }
        public int NumeroDePaginas { get; set; }
        public int PaginaAtual { get; set; }
        public int TotalDeResultados { get; set; }
        
        public Paginacao(IList<T> entidades, int numeroDePaginas, int paginaAtual, int totalDeResultados)
        {
            Entidades = entidades;
            NumeroDePaginas = numeroDePaginas;
            PaginaAtual = paginaAtual;
            TotalDeResultados = totalDeResultados;
        }
    }
}