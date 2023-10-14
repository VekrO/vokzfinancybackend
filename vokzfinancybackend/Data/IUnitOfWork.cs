using Microsoft.EntityFrameworkCore.Storage;
using VokzFinancy.Repository;

namespace VokzFinancy.Data {

    public interface IUnitOfWork : IDisposable {

        IUsuarioRepository UsuarioRepository { get; }
        IContaRepository ContaRepository { get; }
        IDespesaRepository DespesaRepository {get; }
        IReceitaRepository ReceitaRepository {get;} 
        Task CommitAsync();        
        Task BeginTransactionAsync();        
        Task RollbackAsync();        
        void Dispose();

    }

}