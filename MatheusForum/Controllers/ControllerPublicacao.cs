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
        // GET api/values
        [HttpGet]
        public  IActionResult BuscarTodasPublicacoes([FromHeader]string UsuarioId) 
        {
            var todasPublicacoes = new PublicacaoCore(_mapper).BuscarTodosPublicacoes(UsuarioId);
            return todasPublicacoes.Status ? Ok(todasPublicacoes) : (IActionResult)BadRequest(todasPublicacoes);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult BuscarPublicacaoPorID(string id,[FromHeader] string UsuarioId)
        {
            var PublicacaoID = new PublicacaoCore(_mapper).BuscarPublicacaoID(id, UsuarioId);
            return PublicacaoID.Status ? Ok(PublicacaoID): (IActionResult)BadRequest(PublicacaoID);
        }

        // POST api/values
        [HttpPost]
        public IActionResult CriarUmaPublicacao([FromBody] PublicacaoView publicacaoView ,[FromHeader] string UsuarioId)
        {
            var Publicacao = new PublicacaoCore(publicacaoView, _mapper).CadastrarPublicacao(UsuarioId);
            return Publicacao.Status ? Ok(Publicacao) : (IActionResult)BadRequest(Publicacao);
        }

        // PUT api/values/5
        [HttpPut("{publicacaoID}")]
        public IActionResult AtualizarPublicacao([FromHeader] string UsuarioId, [FromBody] PublicacaoView  PublicacaoView ,string publicacaoID)
        {
            var AtualizarPublicacao = new PublicacaoCore(_mapper).AtualizarPublicacao(PublicacaoView, publicacaoID, UsuarioId);
            return AtualizarPublicacao.Status ? Ok(AtualizarPublicacao) : (IActionResult)BadRequest(AtualizarPublicacao);
        }

        // DELETE api/values/5
        [HttpDelete("{IdPublicacao}")]
        public IActionResult DeletarPublicacao(string IdPublicacao,[FromHeader] string UsuarioId)
        {
            var PublicacaoDeletada = new PublicacaoCore(_mapper).DeletarPublicacao(IdPublicacao, UsuarioId);
            return PublicacaoDeletada.Status ? Ok(PublicacaoDeletada) : (IActionResult)BadRequest(PublicacaoDeletada);
        }
    }
}