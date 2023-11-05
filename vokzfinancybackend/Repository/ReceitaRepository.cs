using Microsoft.EntityFrameworkCore;
using VokzFinancy.Data;
using VokzFinancy.DTOs;
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

        public async Task<IEnumerable<Receita>> GetByIdContaAsync(int idConta, DateTime dtIni, DateTime dtFim) {

            try {
                
                 IEnumerable<Receita> receitas = await _context.Receitas.AsNoTracking().Where(x => x.ContaId == idConta && x.Data >= dtIni && x.Data <= dtFim).ToListAsync();
                 return receitas;

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }

    }

}