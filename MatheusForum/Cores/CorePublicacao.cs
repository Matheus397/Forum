
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
    public class PublicacaoCore : AbstractValidator<Publicacao>
    {

        private Publicacao _Publicacao { get; set; }
        private Compactar _compactar { get; set; }
        private IMapper _Mapper { get; set; }
        public PublicacaoCore(IMapper mapper) { _Mapper = mapper; _compactar = FileCloud.Recuperar(_compactar) ?? new Compactar(); }
        public PublicacaoCore(PublicacaoView publicacao, IMapper mapper)
        {
            _Mapper = mapper;
            _Publicacao = _Mapper.Map<PublicacaoView, Publicacao>(publicacao);
            _compactar = FileCloud.Recuperar(_compactar) ?? new Compactar();

            RuleFor(p => p.Titulo).NotNull().Length(8, 250)
                .WithMessage("O título não pode ser nulo e dever ter de 8 a 250 caracteres.");

            RuleFor(p => p.Texto).NotNull().MinimumLength(50)
                .WithMessage("o texto não pode ser nulo e deve ter pelo menos 50 caracteres para a descrição da publicação.");

        }


        public Retorno CadastrarPublicacao(string UsuarioId)
        {
            if (_Publicacao.Tipo != null && _Publicacao.Tipo.ToLower().Contains("tutorial") || _Publicacao.Tipo.ToLower().Contains("duvida") || _Publicacao.Tipo.ToLower().Contains("dúvida"))
            {
                if (!Validate(_Publicacao).IsValid) return new Retorno { Status = false, Resultado = Validate(_Publicacao).Errors.Select(e => e.ErrorMessage).ToList() };

                if (!Guid.TryParse(UsuarioId, out Guid id))
                    return new Retorno { Status = false, Resultado = new List<string> { "Não foi possível identificar esse ID." } };

                _Publicacao.Autor = _Mapper.Map<Usuario, Autor>(_compactar.lstUsuarios.Find(u => u.ID == id));

                if (_Publicacao.Autor == null) return new Retorno { Status = false, Resultado = new List<string> { "Usúario não está logado, por favor efetue o login." } };

                if (_Publicacao.Tipo.ToLower().Contains("tutorial")) _Publicacao.Status = null;

                else if (_Publicacao.Tipo.ToLower().Contains("duvida") || _Publicacao.Tipo.ToLower().Contains("dúvida"))
                    _Publicacao.Status = "aberta";

                _compactar.lstPublicacoes.Add(_Publicacao);
                FileCloud.Salvar(_compactar);
                return new Retorno { Status = true, Resultado = new List<string> { $"{_Publicacao.Autor.Nome} sua publicação foi publicada com Sucesso." } };

            }

            return new Retorno { Status = false, Resultado = new List<string> { "Tipo não pode ser nulo e só pode ser 2 tipos de publicação 'tutorial' ou 'duvida' , informe um valor valido." } };
        }

        public Retorno AtualizarPublicacao(PublicacaoView publicacao, string IdPublicacao, string IdUsuario)
        {
            if (!Guid.TryParse(IdPublicacao, out Guid idconvertPublicacao))
                return new Retorno { Status = false, Resultado = new List<string> { "Não foi possível localizar ID , Insira um ID válido." } };

            if (!Guid.TryParse(IdUsuario, out Guid idconvertUsuario))
                new Retorno { Status = false, Resultado = new List<string> { "Não foi possível achar o token de Acesso do Usuário inserido , Insira um token válido." } };

            var PublicacaoSelecionada = _compactar.lstPublicacoes.Find(p => p.ID == idconvertPublicacao);
            if (PublicacaoSelecionada == null) return new Retorno { Status = false, Resultado = new List<string> { "Não existe nenhuma Publicação com ID inserido" } };

            var autorPublicacao = _compactar.lstUsuarios.Find(u => u.ID == idconvertUsuario);
            if (autorPublicacao == null) return new Retorno { Status = false, Resultado = new List<string> { "Não existe nenhum Usuário com ID inserido" } };

            if (PublicacaoSelecionada.Autor.Email != autorPublicacao.Email)
                return new Retorno { Status = false, Resultado = new List<string> { "O Usuário inserido não tem permissão para Atualizar essa publicação , somente o autor tem permissão." } };

            if (publicacao.Titulo != null)
            {
                if (publicacao.Titulo.Length < 8 && publicacao.Titulo.Length > 250)
                    return new Retorno { Status = false, Resultado = new List<string> { "O título dever ter de 8 a 250 caracteres." } };
            }
            if (publicacao.Texto != null)
            {
                if (publicacao.Texto.Length < 50)
                    return new Retorno { Status = false, Resultado = new List<string> { "o texto deve ter pelo menos 50 caracteres para a descrição da publicação." } };
            }
            if (PublicacaoSelecionada.Tipo.ToLower() == "tutorial")
                publicacao.Status = null;

            _Mapper.Map(publicacao, PublicacaoSelecionada);
            FileCloud.Salvar(_compactar);
            return new Retorno { Status = true, Resultado = PublicacaoSelecionada };
        }

        public Retorno DeletarPublicacao(string IdPublicacao, string IdUsuario)
        {
            //validações para ver se os ID's inseridos ão válidos.
            if (!Guid.TryParse(IdPublicacao, out Guid idconvertPublicacao))
                return new Retorno { Status = false, Resultado = new List<string> { "Não foi possível localizar ID , Insira um ID válido." } };

            if (!Guid.TryParse(IdUsuario, out Guid idconvertUsuario))
                return new Retorno { Status = false, Resultado = new List<string> { "Não foi possível achar o token de Acesso do Usuário inserido , Insira um token válido." } };

            //validações para ver se o autor e a publi~ção realmente existe
            var Publicacao = _compactar.lstPublicacoes.Find(p => p.ID == idconvertPublicacao);
            if (Publicacao == null) return new Retorno { Status = false, Resultado = new List<string> { "Não existe nenhuma Publicação com ID inserido" } };

            var autorPublicacao = _compactar.lstUsuarios.Find(u => u.ID == idconvertUsuario);
            if (autorPublicacao == null) return new Retorno { Status = false, Resultado = new List<string> { "Não existe nenhum Usuário com ID inserido" } };

            //validação para ver se quem quer deletar a publicação é realmente o autor.
            if (Publicacao.Autor.Email != autorPublicacao.Email)
                return new Retorno { Status = false, Resultado = new List<string> { "O Usuário inserido não tem permissão para apagar essa publicação , somente o autor tem permissão." } };

            //se for o autor eu faço todo processo de deletação.
            _compactar.lstPublicacoes.Remove(Publicacao);
            FileCloud.Salvar(_compactar);

            return new Retorno { Status = true, Resultado = new List<string> { $"{autorPublicacao.Nome} sua Publicação Deletada com sucesso." } };

        }

        public Retorno BuscarPublicacaoID(string IdPublicacao, string IdUsuario)
        {
            if (!Guid.TryParse(IdPublicacao, out Guid idconvertPublicacao))
                return new Retorno { Status = false, Resultado = new List<string> { "Não foi possível localizar ID , Insira um ID válido." } };


            if (!Guid.TryParse(IdUsuario, out Guid idconvertUsuario))
                return new Retorno { Status = false, Resultado = new List<string> { "Não foi possível achar o token de Acesso do Usuário inserido , Insira um token válido." } };

            var autorPublicacao = _compactar.lstUsuarios.Find(u => u.ID == idconvertUsuario);
            if (autorPublicacao == null) return new Retorno { Status = false, Resultado = new List<string> { "Não existe nenhum Usuário com ID inserido" } };


            var Publicacao = _compactar.lstPublicacoes.Find(p => p.ID == idconvertPublicacao);
            return Publicacao == null ? new Retorno { Status = false, Resultado = new List<string> { "Não existe nenhuma Publicação com ID inserido" } } : new Retorno { Status = true, Resultado = Publicacao };
        }

        public Retorno BuscarTodosPublicacoes(string IdUsuario)
        {
            if (!Guid.TryParse(IdUsuario, out Guid idconvertUsuario))
                return new Retorno { Status = false, Resultado = new List<string> { "Não foi possível achar o token de Acesso do Usuário inserido , Insira um token válido." } };

            var autorPublicacao = _compactar.lstUsuarios.Find(u => u.ID == idconvertUsuario);
            if (autorPublicacao == null) return new Retorno { Status = false, Resultado = new List<string> { "Não existe nenhum Usuário com ID inserido" } };

            var todas = _compactar.lstPublicacoes;
            return todas.Any() ? new Retorno { Status = true, Resultado = todas } : new Retorno { Status = false, Resultado = new List<string> { "Não tem nenhuma Publicação na Base de Dados." } };
        }
    }
}