using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using VokzFinancy.Data;
using VokzFinancy.DTOs;
using VokzFinancy.Models;


namespace VokzFinancy.Controllers {

    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/[controller]")]
    public class UsuarioController : ControllerBase {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsuarioController(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> GetByIdAsync(int id) {
           Usuario usuario = await _unitOfWork.UsuarioRepository.GetByIdAsync(x => x.Id == id);
           if(usuario == null) {
                return NotFound("Registro não encontrado!");
           }
           UsuarioDTO usuarioDTO = _mapper.Map<Usuario, UsuarioDTO>(usuario);
           return Ok(usuarioDTO);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetAllAsync() {
            IEnumerable<Usuario> usuarios = await _unitOfWork.UsuarioRepository.GetAllAsync();
            return Ok(usuarios);
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> Post(Usuario usuario) {

            try {
                
                await _unitOfWork.UsuarioRepository.Add(usuario);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = usuario.Id }, usuario);

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        } 

        [HttpPut("{id}")]
        public async Task<ActionResult<Usuario>> Update(int id, Usuario usuario) {

            try {

                if(id != usuario.Id) {
                    return BadRequest("Confira os dados enviados e tente novamente!");
                }

                // Checar se o usuário existe no banco de dados...
                Usuario usuarioDb = await _unitOfWork.UsuarioRepository.GetByIdAsync(x => x.Id == usuario.Id);
                if(usuarioDb == null) {
                    return NotFound("Usuário não encontrado!");
                }

                // Atualizar o usuário.
                await _unitOfWork.UsuarioRepository.Update(usuario);
                
                return Ok(usuario);

                
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
               
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id) {
            
            try {

                Usuario usuario = await _unitOfWork.UsuarioRepository.GetByIdAsync(x => x.Id == id);
                if(usuario == null) {
                    return NotFound("Usuário não encontrado!");
                }

                await _unitOfWork.UsuarioRepository.Delete(usuario);

                return Ok();

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }

        [HttpPatch("{id}/perfil")]
        public async Task<ActionResult<Usuario>> Patch([FromRoute] int id, UsuarioDTO usuario) {

            try {

                if(id != usuario.Id) {
                    return BadRequest("Confira os dados enviados e tente novamente!");
                }

                // Checar se o usuário existe no banco de dados...
                Usuario usuarioDb = await _unitOfWork.UsuarioRepository.GetByIdAsync(x => x.Id == usuario.Id);
                if(usuarioDb == null) {
                    return NotFound("Usuário não encontrado!");
                }
                
                usuarioDb.Name = usuario.Name;

                // Atualizar o usuário.
                await _unitOfWork.UsuarioRepository.Update(usuarioDb);
                
                return Ok(usuarioDb);

                
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
               
        }

        [HttpGet("{id}/despesas/valor")]
        public async Task<ActionResult<double>> GetAllDespesasByIdUsuarioAsync(int id)
        {

            try
            {

                double despesas = await _unitOfWork.UsuarioRepository.GetAllDespesasByIdUsuarioAsync(id);

                return Ok(despesas);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);

            }

        }

        [HttpGet("{id}/receitas/valor")]
        public async Task<ActionResult<double>> GetAllReceitasByIdUsuarioAsync(int id)
        {

            try
            {

                double receitas = await _unitOfWork.UsuarioRepository.GetAllReceitasByIdUsuarioAsync(id);

                return Ok(receitas);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);

            }

        }

        [HttpGet("{id}/saldo")]
        public async Task<ActionResult<double>> GetSaldoByIdUsuarioAsync(int id) {
            try
            {

                double saldo = await _unitOfWork.UsuarioRepository.GetSaldoByIdUsuarioAsync(id);
                return Ok(saldo);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);

            }
        }

    }


}