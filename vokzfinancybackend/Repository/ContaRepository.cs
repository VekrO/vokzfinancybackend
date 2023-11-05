using Microsoft.EntityFrameworkCore;
using VokzFinancy.Data;
using VokzFinancy.DTOs;
using VokzFinancy.Models;
using vokzfinancybackend.Repository.Interfaces;

namespace VokzFinancy.Repository
{

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
                Conta conta = await _context.Contas.Include(c => c.Usuario).Include(c => c.Despesas).Include(c => c.Receitas).FirstOrDefaultAsync(c => c.Id == id);
                return conta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<double> GetDespesasByIdContaAsync(int idConta)
        {   

            try {
                
                double despesas = await _context.Despesas.Where(x => x.ContaId == idConta).SumAsync(x => x.Valor);
                return despesas;

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }

        public async Task<double> GetReceitasByIdContaAsync(int idConta)
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

                Conta conta = await _context.Contas.Where(c => c.UsuarioId == idUsuario && c.Padrao).FirstOrDefaultAsync();
                return conta;    

            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ReceitaDespesaDTO> GetReceitaDespesaByIdContaAsync(int idConta)
        {
            try
            {

                ReceitaDespesaDTO receitasDespesas = await _context.Contas.Where(c => c.Id == idConta).Select(c => new ReceitaDespesaDTO
                {
                    Conta = c,
                    ValorDespesa = c.Despesas.Sum(x => x.Valor),
                    ValorReceita = c.Receitas.Sum(x => x.Valor)
                }).FirstOrDefaultAsync();

                return receitasDespesas;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ReceitaDespesaDTO> GetReceitaDespesaByIdUsuarioAsync(int idUsuario)
        {
            try
            {

                double valorDespesa = 0;
                double valorReceita = 0;

                List<Conta> contas = await _context.Contas.Where(c => c.UsuarioId == idUsuario).Include(c => c.Despesas).Include(c => c.Receitas).ToListAsync();

                foreach (Conta conta in contas)
                {

                    if(conta.Despesas.Any() || conta.Receitas.Any())
                    {

                        valorDespesa += conta.Despesas.Sum(x => x.Valor);
                        valorReceita += conta.Receitas.Sum(x => x.Valor);

                    }

                }

                return new ReceitaDespesaDTO
                {
                    Conta = new Conta { Nome = "Todas" },
                    ValorDespesa = valorDespesa,
                    ValorReceita = valorReceita
                };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }

}