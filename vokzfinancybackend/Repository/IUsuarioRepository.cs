using VokzFinancy.DTOs;
using VokzFinancy.Models;

namespace VokzFinancy.Repository {

    public interface IUsuarioRepository : IRepository<Usuario> {

        Task<Usuario> GetContasByIdUsuarioAsync(int idUsuario);
        Task<Usuario> GetByEmail(string email);
        Task<Usuario> GetByResetPasswordToken(string token);   
        Task<Usuario> GetByVerificationEmailToken(string token);   
        Task<double> GetAllDespesasByIdUsuarioAsync(int idUsuario);
        Task<double> GetAllReceitasByIdUsuarioAsync(int idUsuario);
        Task<double> GetSaldoByIdUsuarioAsync(int idUsuario);

        
    }

}