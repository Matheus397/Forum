
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
        private Comentario _Comentario { get; set; }
        private Compactar _compactar { get; set; }
        private IMapper _Mapper { get; set; }

        public ComentarioCore(IMapper mapper) { _Mapper = mapper; _compactar = FileCloud.Recuperar(_compactar) ?? new Compactar(); }

        public ComentarioCore(ComentarioView comentario, IMapper mapper)
        {
            _Mapper = mapper;
            _Comentario = _Mapper.Map<ComentarioView, Comentario>(comentario);
            _compactar = FileCloud.Recuperar(_compactar) ?? new Compactar();

            RuleFor(c => c.PublicacaoId).NotNull().Length(36)
                .WithMessage("O id da publicação é o identificador obrigatório para o comentário.");

            RuleFor(c => c.Msg).NotNull().Length(10, 500)
                .WithMessage("A Mensagem deve ter no mínimo de 10 caracteres e o máximo de 500.");
        }


        public Retorno CadastrarComentario(string UsuarioId)
        {
            if (!Validate(_Comentario).IsValid) return new Retorno { Status = false, Resultado = Validate(_Comentario).Errors.Select(e => e.ErrorMessage).ToList() };

            //vejo se os ID's recebidos são válidos.
            var listaComentarios = _compactar.lstComentarios;
            if (_Comentario.ComentarioId != null)
            {
                if (!Guid.TryParse(_Comentario.ComentarioId, out Guid comentarioID) || !listaComentarios.Any(C => C.ID == comentarioID))
                    return new Retorno { Status = false, Resultado = new List<string> { "O comentário ID inserio não existe na base de dados , informe um ID comentário válido." } };
            }
            if (_Comentario.CitacaoId != null)
            {
                if (!Guid.TryParse(_Comentario.CitacaoId, out Guid citacaoID) || !listaComentarios.Any(C => C.ID == citacaoID))
                    return new Retorno { Status = false, Resultado = new List<string> { "O citação ID inserio não existe na base de dados , informe um ID comentário válido." } };
            }

            if (!Guid.TryParse(UsuarioId, out Guid id))
                return new Retorno { Status = false, Resultado = new List<string> { "Por favor efetue o login do usuário para postar o comentário." } };

            if (!Guid.TryParse(_Comentario.PublicacaoId, out Guid idPublicacao))
                return new Retorno { Status = false, Resultado = new List<string> { "Insirá um ID de Publicação válido." } };

            //verifico usuario do comentário se existe.
            var UsuarioComentrio = _compactar.lstUsuarios.Find(u => u.ID == id);
            if (UsuarioComentrio == null) return new Retorno { Status = false, Resultado = new List<string> { "Esse ID de Usuário não existe na base de dados. Por favor insira um ID de Usuário cadastrado na base." } };

            _Comentario.AutorComentario = _Mapper.Map<Usuario, Autor>(UsuarioComentrio);
            //varifico se a publicação existe.
            var publicacaoComentario = _compactar.lstPublicacoes.Find(p => p.ID == idPublicacao);
            if (publicacaoComentario == null) return new Retorno { Status = false, Resultado = new List<string> { "Esse ID de Publicação não existe na base de dados. Por favor insira um ID de Publicação cadastrado na base." } };

            publicacaoComentario.lstComentarios.Add(_Comentario);
            FileCloud.Salvar(_compactar);
            return new Retorno { Status = true, Resultado = new List<string> { "Comentário cadastrado com sucesso!" } };
        }

        public Retorno DeletarComentario(string UsuarioId, string ComentarioID)
        {
            //válidações para verificar se os ID's são válidos.
            if (!Guid.TryParse(UsuarioId, out Guid id) || _compactar.lstUsuarios.Find(u => u.ID == id) == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Por favor efetue o login do usuário para buscar o comentário." } };

            if (!Guid.TryParse(ComentarioID, out Guid GuidComentario))
                return new Retorno { Status = false, Resultado = new List<string> { "por favor insira um ID válido para buscar o comentário." } };

            var comentario = _compactar.lstComentarios.Find(C => C.ID == GuidComentario);
            if (comentario == null) return new Retorno { Status = false, Resultado = new List<string> { "Esse comentário não existe na base de dados. Por favor insira um ID de comentário cadastrado na base." } };

            _compactar.lstComentarios.Remove(comentario);
            FileCloud.Salvar(_compactar);
            return new Retorno { Status = true, Resultado = "Comentário deletado com sucesso!" };
        }

        public Retorno AtualizarComentario(string UsuarioId, ComentarioAtualizacaoView comentarioView, string ComentarioID)
        {
            //válidações para verificar se os ID's são válidos.
            if (!Guid.TryParse(UsuarioId, out Guid id) || _compactar.lstUsuarios.Find(u => u.ID == id) == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Por favor efetue o login do usuário para buscar o comentário." } };

            if (!Guid.TryParse(ComentarioID, out Guid GuidComentario))
                return new Retorno { Status = false, Resultado = new List<string> { "por favor insira um ID válido para buscar o comentário." } };

            var comentario = _compactar.lstComentarios.Find(C => C.ID == GuidComentario);
            if (comentario == null) return new Retorno { Status = false, Resultado = new List<string> { "Esse comentário não existe na base de dados. Por favor insira um ID de comentário cadastrado na base." } };

            if (comentarioView.Msg.Length < 10 && comentarioView.Msg.Length > 500)
                return new Retorno { Status = false, Resultado = new List<string> { "A Mensagem deve ter no mínimo de 10 caracteres e o máximo de 500." } };

            _Mapper.Map(comentarioView, comentario);
            FileCloud.Salvar(_compactar);
            return new Retorno { Status = true, Resultado = comentario };

        }

        public Retorno BuscarComentarioID(string UsuarioId, string ComentarioID)
        {
            //válidações para verificar se os ID's são válidos.
            if (!Guid.TryParse(UsuarioId, out Guid id) || _compactar.lstUsuarios.Find(u => u.ID == id) == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Por favor efetue o login do usuário para buscar o comentário." } };

            if (!Guid.TryParse(ComentarioID, out Guid GuidComentario))
                return new Retorno { Status = false, Resultado = new List<string> { "por favor insira um ID válido para buscar o comentário." } };

            var comentario = _compactar.lstComentarios.Find(C => C.ID == GuidComentario);
            if (comentario == null) return new Retorno { Status = false, Resultado = new List<string> { "Esse comentário não existe na base de dados. Por favor insira um ID de comentário cadastrado na base." } };

            return new Retorno { Status = true, Resultado = comentario };
        }
    }
}