using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VokzFinancy.Data;
using VokzFinancy.DTOs;
using VokzFinancy.Models;

namespace VokzFinancy.Controllers {

    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/[controller]")]
    public class DespesaController : ControllerBase {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DespesaController(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DespesaDTO>> GetByIdAsync(int id) {

            try {

                Despesa despesa = await _unitOfWork.DespesaRepository.GetByIdAsync(x => x.Id == id);
                if(despesa == null) {
                    return NotFound("Registro não encontrado!");
                }
                DespesaDTO despesaDto = _mapper.Map<Despesa, DespesaDTO>(despesa);

                return Ok(despesaDto);

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }

        [HttpPost]
        public async Task<ActionResult<Despesa>> Post(DespesaDTO model) {

            try {
                
                Console.WriteLine(model.ContaId);

                Despesa despesa = _mapper.Map<DespesaDTO, Despesa>(model);
                Conta conta = await _unitOfWork.ContaRepository.GetByIdAsync(x => x.Id == model.ContaId);
                
                if(conta == null) {
                    return NotFound("A conta informada não existe!");
                }

                despesa.ContaId = conta.Id;
                despesa.CreatedAt = DateTime.UtcNow;
                despesa.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.DespesaRepository.Add(despesa);

                DespesaDTO result = _mapper.Map<Despesa, DespesaDTO>(despesa);

                return Ok(result);

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DespesaDTO>> Put(int id, DespesaDTO model) {
            
            try {

                if(id != model.Id) {
                    return BadRequest("Confira os dados enviados e tente novamente!");
                }    

                Despesa despesaDb = await _unitOfWork.DespesaRepository.GetByIdAsync(x => x.Id == id);
            
                if(despesaDb == null) {
                    return NotFound("Nenhum registro encontrado!");
                }

                Despesa despesa = _mapper.Map(model, despesaDb);
                despesa.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.DespesaRepository.Update(despesa);

                DespesaDTO despesaDto = _mapper.Map<Despesa, DespesaDTO>(despesa);
                return Ok(despesaDto);

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }

        [HttpDelete("{id}/conta/{idConta}")]
        public async Task<ActionResult> Delete(int id, int idConta) {
            try {

                Despesa despesa = await _unitOfWork.DespesaRepository.GetByIdAsync(x => x.Id == id);
                if(despesa == null) {
                    return NotFound("Nenhum registro encontrado!");
                }

                if(despesa.ContaId != idConta) {
                    return NotFound("Você não tem permissão para excluir esse registro!");
                }

                await _unitOfWork.DespesaRepository.Delete(despesa);

                return Ok();

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("conta/{idConta}")]
        public async Task<ActionResult<IEnumerable<DespesaDTO>>> GetByIdContaAsync(int idConta) {

            try {

                IEnumerable<Despesa> despesas = await _unitOfWork.DespesaRepository.GetByIdContaAsync(idConta);
                IEnumerable<DespesaDTO> despesasDto = _mapper.Map<IEnumerable<Despesa>, IEnumerable<DespesaDTO>>(despesas);

                return Ok(despesasDto);

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }

        [HttpGet("conta/{idConta}/vencido")]
        public async Task<ActionResult<IEnumerable<DespesaDTO>>> GetVencidoByIdContaAsync(int idConta) {
            try {
                IEnumerable<Despesa> despesas = await _unitOfWork.DespesaRepository.GetVencidoByIdContaAsync(idConta);
                IEnumerable<DespesaDTO> despesasDto = _mapper.Map<IEnumerable<Despesa>, IEnumerable<DespesaDTO>>(despesas);
                return Ok(despesasDto);
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("conta/{idConta}/valor")]
        public async Task<ActionResult<double>> GetValorByIdContaAsync(int idConta) {
            try {
                double valor = await _unitOfWork.DespesaRepository.GetValorByIdContaAsync(idConta);
                return Ok(valor);
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

    }

}