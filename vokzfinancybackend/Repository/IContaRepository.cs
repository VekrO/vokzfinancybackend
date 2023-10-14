using VokzFinancy.Models;

namespace VokzFinancy.Repository {

    public interface IContaRepository : IRepository<Conta> {

        Task<ICollection<Conta>> GetAllByIdUsuarioAsync(int idUsuario);
        Task<Conta> GetContaByIdAsync(int idUsuario);
        Task<double> GetDespesasByIdUsuarioAsync(int idConta);
        Task<double> GetReceitasByIdUsuarioAsync(int idConta);
           
    }

}