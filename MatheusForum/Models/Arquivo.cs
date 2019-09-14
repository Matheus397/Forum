using System.Collections.Generic;
namespace Models
{
    /// <summary>
    /// Compactar tem lista de todas minhas models, é essa classe que vou salvar no Json.
    /// </summary>
    public class Arquivo
    {
        public List<Usuario> lstUsuarios { get; set; } = new List<Usuario>();
        public List<Publicacao> lstPublicacoes { get; set; } = new List<Publicacao>();
        public List<Comentario> lstComentarios { get; set; } = new List<Comentario>();
    }
}