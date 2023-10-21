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
    public class ReceitaController : ControllerBase {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public ReceitaController(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }   

        [HttpGet("{id}/usuario/{idUsuario}")]
        public async Task<ActionResult<ReceitaDTO>> GetByIdAsync(int id, int idUsuario) {
            try {
                Receita receita = await _unitOfWork.ReceitaRepository.GetByIdIncludesAsync(x => x.Id == id, x => x.Conta);
                if (receita.Conta.UsuarioId != idUsuario)
                {
                    return BadRequest("Você não tem permissão para acessar esse registro!");
                }
                if (receita == null) {
                    return NotFound("Nenhum registro foi encontrado!");
                }
                ReceitaDTO receitaDto = _mapper.Map<Receita, ReceitaDTO>(receita);
                return Ok(receitaDto);
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ReceitaDTO>> Post(ReceitaDTO model) {

            try
            {   

                Console.WriteLine("Model: " + model.ContaId);

                Receita receita = _mapper.Map<ReceitaDTO, Receita>(model);
                Conta conta = await _unitOfWork.ContaRepository.GetByIdAsync(x => x.Id == model.ContaId);
                if(conta == null) {
                    return NotFound("A conta não foi encontrado!");
                }
                receita.ContaId = conta.Id;
                receita.CreatedAt = DateTime.UtcNow;
                receita.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.ReceitaRepository.Add(receita);

                ReceitaDTO result = _mapper.Map<Receita, ReceitaDTO>(receita);

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ReceitaDTO>> Put(int id, ReceitaDTO model) {

            try {

                if(id != model.Id) {
                    return BadRequest("Confira os dados enviados e tente novamente!");
                }
                
                Receita receitaDb = await _unitOfWork.ReceitaRepository.GetByIdAsync(x => x.Id == id);

                if(receitaDb == null) {
                    return NotFound("Nenhum registro encontrado!");
                }

                Receita receita = _mapper.Map(model, receitaDb);
                receita.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.ReceitaRepository.Update(receita);

                ReceitaDTO receitaDto = _mapper.Map<Receita, ReceitaDTO>(receita);
                return Ok(receitaDto);
                

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }

        [HttpDelete("{id}/conta/{idConta}")]
        public async Task<ActionResult> Delete(int id, int idConta) {
            try {

                Receita receita = await _unitOfWork.ReceitaRepository.GetByIdAsync(x => x.Id == id);
                if(receita == null) {
                    return NotFound("Registro não encontrado!");
                }

                if(receita.ContaId != idConta) {
                    return NotFound("Você não tem permissão para excluir esse registro!");
                }

                await _unitOfWork.ReceitaRepository.Delete(receita);
                
                return Ok();

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }


        [HttpGet("conta/{idConta}")]
        public async Task<ActionResult<IEnumerable<DespesaDTO>>> GetByIdUsuarioAsync(int idConta) {

            try {
                IEnumerable<Receita> receitas = await _unitOfWork.ReceitaRepository.GetByIdContaAsync(idConta);
                IEnumerable<ReceitaDTO> receitasDto = _mapper.Map<IEnumerable<Receita>, IEnumerable<ReceitaDTO>>(receitas);
                return Ok(receitasDto);
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }

        [HttpGet("usuario/{idUsuario}/valor")]
        public async Task<ActionResult<double>> GetValorByIdUsuarioAsync(int idUsuario) {
            try {
                double valor = await _unitOfWork.ReceitaRepository.GetValorByIdUsuarioAsync(idUsuario);
                return Ok(valor);
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

    }

}