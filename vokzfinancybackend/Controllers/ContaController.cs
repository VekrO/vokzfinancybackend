using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using VokzFinancy.Data;
using VokzFinancy.DTOs;
using VokzFinancy.Models;

namespace VokzFinancy.Controllers
{

    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/[controller]")]
    public class ContaController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContaController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}/usuario/{idUsuario}")]
        public async Task<ActionResult<Conta>> GetByIdAsync(int id, int idUsuario)
        {

            try
            {

                Conta conta = await _unitOfWork.ContaRepository.GetByIdAsync(x => x.Id == id && x.UsuarioId == idUsuario);
                if (conta == null)
                {
                    return NotFound("Nenhuma conta foi encontrada com esse ID!");
                }

                return Ok(conta);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpPost]
        public async Task<ActionResult<Conta>> Post(ContaDTO model)
        {

            try
            {

                // Validar se o usuário existe.
                Usuario usuario = await _unitOfWork.UsuarioRepository.GetContasByIdUsuarioAsync(model.UsuarioId);
                if (usuario == null)
                {
                    return BadRequest("O usuário não existe!");
                }

                // Verifica se já existe uma conta padrão para o usuário.
                if(model.Padrao)
                {

                    Conta contaPadrao = await _unitOfWork.ContaRepository.GetContaPadraoByIdUsuarioAsync(model.UsuarioId);
                    if(contaPadrao != null)
                    {
                        return BadRequest("Você já possuí uma conta padrão!");
                    }

                }

                Conta conta = new Conta()
                {
                    Nome = model.Nome,
                    Usuario = usuario,
                    Padrao = model.Padrao,
                    Descricao = model.Descricao,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.ContaRepository.Add(conta);

                return Ok(conta);

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ContaDTO>> Put(int id, ContaDTO model) {

            try {

                if(id != model.Id) {
                    return BadRequest("Confira os dados enviados e tente novamente!");
                } 
                
                Conta contaDb = await _unitOfWork.ContaRepository.GetByIdAsync(x => x.Id == id);

                if (contaDb == null)
                {
                    return NotFound("Nenhum registro encontrado!");
                }

                // Verifica se já existe uma conta padrão para o usuário.
                if (model.Padrao)
                {

                    Conta contaPadrao = await _unitOfWork.ContaRepository.GetContaPadraoByIdUsuarioAsync((int)model.UsuarioId);
                    if (contaPadrao != null && model.Id != contaPadrao.Id)
                    {
                        return BadRequest("Você já possuí uma conta padrão!");
                    }
                }

                contaDb.Nome = model.Nome;
                contaDb.Descricao = model.Descricao;
                contaDb.Padrao = model.Padrao;
                contaDb.UpdatedAt = DateTime.UtcNow;
   
                await _unitOfWork.ContaRepository.Update(contaDb);

                ContaDTO contaDto = _mapper.Map<Conta, ContaDTO>(contaDb);

                return Ok(contaDto);

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult<ICollection<Conta>>> GetAllByIdUsuarioAsync(int idUsuario) {

            try {   

                ICollection<Conta> contas = await _unitOfWork.ContaRepository.GetAllByIdUsuarioAsync(idUsuario);
                ICollection<ContaDTO> contasDTO = _mapper.Map<ICollection<Conta>, ICollection<ContaDTO>>(contas);
                return Ok(contasDTO);

            } catch (Exception ex) {

                throw new Exception(ex.Message);

            }

        }

        [HttpGet("{id}/despesas/valor")]
        public async Task<ActionResult<double>> GetDespesasByIdContaAsync(int id) {

            try {   

                double despesas = await _unitOfWork.ContaRepository.GetDespesasByIdContaAsync(id);

                return Ok(despesas);

            } catch (Exception ex) {

                throw new Exception(ex.Message);

            }

        }

        [HttpGet("{id}/receitas/valor")]
        public async Task<ActionResult<double>> GetReceitasByIdContaAsync(int id) {

            try {   

                double receitas = await _unitOfWork.ContaRepository.GetReceitasByIdContaAsync(id);
                return Ok(receitas);

            } catch (Exception ex) {

                throw new Exception(ex.Message);

            }

        }

        [HttpDelete("{id}/usuario/{idUsuario}")]
        public async Task<ActionResult> Delete(int id, int idUsuario)
        {

            try
            {

                Conta conta = await _unitOfWork.ContaRepository.GetContaByIdAsync(id);

                if (conta == null)
                {
                    return NotFound("Nenhum registro encontrado!");
                }
                
                if (conta.UsuarioId != idUsuario)
                {
                    return NotFound("Você não tem permissão para excluir esse registro!");
                }

                Usuario usuario = await _unitOfWork.UsuarioRepository.GetContasByIdUsuarioAsync(idUsuario);
                
                if(usuario.Contas.Count() <= 1) {
                    return BadRequest("Você não pode excluir sua única conta!");
                }

                await _unitOfWork.BeginTransactionAsync();

                if(conta.Despesas.Any() || conta.Receitas.Any())
                {

                    // Crie cópias das despesas e receitas
                    var despesas = conta.Despesas.ToList();
                    var receitas = conta.Receitas.ToList();

                    foreach (Despesa item in despesas)
                    {
                        await _unitOfWork.DespesaRepository.Delete(item);
                    }

                    foreach (Receita item in receitas)
                    {
                        await _unitOfWork.ReceitaRepository.Delete(item);
                    }

                }

                conta.Usuario = null;
                conta.UsuarioId = null;

                await _unitOfWork.ContaRepository.Delete(conta);
                await _unitOfWork.CommitAsync();

                return Ok();

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception(ex.Message);
            }

        }

        [HttpGet("usuario/{idUsuario}/padrao")]
        public async Task<ActionResult<ContaDTO>> GetContaPadraoByIdUsuarioAsync(int idUsuario)
        {

            try
            {

                Conta conta = await _unitOfWork.ContaRepository.GetContaPadraoByIdUsuarioAsync(idUsuario);
                if(conta == null)
                {
                    return BadRequest("Nenhuma conta foi encontrada para esse usuário!");
                }
                ContaDTO contaDto = _mapper.Map<Conta, ContaDTO>(conta);
                return Ok(contaDto);

            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpGet("{id}/valores")]
        public async Task<ActionResult<ReceitaDespesaDTO>> GetReceitaDespesaByIdContaAsync(int id)
        {

            try
            {

                ReceitaDespesaDTO valores = await _unitOfWork.ContaRepository.GetReceitaDespesaByIdContaAsync(id);
                return Ok(valores);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);

            }

        }

        [HttpGet("usuario/{idUsuario}/valores")]
        public async Task<ActionResult<ReceitaDespesaDTO>> GetReceitaDespesaByIdUsuarioAsync(int idUsuario)
        {

            try
            {

                ReceitaDespesaDTO valores = await _unitOfWork.ContaRepository.GetReceitaDespesaByIdUsuarioAsync(idUsuario);
                return Ok(valores);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);

            }

        }

    }

}