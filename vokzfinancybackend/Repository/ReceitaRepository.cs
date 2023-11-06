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

        public async Task<IEnumerable<Receita>> GetAllReceitasByIdUsuarioAsync(int idUsuario, DateTime dtIni, DateTime dtFim)
        {
            try
            {

                // Encontra todas as contas do usuário baseando-se no id dele.
                IEnumerable<Conta> contas = await _context.Contas.Include(x => x.Receitas).Where(x => x.UsuarioId == idUsuario).ToListAsync();

                List<Receita> receitas = new List<Receita>();

                foreach (Conta conta in contas)
                {

                    if (conta.Receitas.Any())
                    {

                        foreach (Receita receita in conta.Receitas)
                        {
                            if (receita.Data >= dtIni && receita.Data <= dtFim)
                            {
                                receitas.Add(receita);
                            }
                        }

                    }

                }

                if (receitas.Count > 0)
                {
                    receitas.OrderBy(x => x.Data);
                }


                return receitas;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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