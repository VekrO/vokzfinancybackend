using Microsoft.EntityFrameworkCore;
using VokzFinancy.Data;
using VokzFinancy.Models;
using vokzfinancybackend.Repository.Interfaces;

namespace VokzFinancy.Repository
{

    public class DespesaRepository : Repository<Despesa>, IDespesaRepository {

        private readonly BancoContext _context;
        public DespesaRepository(BancoContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Despesa>> GetByIdContaAsync(int idConta) {

            try {
                IEnumerable<Despesa> despesas = await _context.Despesas.AsNoTracking().Where(x => x.ContaId == idConta).ToListAsync();
                return despesas;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }

        public async Task<double> GetValorByIdContaAsync(int idConta)
        {
            try {
                double valor = await _context.Despesas.Where(x => x.ContaId == idConta).AsNoTracking().SumAsync(x => x.Valor);
                return valor;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Despesa>> GetVencidoByIdContaAsync(int idConta)
        {   
            
            try {
                IEnumerable<Despesa> despesas = await _context.Despesas.AsNoTracking().Where(x => x.ContaId == idConta && DateTime.UtcNow.Date > x.Vencimento).ToListAsync();
                return despesas;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

            throw new NotImplementedException();
        }


    }

}