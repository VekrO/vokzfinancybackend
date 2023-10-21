using Microsoft.EntityFrameworkCore;
using VokzFinancy.Data;
using VokzFinancy.Models;
using vokzfinancybackend.Repository.Interfaces;

namespace VokzFinancy.Repository
{

    public class ReceitaRepository : Repository<Receita>, IReceitaRepository
    {

        private readonly BancoContext _context;

        public ReceitaRepository(BancoContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Receita>> GetByIdContaAsync(int idConta) {

            try {
                
                 IEnumerable<Receita> receitas = await _context.Receitas.AsNoTracking().Where(x => x.ContaId == idConta).ToListAsync();
                 return receitas;

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }

        public async Task<double> GetValorByIdUsuarioAsync(int idConta)
        {
            try {
                double valor = await _context.Receitas.Where(x => x.ContaId == idConta).AsNoTracking().SumAsync(x => x.Valor);
                return valor;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

    }

}