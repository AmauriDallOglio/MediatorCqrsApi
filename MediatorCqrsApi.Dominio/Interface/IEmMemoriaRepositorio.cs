using MediatorCqrsApi.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatorCqrsApi.Dominio.Interface
{
    public interface IEmMemoriaRepositorio
    {
        Task Save(Notificacao customer);
        Task<Notificacao> Find(Guid id);
    }
}
