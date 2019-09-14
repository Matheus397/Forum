using System.ComponentModel.DataAnnotations;
namespace Models
{
    public class Usuario : Heranca
    {  

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Informe a Senha")]
        [StringLength(12, MinimumLength = 8, ErrorMessage = "Senha deve ter entre 4 e 10 caracteres")]
        public string Senha { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirme a Senha")]
        [StringLength(12, MinimumLength = 8, ErrorMessage = "Senha deve ter entre 4 e 10 caracteres")]
        public string ConfirmacaoDaSenha { get; set; }

        [Required]
        [Display(Name = "Informe seu nome")]
        [MinLength(3, ErrorMessage = "Informe um nome com pelo menos 3 caracters")]
        public string Nome { get; set; }


        [Required(ErrorMessage = "Informe o seu email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um email válido...")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}