using System.Transactions;
using Microsoft.EntityFrameworkCore.Storage;
using VokzFinancy.Repository;
using vokzfinancybackend.Repository.Interfaces;

namespace VokzFinancy.Data
{

    public class UnitOfWork : IUnitOfWork {


        private BancoContext _context;
        private IDbContextTransaction _transaction;
        private UsuarioRepository _usuarioRepository;
        private ContaRepository _contaRepository;
        private DespesaRepository _despesaRepository;
        private ReceitaRepository _receitaRepository;

        public UnitOfWork(BancoContext context) {
            _context = context;
        }

        public IUsuarioRepository UsuarioRepository {
            get {
                return _usuarioRepository = _usuarioRepository ?? new UsuarioRepository(_context);
            }
        }
        
        public IContaRepository ContaRepository {
            get {
                return _contaRepository = _contaRepository ?? new ContaRepository(_context);
            }
        }

        public IDespesaRepository DespesaRepository {
            get {
                return _despesaRepository = _despesaRepository ?? new DespesaRepository(_context);
            }
        }
        
        public IReceitaRepository ReceitaRepository {

            get {
                return _receitaRepository = _receitaRepository ?? new ReceitaRepository(_context);
            }

        }

        public async Task BeginTransactionAsync() {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync() {
            await _transaction.CommitAsync();
        }

        public async Task RollbackAsync() {
            await _transaction.RollbackAsync();
        }

        public void Dispose() {
            _context.Dispose();
        }

    }

}