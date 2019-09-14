using ViewModels;
namespace Models
{
    public class Comentario : Generic
    {
        public string PublicacaoId { get; set; }
        public string ComentarioId { get; set; }
        public string CitacaoId { get; set; }
        public string Msg { get; set; }
        public Autor AutorComentario { get; set; }
    }
}