using AutoMapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using Models;
using ViewModels;

namespace APIForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private IMapper _mapper;

        public UsuarioController(IMapper mapper) => _mapper = mapper;

        // Post api/Autenticar
        [HttpPost("Autenticar")]
        public IActionResult Login([FromBody] LoginViewRetorno loginView)
        {
            var login = new UsuarioCore(_mapper).AutenticareUsuario(loginView);
            return login.Status ? Ok(login) : (IActionResult)BadRequest(login);
        }


        // POST api/
        [HttpPost]
        public IActionResult CadastroUsuario([FromBody] UsuarioView usuarioView)
        {
            var usuario = new UsuarioCore(usuarioView, _mapper).CadastrarUsuario();
            return usuario.Status ? Ok(usuario) : (IActionResult)BadRequest(usuario);
        }

        //GET api/
        [HttpGet]
        public IActionResult BuscarTodosUsuarios()
        {
            var todosUsuarios = new UsuarioCore(_mapper).BuscarTodosUsuarios();
            return todosUsuarios.Status ? Ok(todosUsuarios) : (IActionResult)BadRequest(todosUsuarios);
        }

    }
}