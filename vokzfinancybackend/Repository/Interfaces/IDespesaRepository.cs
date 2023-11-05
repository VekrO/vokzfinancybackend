using VokzFinancy.DTOs;
using VokzFinancy.Models;

namespace vokzfinancybackend.Repository.Interfaces
{

    public interface IDespesaRepository : IRepository<Despesa>
    {

        Task<IEnumerable<Despesa>> GetByIdContaAsync(int idUsuario, DateTime dtIni, DateTime dtFim);
        Task<IEnumerable<Despesa>> GetVencidoByIdContaAsync(int idUsuario, DateTime dtIni, DateTime dtFim);

    }

}