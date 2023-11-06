using VokzFinancy.DTOs;
using VokzFinancy.Models;

namespace vokzfinancybackend.Repository.Interfaces
{

    public interface IDespesaRepository : IRepository<Despesa>
    {

        Task<IEnumerable<Despesa>> GetByIdContaAsync(int idConta, DateTime dtIni, DateTime dtFim);
        Task<IEnumerable<Despesa>> GetVencidoByIdContaAsync(int idConta, DateTime dtIni, DateTime dtFim);
        Task<IEnumerable<Despesa>> GetAllDespesasByIdUsuarioAsync(int idUsuario, DateTime dtIni, DateTime dtFim);

    }

}