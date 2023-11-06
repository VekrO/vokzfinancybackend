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

        public async Task<IEnumerable<Despesa>> GetAllDespesasByIdUsuarioAsync(int idUsuario, DateTime dtIni, DateTime dtFim)
        {
            try
            {

                // Encontra todas as contas do usuário baseando-se no id dele.
                IEnumerable<Conta> contas = await _context.Contas.Include(x => x.Despesas).Where(x => x.UsuarioId == idUsuario).ToListAsync();

                List<Despesa> despesas = new List<Despesa>();

                foreach(Conta conta in contas)
                {

                    if(conta.Despesas.Any())
                    {
                        
                        foreach(Despesa despesa in conta.Despesas)
                        {
                            if(despesa.Vencimento >= dtIni && despesa.Vencimento <= dtFim)
                            {
                                despesas.Add(despesa);
                            }
                        }

                    }

                }

                if(despesas.Count > 0)
                {
                    despesas.OrderBy(x => x.Vencimento);
                }

 
                return despesas;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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