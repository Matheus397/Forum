using AutoMapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using Models;
using ModelsProject.DataBase;

namespace APIForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicacaoController : ControllerBase
    {
        private IMapper _mapper { get; set; }
        public Arquivo _arquivo { get; set; }

        public PublicacaoController(IMapper mapper, Arquivo arquivo)
        {
            _mapper = mapper;
            _arquivo = arquivo;
        }

        [HttpGet]
        public  IActionResult buscarTodasPublicacoes([FromHeader]string id_user) 
        {
            var todasPublicacoes = new PublicacaoCore(_mapper, _arquivo).buscaTodasPublicacoes(id_user);
            return todasPublicacoes.Status ? 
                Ok(todasPublicacoes) : 
                (IActionResult)BadRequest(todasPublicacoes);
        }
     
        [HttpGet("{id}")]
        public IActionResult buscarPublicacaoPorID(string id,[FromHeader] string id_user)
        {
            var idPublicacao = new PublicacaoCore(_mapper, _arquivo).buscaIdPublicacao(id, id_user);
            return idPublicacao.Status ? 
                Ok(idPublicacao): 
                (IActionResult)BadRequest(idPublicacao);
        }
       
        [HttpPost]
        public IActionResult criarUmaPublicacao([FromBody] Publicacao publicacaoView ,[FromHeader] string id_user)
        {
            var publicacao = new PublicacaoCore(publicacaoView, _mapper, _arquivo).cadastrarPublicacao(id_user);
            return publicacao.Status ? 
                Ok(publicacao) : 
                (IActionResult)BadRequest(publicacao);
        }
 
        [HttpPut("{publicacaoID}")]
        public IActionResult atualizarPublicacao([FromHeader] string id_user, [FromBody] Publicacao  PublicacaoView ,string id_publicacao)
        {
            var publicacaoAtualiza = new PublicacaoCore(PublicacaoView, _mapper, _arquivo).atualizaPublicacao(PublicacaoView, id_publicacao, id_user);
            return publicacaoAtualiza.Status ? 
                Ok(publicacaoAtualiza) : 
                (IActionResult)BadRequest(publicacaoAtualiza);
        }
        
        [HttpDelete("{IdPublicacao}")]
        public IActionResult deletarPublicacao(string IdPublicacao, [FromHeader] string UsuarioId)
        {
            var PublicacaoDeletada = new PublicacaoCore(_mapper, _arquivo).deletePublicacao(IdPublicacao, UsuarioId);
            return PublicacaoDeletada.Status ? Ok(PublicacaoDeletada) : (IActionResult)BadRequest(PublicacaoDeletada);
        }
    }
}