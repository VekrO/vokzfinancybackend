using VokzFinancy.Models;

namespace vokzfinancybackend.Repository.Interfaces
{

    public interface IContaRepository : IRepository<Conta>
    {

        Task<ICollection<Conta>> GetAllByIdUsuarioAsync(int idUsuario);
        Task<Conta> GetContaByIdAsync(int idUsuario);
        Task<double> GetDespesasByIdUsuarioAsync(int idConta);
        Task<double> GetReceitasByIdUsuarioAsync(int idConta);
        Task<Conta> GetContaPadraoByIdUsuarioAsync(int idUsuario);


    }

}