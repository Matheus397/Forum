using System;

namespace ViewModels
{
    public class LoginViewRetorno
    {
        public bool Status { get; set; } = true;
        public Guid tokenAcesso { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
    }
}