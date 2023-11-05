using VokzFinancy.DTOs;
using VokzFinancy.Models;

namespace vokzfinancybackend.Repository.Interfaces
{

    public interface IContaRepository : IRepository<Conta>
    {

        Task<ICollection<Conta>> GetAllByIdUsuarioAsync(int idUsuario);
        Task<Conta> GetContaByIdAsync(int idUsuario);
        Task<double> GetDespesasByIdContaAsync(int idConta);
        Task<double> GetReceitasByIdContaAsync(int idConta);
        Task<Conta> GetContaPadraoByIdUsuarioAsync(int idUsuario);

        Task<ReceitaDespesaDTO> GetReceitaDespesaByIdContaAsync(int idConta);
        Task<ReceitaDespesaDTO> GetReceitaDespesaByIdUsuarioAsync(int idUsuario);

    }

}