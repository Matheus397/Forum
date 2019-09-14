using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using Models;
using ViewModels;

namespace APIForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicacaoController : ControllerBase
    {
        private IMapper _mapper { get; set; }
        public PublicacaoController(IMapper mapper) => _mapper = mapper;
   
        [HttpGet]
        public  IActionResult BuscarTodasPublicacoes([FromHeader]string id_user) 
        {
            var todasPublicacoes = new PublicacaoCore(_mapper).BuscarTodosPublicacoes(id_user);
            return todasPublicacoes.Status ? 
                Ok(todasPublicacoes) : 
                (IActionResult)BadRequest(todasPublicacoes);
        }

     
        [HttpGet("{id}")]
        public IActionResult BuscarPublicacaoPorID(string id,[FromHeader] string id_user)
        {
            var PublicacaoID = new PublicacaoCore(_mapper).BuscarPublicacaoID(id, id_user);
            return PublicacaoID.Status ? 
                Ok(PublicacaoID): 
                (IActionResult)BadRequest(PublicacaoID);
        }

       
        [HttpPost]
        public IActionResult CriarUmaPublicacao([FromBody] PublicacaoView publicacaoView ,[FromHeader] string id_user)
        {
            var Publicacao = new PublicacaoCore(publicacaoView, _mapper).CadastrarPublicacao(id_user);
            return Publicacao.Status ? 
                Ok(Publicacao) : 
                (IActionResult)BadRequest(Publicacao);
        }

       
        [HttpPut("{publicacaoID}")]
        public IActionResult AtualizarPublicacao([FromHeader] string id_user, [FromBody] PublicacaoView  PublicacaoView ,string id_publicacao)
        {
            var AtualizarPublicacao = new PublicacaoCore(_mapper).AtualizarPublicacao(PublicacaoView, id_publicacao, id_user);
            return AtualizarPublicacao.Status ? 
                Ok(AtualizarPublicacao) : 
                (IActionResult)BadRequest(AtualizarPublicacao);
        }

        
        [HttpDelete("{IdPublicacao}")]
        public IActionResult DeletarPublicacao(string IdPublicacao,[FromHeader] string UsuarioId)
        {
            var PublicacaoDeletada = new PublicacaoCore(_mapper).deletePublicacao(IdPublicacao, UsuarioId);
            return PublicacaoDeletada.Status ? Ok(PublicacaoDeletada) : (IActionResult)BadRequest(PublicacaoDeletada);
        }
    }
}