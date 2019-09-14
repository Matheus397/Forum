using AutoMapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using ViewModels;

namespace APIForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        private IMapper _mapper { get; set; }
        public ComentarioController(IMapper mapper) => _mapper = mapper;


        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult BuscarComentarioPorID(string id, [FromHeader] string UsuarioId)
        {
            var ComentarioID = new ComentarioCore(_mapper).BuscarComentarioID(UsuarioId, id);
            return ComentarioID.Status ? Ok(ComentarioID) : (IActionResult)BadRequest(ComentarioID);
        }

        // POST api/values
        [HttpPost]
        public IActionResult CriarUmComentario([FromBody] ComentarioView comentarioView, [FromHeader] string UsuarioId)
        {
            var Comentario = new ComentarioCore(comentarioView, _mapper).CadastrarComentario(UsuarioId);
            return Comentario.Status ? Ok(Comentario) : (IActionResult)BadRequest(Comentario);
        }

        // PUT api/values/5
        [HttpPut("{publicacaoID}")]
        public IActionResult AtualizarComentario([FromHeader] string UsuarioId, [FromBody] ComentarioAtualizacaoView ComentarioView, string ComentarioID)
        {
            var AtualizarComentario = new ComentarioCore(_mapper).AtualizarComentario(UsuarioId, ComentarioView, ComentarioID);
            return AtualizarComentario.Status ? Ok(AtualizarComentario) : (IActionResult)BadRequest(AtualizarComentario);
        }

        // DELETE api/values/5
        [HttpDelete("{IdPublicacao}")]
        public IActionResult DeletarComentario(string ComentarioID, [FromHeader] string UsuarioId)
        {
            var ComentarioDeletado = new ComentarioCore(_mapper).DeletarComentario(ComentarioID, UsuarioId);
            return ComentarioDeletado.Status ? Ok(ComentarioDeletado) : (IActionResult)BadRequest(ComentarioDeletado);
        }
    }
}