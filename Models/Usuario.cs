using System.ComponentModel.DataAnnotations;
namespace Models
{
    /// <summary>
    /// Usuario contém todos os dados que necessito para identificação e manipulação dos usuarios.
    /// </summary>
    public class Usuario : Generic
    {

        [Required]
        [Display(Name = "Informe seu nome")]
        [MinLength(3, ErrorMessage = "Informe um nome de no Minimo 3 caracters.")]
        public string Nome { get; set; }


        [Required(ErrorMessage = "Informe o seu email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um email válido...")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Informe a Senha")]
        [StringLength(12, MinimumLength = 8, ErrorMessage = "Insirá uma senha de no minimo 4 caracteres e no máximo de 10 caracteres.")]
        public string Senha { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Informe a Confirmação da Senha")]
        [StringLength(12, MinimumLength = 8, ErrorMessage = "Insirá uma senha de Confirmação de no minimo 4 caracteres e no máximo de 10 caracteres.")]
        public string ConfirmacaoDaSenha { get; set; }
    }
}