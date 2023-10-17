using Microsoft.EntityFrameworkCore;
using VokzFinancy.Data;
using VokzFinancy.Models;

namespace VokzFinancy.Repository {

    public class ContaRepository : Repository<Conta>, IContaRepository
    {

        private readonly BancoContext _context;
        public ContaRepository(BancoContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ICollection<Conta>> GetAllByIdUsuarioAsync(int idUsuario)
        {
            try
            {
                ICollection<Conta> contas = await _context.Contas.Include(c => c.Usuario).Where(c => c.UsuarioId == idUsuario).ToListAsync();
                return contas;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Conta> GetContaByIdAsync(int id)
        {
            try
            {
                Conta conta = await _context.Contas.Include(c => c.Usuario).FirstOrDefaultAsync(c => c.Id == id);
                return conta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<double> GetDespesasByIdUsuarioAsync(int idConta)
        {   

            try {
                
                double despesas = await _context.Despesas.Where(x => x.ContaId == idConta).SumAsync(x => x.Valor);
                return despesas;

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }

        public async Task<double> GetReceitasByIdUsuarioAsync(int idConta)
        {

            try
            {

                double receitas = await _context.Receitas.Where(x => x.ContaId == idConta).SumAsync(x => x.Valor);
                return receitas;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
            
        public async Task<Conta> GetContaPadraoByIdUsuarioAsync(int idUsuario)
        {
            try
            {

                Conta conta = await _context.Contas.Where(c => c.UsuarioId == idUsuario).FirstOrDefaultAsync();
                return conta;    

            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        } 

    }

}