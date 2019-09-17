using AutoMapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using Models;
using ModelsProject;
using ModelsProject.DataBase;
using ViewModels;

namespace APIForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private IMapper _mapper { get; set; }
        public Arquivo _arquivo { get; set; }

        public UsuarioController(IMapper mapper, Arquivo arquivo)
        {
            _mapper = mapper;
            _arquivo = arquivo;
        }

        [HttpGet]
        public IActionResult BuscarTodosUsuarios()
        {
            var todosUsuarios = new UsuarioCore(_mapper).buscaTdsUsuarios();
            return todosUsuarios.Status ?
                Ok(todosUsuarios) :           
                (IActionResult)BadRequest(todosUsuarios);
        }

        [HttpPost("Autenticar")]
        public IActionResult Login([FromBody] LoginView loginView)
        {
            var login = new UsuarioCore(_mapper).autenticaUsuario(loginView);
            return login.Status ? 
                Ok(login) : 
                (IActionResult)BadRequest(login);
        }


        [HttpPost]
        public IActionResult CadastroUsuario([FromBody] Usuario usuarioView)
        {
            var usuario = new UsuarioCore(usuarioView, _mapper).cadastrarUsuario();
            return usuario.Status ? 
                Ok(usuario) : 
                (IActionResult)BadRequest(usuario);
        }
    }
}