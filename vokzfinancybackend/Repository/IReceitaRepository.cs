using VokzFinancy.Models;

namespace VokzFinancy.Repository {

    public interface IReceitaRepository : IRepository<Receita> {

        Task<IEnumerable<Receita>> GetByIdContaAsync(int idUsuario);
        Task<double> GetValorByIdUsuarioAsync(int idUsuario);

    }

}