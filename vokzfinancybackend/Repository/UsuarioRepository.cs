using Microsoft.EntityFrameworkCore;
using VokzFinancy.Data;
using VokzFinancy.Models;

namespace VokzFinancy.Repository
{

    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {

        private readonly BancoContext _context;

        public UsuarioRepository(BancoContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Usuario> GetContasByIdUsuarioAsync(int idUsuario)
        {
            try
            {
                
                Usuario usuario = await _context.Usuarios.Include(x => x.Contas).Where(x => x.Id == idUsuario).FirstOrDefaultAsync();
                return usuario;

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<double> GetAllDespesasByIdUsuarioAsync(int idUsuario)
        {
            try
            {
                
                Conta conta = await _context.Contas.Where(x => x.UsuarioId == idUsuario).FirstOrDefaultAsync();
                double despesas = await _context.Despesas.Where(x => x.Conta == conta).SumAsync(x => x.Valor);
                return despesas;

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<double> GetAllReceitasByIdUsuarioAsync(int idUsuario)
        {
            try
            {
                
                Conta conta = await _context.Contas.Where(x => x.UsuarioId == idUsuario).FirstOrDefaultAsync();
                double receitas = await _context.Receitas.Where(x => x.Conta == conta).SumAsync(x => x.Valor);
                return receitas;

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Usuario> GetByEmail(string email)
        {
            Usuario usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);
            return usuario;
        }

        public async Task<Usuario> GetByResetPasswordToken(string token)
        {
            Usuario usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.ResetPasswordToken == token);
            return usuario;
        }

        public async Task<Usuario> GetByVerificationEmailToken(string token)
        {
            Usuario usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.VerificationEmailToken == token);
            return usuario;
        }

        public async Task<double> GetSaldoByIdUsuarioAsync(int idUsuario)
        {
            try
            {

                Conta conta = await _context.Contas.Where(x => x.UsuarioId == idUsuario).FirstOrDefaultAsync();
                double receitas = await _context.Receitas.Where(x => x.Conta == conta).SumAsync(x => x.Valor);
                double despesas = await _context.Despesas.Where(x => x.Conta == conta).SumAsync(x => x.Valor);
                return receitas - despesas;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }

}