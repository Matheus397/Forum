
using AutoMapper;
using Core.Util;
using FluentValidation;
using Models;
using ModelsProject.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class PublicacaoCore : AbstractValidator<Publicacao>
    {
        protected IMapper _Mapper { get; set; }
        protected Publicacao _publicacao { get; set; }
        protected Arquivo _arquivo { get; set; }
        

        public PublicacaoCore(Publicacao publicacao, IMapper mapper, Arquivo arquivo)
        {
            _Mapper = mapper;
            _publicacao = _Mapper.Map<Publicacao, Publicacao>(publicacao);
            
            RuleFor(q => q.Titulo).NotNull().Length(8, 250)
                .WithMessage("O título deve ter entre 8 e 250 caracters");
            RuleFor(e => e.Texto).NotNull().MinimumLength(50)
                .WithMessage("Favor inserir um texto com pelo menos 50 caracters");
        }

        public PublicacaoCore(IMapper mapper, Arquivo arquivo)
        {
            _Mapper = mapper;
            _arquivo = arquivo;
        }

        public Retorno atualizaPublicacao(Publicacao publicacao, string id_publicacao, string id_user)
        {
            if (!Guid.TryParse(id_publicacao, out Guid idconvertPublicacao))
                return new Retorno
                {
                    Status = false,
                    Resultado = new List<string>
                    { "Insira um ID válido." }
                };

            if (!Guid.TryParse(id_user, out Guid idconvertUsuario))
                new Retorno
                {
                    Status = false,
                    Resultado = new List<string>
                    { "Token inválido ou não encontrado" }
                };
            var PublicacaoSelecionada =
                _arquivo.Publicacoes.FirstOrDefault(p => p.ID == idconvertPublicacao);

            if (PublicacaoSelecionada == null) return new Retorno
            {
                Status = false,
                Resultado = new List<string>
                { "Publicação não encontrada por este Id" }
            };

            var autorPublicacao = _arquivo.Usuarios.FirstOrDefault(u => u.ID == idconvertUsuario);
            if (autorPublicacao == null) return new Retorno
            {
                Status = false,
                Resultado = new List<string>
                { "Não existe nenhum Usuário com ID inserido" }
            };

            if (PublicacaoSelecionada.Autor.Email != autorPublicacao.Email)
                return new Retorno
                {
                    Status = false,
                    Resultado = new List<string>
                    { "Apenas Autores podem mudar suas publicações" }
                };

            if (publicacao.Titulo != null)
            {
                if (publicacao.Titulo.Length < 8 && publicacao.Titulo.Length > 250)
                    return new Retorno
                    {
                        Status = false,
                        Resultado = new List<string>
                        { "O título dever conter 8 a 250 caracteres." }
                    };
            }

            if (publicacao.Texto != null)
            {
                if (publicacao.Texto.Length < 50)
                    return new Retorno
                    {
                        Status = false,
                        Resultado = new List<string>
                        { "O texto deve ter ao menos 50 carteres" }
                    };
            }

            if (PublicacaoSelecionada.Tipo.ToLower() == "tutorial")
                publicacao.Status = null;

            _Mapper.Map(publicacao, PublicacaoSelecionada);
           
            return new Retorno { Status = true, Resultado = PublicacaoSelecionada };
        }

        public Retorno deletePublicacao(string id_publicacao, string id_user)
        {

            if (!Guid.TryParse(id_publicacao, out Guid idconvertPublicacao))
                return new Retorno
                {
                    Status = false,
                    Resultado = new List<string>
                    { "Id não encontrado" }
                };

            if (!Guid.TryParse(id_user, out Guid idconvertUsuario))
                return new Retorno
                {
                    Status = false,
                    Resultado = new List<string>
                    { "Token inválido ou não encontrado" }
                };


            var Publicacao = _arquivo.Publicacoes.FirstOrDefault(p => p.ID == idconvertPublicacao);
            if (Publicacao == null) return new Retorno
            {
                Status = false,
                Resultado = new List<string>
            { "Publicação não encontrada" }
            };

            var autorPublicacao = _arquivo.Usuarios.FirstOrDefault(u => u.ID == idconvertUsuario);

            if (autorPublicacao == null) return new Retorno
            {
                Status = false,
                Resultado = new List<string>
                { "Id de usuário não encontrado" }
            };

            if (Publicacao.Autor.Email != autorPublicacao.Email)
                return new Retorno
                {
                    Status = false,
                    Resultado = new List<string>
                    { "Somente autores podem atualizar suas publicações" }
                };

            _arquivo.Publicacoes.Remove(Publicacao);
            
            return new Retorno
            {
                Status = true,
                Resultado = new List<string>
                { "Publicação Deletada "}
            };
        }

        public Retorno buscaIdPublicacao(string id_publicacao, string id_user)
        {
            if (!Guid.TryParse(id_publicacao, out Guid idconvertPublicacao))
                return new Retorno
                {
                    Status = false,
                    Resultado = new List<string>
                { "Id não encontrado" }
                };

            if (!Guid.TryParse(id_user, out Guid idconvertUsuario))
                return new Retorno
                {
                    Status = false,
                    Resultado = new List<string>
                    { "Token inválido ou não encontrado" }
                };

            var autorPublicacao = _arquivo.Usuarios.FirstOrDefault(u => u.ID == idconvertUsuario);
            if (autorPublicacao == null) return new Retorno
            {
                Status = false,
                Resultado = new List<string>
                { "Usuário não encontrado" }
            };

            var Publicacao = _arquivo.Publicacoes.FirstOrDefault(p => p.ID == idconvertPublicacao);
            return Publicacao == null ? new Retorno
            {
                Status = false,
                Resultado = new List<string>
                { "Publicação não encontrada" }
            } :
            new Retorno { Status = true, Resultado = Publicacao };
        }

        public Retorno buscaTodasPublicacoes(string id_user)
        {
            if (!Guid.TryParse(id_user, out Guid idconvertUsuario))
                return new Retorno
                {
                    Status = false,
                    Resultado = new List<string>
                    { "Token inválido ou não encontrado" }
                };

            var autorPublicacao = _arquivo.Usuarios.FirstOrDefault(u => u.ID == idconvertUsuario);
            if (autorPublicacao == null) return new Retorno
            {
                Status = false,
                Resultado = new List<string>
                { "Id de usuário não encontrado" }
            };
            var todas = _arquivo.Publicacoes;
            return todas.Any() ? new Retorno
            { Status = true, Resultado = todas } :
            new Retorno
            {
                Status = false,
                Resultado = new List<string>
                { "Não há publicações na base ainda" }
            };
        }
        public Retorno cadastrarPublicacao(string id_user)
        {
            if (_publicacao.Tipo != null && _publicacao.Tipo.ToLower().Contains("tutorial") || _publicacao.Tipo.ToLower().Contains("duvida"))
            {

                if (!Validate(_publicacao).IsValid)
                    return new Retorno
                    { Status = false, Resultado = Validate(_publicacao).Errors.Select(e => e.ErrorMessage).ToList() };

                if (!Guid.TryParse(id_user, out Guid id))
                    return new Retorno
                    {
                        Status = false,
                        Resultado = new List<string>
                        { "Id não encontrado" }
                    };

                _publicacao.Autor = _arquivo.Usuarios.FirstOrDefault(u => u.ID == id);

                if (_publicacao.Autor == null) return new Retorno
                {
                    Status = false,
                    Resultado = new List<string>
                    { "É necessário logar primeiro" }
                };

                if (_publicacao.Tipo.ToLower().Contains("tutorial")) _publicacao.Status = null;

                else if (_publicacao.Tipo.ToLower().Contains("duvida"))
                    _publicacao.Status = "aberta";

                _arquivo.Publicacoes.Add(_publicacao);
                
                return new Retorno { Status = true, Resultado = new List<string> { $"{_publicacao.Autor.Nome} sua publicação foi publicada com Sucesso." } };

            }
            return new Retorno
            {
                Status = false,
                Resultado = new List<string>
                { "A tipagem da publicação não pode ser nula" }
            };
        }
    }
}