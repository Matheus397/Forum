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
        public  IActionResult buscarTodasPublicacoes([FromHeader]string id_user) 
        {
            var todasPublicacoes = new PublicacaoCore(_mapper).buscaTodasPublicacoes(id_user);
            return todasPublicacoes.Status ? 
                Ok(todasPublicacoes) : 
                (IActionResult)BadRequest(todasPublicacoes);
        }

     
        [HttpGet("{id}")]
        public IActionResult buscarPublicacaoPorID(string id,[FromHeader] string id_user)
        {
            var idPublicacao = new PublicacaoCore(_mapper).buscaIdPublicacao(id, id_user);
            return idPublicacao.Status ? 
                Ok(idPublicacao): 
                (IActionResult)BadRequest(idPublicacao);
        }

       
        [HttpPost]
        public IActionResult criarUmaPublicacao([FromBody] PublicacaoView publicacaoView ,[FromHeader] string id_user)
        {
            var publicacao = new PublicacaoCore(publicacaoView, _mapper).cadastrarPublicacao(id_user);
            return publicacao.Status ? 
                Ok(publicacao) : 
                (IActionResult)BadRequest(publicacao);
        }

       
        [HttpPut("{publicacaoID}")]
        public IActionResult atualizarPublicacao([FromHeader] string id_user, [FromBody] PublicacaoView  PublicacaoView ,string id_publicacao)
        {
            var publicacaoAtualiza = new PublicacaoCore(_mapper).atualizaPublicacao(PublicacaoView, id_publicacao, id_user);
            return publicacaoAtualiza.Status ? 
                Ok(publicacaoAtualiza) : 
                (IActionResult)BadRequest(publicacaoAtualiza);
        }

        
        [HttpDelete("{IdPublicacao}")]
        public IActionResult deletarPublicacao(string IdPublicacao,[FromHeader] string UsuarioId)
        {
            var PublicacaoDeletada = new PublicacaoCore(_mapper).deletePublicacao(IdPublicacao, UsuarioId);
            return PublicacaoDeletada.Status ? Ok(PublicacaoDeletada) : (IActionResult)BadRequest(PublicacaoDeletada);
        }
    }
}