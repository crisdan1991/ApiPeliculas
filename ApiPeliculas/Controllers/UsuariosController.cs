using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiPeliculas.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usRepo;
        private readonly IMapper _mapper;
        protected RespuestaAPI _respuesta;

        public UsuariosController(IUsuarioRepositorio usRepo, IMapper mapper)
        {
            _usRepo = usRepo;
            _mapper = mapper;
            this._respuesta = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUsuarios()
        {
            var listaUsuarios = _usRepo.GetUsuario();
            var listaUsuariosDto = new List<UsuarioDto>();

            foreach (var lista in listaUsuarios)
            {
                listaUsuariosDto.Add(_mapper.Map<UsuarioDto>(lista));
            }

            return Ok(listaUsuariosDto);
        }

        [HttpGet("{usuarioId:int}", Name = "GetUsuario")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUsuario(int usuarioId)
        {
            var itemUsuario = _usRepo.GetUsuario(usuarioId);

            if (itemUsuario == null)
            {
                return NotFound();
            }

            var itemUsuarioDto = _mapper.Map<Usuario>(itemUsuario);
            return Ok(itemUsuarioDto);
        }


        [HttpPost("registro")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
            bool validarUsuarioUnico = _usRepo.IsUniqueUsuario(usuarioRegistroDto.NombreUsuario);
            if (!validarUsuarioUnico)
            {
                _respuesta.StatusCode = HttpStatusCode.BadRequest;
                _respuesta.IsSuccess = false;
                _respuesta.ErrorMessages.Add("El nombre de usuario ya existe");
                return BadRequest(_respuesta);
            }
            var usuario = await _usRepo.Registro(usuarioRegistroDto);
            if (usuario == null)
            {
                _respuesta.StatusCode = HttpStatusCode.BadRequest;
                _respuesta.IsSuccess = false;
                _respuesta.ErrorMessages.Add("Error al registrar el usuario");
                return StatusCode(500, _respuesta);
            }
            _respuesta.StatusCode = HttpStatusCode.OK;
            _respuesta.IsSuccess = true;
            return Ok(_respuesta);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(UsuarioLoginDto usuarioLoginDto)
        {

            var respuestaLogin = await _usRepo.Login(usuarioLoginDto);
            if (respuestaLogin.Usuario == null || string.IsNullOrEmpty(respuestaLogin.Token) )
            {
                _respuesta.StatusCode = HttpStatusCode.BadRequest;
                _respuesta.IsSuccess = false;
                _respuesta.ErrorMessages.Add("El nombre de usuario o la contraseña son incorrectos");
                return BadRequest(_respuesta);
            }
            
            _respuesta.StatusCode = HttpStatusCode.OK;
            _respuesta.IsSuccess = true;
            _respuesta.Result = respuestaLogin;
            return Ok(_respuesta);
        }


    }
}
