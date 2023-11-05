using VokzFinancy.DTOs;
using VokzFinancy.Models;

namespace vokzfinancybackend.Repository.Interfaces
{

    public interface IReceitaRepository : IRepository<Receita>
    {

        Task<IEnumerable<Receita>> GetByIdContaAsync(int idUsuario, DateTime dtIni, DateTime dtFim);

    }

}