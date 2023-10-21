using VokzFinancy.Models;

namespace vokzfinancybackend.Repository.Interfaces
{

    public interface IReceitaRepository : IRepository<Receita>
    {

        Task<IEnumerable<Receita>> GetByIdContaAsync(int idUsuario);
        Task<double> GetValorByIdUsuarioAsync(int idUsuario);

    }

}