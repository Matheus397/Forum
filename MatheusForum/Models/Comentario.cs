using ViewModels;
namespace Models
{
    public class Comentario : Heranca
    {
        public Autor AutorComentario { get; set; }
        public string PublicacaoId { get; set; }
        public string ComentarioId { get; set; }
        public string CitacaoId { get; set; }
        public string mensagem { get; set; }      
    }
}