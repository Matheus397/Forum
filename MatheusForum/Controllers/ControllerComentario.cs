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
        public IActionResult BuscarComentarioPorID(string id, [FromHeader] string id_user)
        {
            var idcoment = new ComentarioCore(_mapper).BuscarComentarioID(id_user, id);
            return idcoment.Status ? 
                Ok(idcoment) : 
                (IActionResult)BadRequest(idcoment);
        }

        // POST api/values
        [HttpPost]
        public IActionResult CriarUmComentario([FromBody] ComentarioView comentarioView, [FromHeader] string id_user)
        {
            var coment = new ComentarioCore(comentarioView, _mapper).CadastrarComentario(id_user);
            return coment.Status ? 
                Ok(coment) : 
                (IActionResult)BadRequest(coment);
        }

        // PUT api/values/5
        [HttpPut("{publicacaoID}")]
        public IActionResult AtualizarComentario([FromHeader] string id_user, [FromBody] ComentarioAtualizacaoView ComentarioView, string id_comentario)
        {
            var comentAtualiza = new ComentarioCore(_mapper).AtualizarComentario(id_user, ComentarioView, id_comentario);
            return comentAtualiza.Status ?
                Ok(comentAtualiza) : 
                (IActionResult)BadRequest(comentAtualiza);
        }

        // DELETE api/values/5
        [HttpDelete("{IdPublicacao}")]
        public IActionResult DeletarComentario(string id_comentario, [FromHeader] string id_user)
        {
            var ComentarioDeletado = new ComentarioCore(_mapper).DeletarComentario(id_comentario, id_user);
            return ComentarioDeletado.Status ? 
                Ok(ComentarioDeletado) :
                (IActionResult)BadRequest(ComentarioDeletado);
        }
    }
}