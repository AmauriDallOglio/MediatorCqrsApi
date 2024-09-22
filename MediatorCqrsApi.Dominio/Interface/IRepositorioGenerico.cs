using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatorCqrsApi.Dominio.Interface
{
    public interface IRepositorioGenerico<TEntity> where TEntity : class
    {
        TEntity Inserir(TEntity entidade, bool finalizar);

        void Comitar();
    }
}
