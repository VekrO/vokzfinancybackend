using Microsoft.EntityFrameworkCore;
using VokzFinancy.Data;
using VokzFinancy.Models;
using vokzfinancybackend.Repository.Interfaces;

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

                List<Conta> contas = await _context.Contas.Where(x => x.UsuarioId == idUsuario).ToListAsync();

                double despesas = 0;

                foreach(Conta conta in contas)
                {

                    despesas += await _context.Despesas.Where(c => c.ContaId == conta.Id).SumAsync(x => x.Valor);

                }

                return despesas;

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<double> GetAllReceitasByIdUsuarioAsync(int idUsuario)
        {
            try
            {

                List<Conta> contas = await _context.Contas.Where(x => x.UsuarioId == idUsuario).ToListAsync();

                double receitas = 0;

                foreach (var conta in contas)
                {

                    receitas += await _context.Receitas.Where(c => c.ContaId == conta.Id).SumAsync(x => x.Valor);

                }

                return receitas;

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<double> GetAllDespesasByIdUsuarioAndIdContaAsync(int idUsuario, int idConta)
        {
            try
            {

                Conta conta = await _context.Contas.Where(x => x.UsuarioId == idUsuario && x.Id == idConta).FirstOrDefaultAsync();
                double despesas = await _context.Despesas.Where(x => x.Conta == conta).SumAsync(x => x.Valor);
                return despesas;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<double> GetAllReceitasByIdUsuarioAndIdContaAsync(int idUsuario, int idConta)
        {
            try
            {

                Conta conta = await _context.Contas.Where(x => x.UsuarioId == idUsuario && x.Id == idConta).FirstOrDefaultAsync();
                double receitas = await _context.Receitas.Where(x => x.Conta == conta).SumAsync(x => x.Valor);
                return receitas;

            }
            catch (Exception ex)
            {
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
                double despesas = await _context.Despesas.Where(x => x.Conta == conta && x.Paga == false).SumAsync(x => x.Valor);
                return receitas - despesas;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<double> GetSaldoByIdUsuarioAndIdContaAsync(int idUsuario, int idConta)
        {
            try
            {

                Conta conta = await _context.Contas.Where(x => x.Id == idConta && x.UsuarioId == idUsuario).FirstOrDefaultAsync();
                double receitas = await _context.Receitas.Where(x => x.Conta == conta).SumAsync(x => x.Valor);
                double despesas = await _context.Despesas.Where(x => x.Conta == conta && x.Paga == false).SumAsync(x => x.Valor);
                return receitas - despesas;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }

}