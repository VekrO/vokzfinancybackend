using VokzFinancy.Models;

namespace VokzFinancy.Repository {

    public interface IDespesaRepository : IRepository<Despesa> {

        Task<IEnumerable<Despesa>> GetByIdContaAsync(int idUsuario);
        Task<IEnumerable<Despesa>> GetVencidoByIdContaAsync(int idUsuario);
        Task<double> GetValorByIdContaAsync(int idUsuario);

    }

}