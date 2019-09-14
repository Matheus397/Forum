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
    public class UsuarioCore : AbstractValidator<Usuario>
    {
        
        protected IMapper _Mapper { get; set; }
        protected Usuario _user { get; set; }
        protected Arquivo _arquivo { get; set; }

        public UsuarioCore(IMapper mapper)
        { _Mapper = mapper; _arquivo = Files.Puxar(_arquivo) ?? new Arquivo(); }

        public UsuarioCore(UsuarioView user, IMapper mapper)
        {
            _Mapper = mapper;
            _user = _Mapper.Map<UsuarioView, Usuario>(user);
            _arquivo = Files.Puxar(_arquivo) ?? new Arquivo();
            RuleFor(w => w.Nome).NotNull().MinimumLength(3)
                .WithMessage($"Nome Curto demais em caracteres");
            RuleFor(w => w.Email).EmailAddress().NotNull()
                .WithMessage("O email do Usuario não pode ser nulo");
            RuleFor(w => w.Senha).Length(8, 12).NotNull()
                .WithMessage("O Usuario deve ter uma senha para ter acesso ao login e deve conter no mínimo 8 caracteres e no máximo 12.");
            RuleFor(t => t.Senha).Matches(@"[a-z-A-Z].\d|\d.[a-z-A-Z]")
                .WithMessage("A senha deve conter ao menos uma letra e um número");
            RuleFor(r => r.ConfirmacaoDaSenha).Equal(_user.Senha)
                .WithMessage($"A confirmação da senha do {_user.Nome} deve ser igual a senha inserida");
        }
 
        public Retorno BuscaTdsUsuarios()
        {
            var allUsers = _arquivo.lstUsuarios;
            return allUsers.Any() ? 
            new Retorno { Status = true, Resultado = allUsers } : 
            new Retorno { Status = false, Resultado = new List<string>
            { "Não há nenhum usuário até então" } };
        }

        public Retorno CadastrarUsuario()
        {
            if (!Validate(_user).IsValid)
                return new Retorno { Status = false, Resultado = Validate(_user).Errors.Select(m => m.ErrorMessage).ToList() };
            if (_arquivo.lstUsuarios.Any(u => u.Email == _user.Email))
                return new Retorno { Status = false, Resultado = new List<string>
                { "Email pertence a outro usuário" } };
            _arquivo.lstUsuarios.Add(_user);
            Files.Salvar(_arquivo);
            return new Retorno { Status = true, Resultado = new List<string> { "Usuário cadastrado" } };
        }

        public Retorno AutenticaUsuario(LoginViewRetorno LoginView)
        {
            var user = _arquivo.lstUsuarios.Find(U => U.Email == LoginView.Login);
            if (user == null) return new Retorno { Status = false, Resultado = new List<string> { "Favor informar e-mail válido" } };
            if (user.Senha != LoginView.Senha) return new Retorno { Status = false, Resultado = new List<string> { "Senha inválida" } };
            return new Retorno { Status = true, Resultado = _Mapper.Map<LoginViewRetorno>(user) };
        }

    }
}