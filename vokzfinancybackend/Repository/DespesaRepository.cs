using Microsoft.EntityFrameworkCore;
using VokzFinancy.Data;
using VokzFinancy.DTOs;
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

        public async Task<IEnumerable<Despesa>> GetByIdContaAsync(int idConta, DateTime dtIni, DateTime dtFim) {

            try {
                IEnumerable<Despesa> despesas = await _context.Despesas.AsNoTracking().Where(x => x.ContaId == idConta && x.Vencimento >= dtIni && x.Vencimento <= dtFim).OrderBy(x => x.Id).ToListAsync();
                return despesas;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }

        public async Task<IEnumerable<Despesa>> GetVencidoByIdContaAsync(int idConta, DateTime dtIni, DateTime dtFim)
        {   
            
            try {
                IEnumerable<Despesa> despesas = await _context.Despesas.AsNoTracking().Where(x => x.ContaId == idConta && x.Vencimento >= dtIni && x.Vencimento <= dtFim && DateTime.UtcNow.Date > x.Vencimento).ToListAsync();
                return despesas;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }

    }

}