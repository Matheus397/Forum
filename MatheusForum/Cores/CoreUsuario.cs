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
        private Usuario _Usuario { get; set; }
        private Compactar _compactar { get; set; }
        private IMapper _Mapper { get; set; }

        public UsuarioCore(IMapper mapper) { _Mapper = mapper; _compactar = FileCloud.Recuperar(_compactar) ?? new Compactar(); }
        public UsuarioCore(UsuarioView user, IMapper mapper)
        {
            _Mapper = mapper;
            _Usuario = _Mapper.Map<UsuarioView, Usuario>(user);
            _compactar = FileCloud.Recuperar(_compactar) ?? new Compactar();


            RuleFor(e => e.Nome).NotNull().MinimumLength(3)
                .WithMessage($"Usuario deve ter um nome de pelo menos 3 caracteres");

            RuleFor(e => e.Email).EmailAddress().NotNull()
                .WithMessage("O email do Usuario não pode ser nulo");

            RuleFor(e => e.Senha).Length(8, 12).NotNull()
                .WithMessage("O Usuario deve ter uma senha para ter acesso ao login e deve conter no mínimo 8 caracteres e no máximo 12.");

            RuleFor(u => u.Senha).Matches(@"[a-z-A-Z].\d|\d.[a-z-A-Z]")
                .WithMessage("A senha deve conter ao menos uma letra e um número");

            RuleFor(e => e.ConfirmacaoDaSenha).Equal(_Usuario.Senha)
                .WithMessage($"A confirmação da senha do {_Usuario.Nome} deve ser igual a senha inserida");
        }



        public Retorno CadastrarUsuario()
        {
            if (!Validate(_Usuario).IsValid) return new Retorno { Status = false, Resultado = Validate(_Usuario).Errors.Select(m => m.ErrorMessage).ToList() };

            if (_compactar.lstUsuarios.Any(u => u.Email == _Usuario.Email)) return new Retorno { Status = false, Resultado = new List<string> { "Já Existe um Usuário com esse Email" } };

            _compactar.lstUsuarios.Add(_Usuario);
            FileCloud.Salvar(_compactar);

            return new Retorno { Status = true, Resultado = new List<string> { "Usuário cadastrado com sucesso" } };

        }

        public Retorno AutenticareUsuario(LoginViewRetorno LoginView)
        {
            var Usuario = _compactar.lstUsuarios.Find(U => U.Email == LoginView.Login);
            if (Usuario == null) return new Retorno { Status = false, Resultado = new List<string> { "Por favor informe um Email valido." } };

            if (Usuario.Senha != LoginView.Senha) return new Retorno { Status = false, Resultado = new List<string> { "Senha inválida , não foi possível realizar o login." } };

            return new Retorno { Status = true, Resultado = _Mapper.Map<LoginViewRetorno>(Usuario) };
        }

        public Retorno BuscarTodosUsuarios()
        {
            var todos = _compactar.lstUsuarios;
            return todos.Any() ? new Retorno { Status = true, Resultado = todos } : new Retorno { Status = false, Resultado = new List<string> { "Não tem nenhum Usuário cadastrado na Base de Dados." } };
        }

    }
}