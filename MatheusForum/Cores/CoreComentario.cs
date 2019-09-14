
using AutoMapper;
using Core.Util;
using FluentValidation;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using ViewModels;

namespace Core
{
    public class ComentarioCore : AbstractValidator<Comentario>
    {
        private Comentario _comentario { get; set; }
        private Arquivo _arquivo { get; set; }
        private IMapper _Mapper { get; set; }

        public ComentarioCore(IMapper mapper) { _Mapper = mapper; _arquivo = Files.Puxar(_arquivo) ?? new Arquivo(); }

        public ComentarioCore(ComentarioView comentario, IMapper mapper)
        {
            _Mapper = mapper;
            _comentario = _Mapper.Map<ComentarioView, Comentario>(comentario);
            _arquivo = Files.Puxar(_arquivo) ?? new Arquivo();
            RuleFor(c => c.PublicacaoId).NotNull().Length(36)
                .WithMessage("A publicação tem que conter seu Id");
            RuleFor(c => c.mensagem).NotNull().Length(10, 500)
                .WithMessage("O texto deve ter entre 10 e 500 caracters");
        }

        public Retorno BuscarComentarioID(string id_user, string id_comentario)
        {
            
            if (!Guid.TryParse(id_user, out Guid id) || _arquivo.lstUsuarios.Find(u => u.ID == id) == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Necessario login de usuário" } };
            if (!Guid.TryParse(id_comentario, out Guid cometarioGuid))
                return new Retorno { Status = false, Resultado = new List<string> { "Id de comentário inálido ou não encontrado" } };
            var comentario = _arquivo.lstComentarios.Find(C => C.ID == cometarioGuid);
            if (comentario == null) return new Retorno { Status = false, Resultado = new List<string> { "Comentário não encontrado na base" } };
            return new Retorno { Status = true, Resultado = comentario };
        }


        public Retorno DeletarComentario(string id_user, string id_comentario)
        {
            
            if (!Guid.TryParse(id_user, out Guid id) || _arquivo.lstUsuarios.Find(u => u.ID == id) == null)
                return new Retorno { Status = false, Resultado = new List<string>
                { "Necessário efetuar Login" } };
            if (!Guid.TryParse(id_comentario, out Guid comentarioGuid))
                return new Retorno { Status = false, Resultado = new List<string>
                { "por favor insira um ID válido para buscar o comentário." } };

            var comentario = _arquivo.lstComentarios.Find(c => c.ID == comentarioGuid);

            if (comentario == null) return new Retorno
            { Status = false, Resultado = new List<string>
            { "Comentário não encontrado na base" } };
            _arquivo.lstComentarios.Remove(comentario);

            Files.Salvar(_arquivo);
            return new Retorno { Status = true, Resultado = "Comentário deletado " };
        }

        public Retorno AtualizarComentario(string id_user, ComentarioAtualizacaoView comentario_view, string id_comentario)
        {
           
            if (!Guid.TryParse(id_user, out Guid id) || _arquivo.lstUsuarios.Find(u => u.ID == id) == null)
                return new Retorno
                { Status = false, Resultado = new List<string>
                { "è necessário fazer o login" } };

            if (!Guid.TryParse(id_comentario, out Guid comentarioGuid))
                return new Retorno
                { Status = false, Resultado = new List<string>
                { "Id de comentário inválido" } };

            var comentario = _arquivo.lstComentarios.Find(c => c.ID == comentarioGuid);
            if (comentario == null) return new Retorno
            { Status = false, Resultado = new List<string>
            { "Comentário não encontrado" } };

            if (comentario_view.Msg.Length < 10 || comentario_view.Msg.Length > 500)
                return new Retorno { Status = false, Resultado = new List<string>
                { "A mensagem não pode ultrapassar 500 caracters ou ser menor que 10 caracters" } };
            _Mapper.Map(comentario_view, comentario);
            Files.Salvar(_arquivo);
            return new Retorno { Status = true, Resultado = comentario };
        }     

        public Retorno CadastrarComentario(string UsuarioId)
        {
            if (!Validate(_comentario).IsValid) return new Retorno 
            { Status = false, Resultado = Validate(_comentario).Errors.Select(e => e.ErrorMessage).ToList()};

            var listaComentarios = _arquivo.lstComentarios;
            if (_comentario.ComentarioId != null)
            {
                if (!Guid.TryParse(_comentario.ComentarioId, out Guid comentarioID) || !listaComentarios.Any(C => C.ID == comentarioID))
                    return new Retorno
                    { Status = false, Resultado = new List<string>
                    { "Comentário não encontrado" } };
            }

            if (_comentario.CitacaoId != null)
            {
                if (!Guid.TryParse(_comentario.CitacaoId, out Guid citacaoID) || !listaComentarios.Any(C => C.ID == citacaoID))
                    return new Retorno
                    { Status = false, Resultado = new List<string>
                    { "Citação não encontrada" } };
            }

            if (!Guid.TryParse(UsuarioId, out Guid id))
                return new Retorno
                { Status = false, Resultado = new List<string>
                { "É necessário fazer login" } };

            if (!Guid.TryParse(_comentario.PublicacaoId, out Guid idPublicacao))
                return new Retorno
                { Status = false, Resultado = new List<string>
                { "Id de publicação inválido" } };

            
            var UsuarioComentrio = _arquivo.lstUsuarios.Find(u => u.ID == id);
            if (UsuarioComentrio == null)
                return new Retorno
                { Status = false, Resultado = new List<string>
                { "Id de usuário não encontrado." } };

            _comentario.AutorComentario = _Mapper.Map<Usuario, Autor>(UsuarioComentrio);
           
            var publicacaoComentario = _arquivo.lstPublicacoes.Find(p => p.ID == idPublicacao);
            if (publicacaoComentario == null)
                return new Retorno { Status = false, Resultado = new List<string>
                { "Publicação não encontrada na base" } };

            publicacaoComentario.lstComentarios.Add(_comentario);
            Files.Salvar(_arquivo);
            return new Retorno
            { Status = true, Resultado = new List<string>
            { "Comentário cadastrado com sucesso!" } };
        }
    }
}