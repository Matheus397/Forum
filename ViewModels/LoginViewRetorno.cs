using System;

namespace ViewModels
{
    public class LoginViewRetorno
    {
        public string Nome { get; set; }
        public bool Status { get; set; } = true;
        public Guid tokenAcesso { get; set; }        
    }
}