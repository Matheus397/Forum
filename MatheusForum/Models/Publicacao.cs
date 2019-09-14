using System.Collections.Generic;
using ViewModels;

namespace Models
{
    /// <summary>
    /// Publicação é minha Classe Generica que será Herdada em Tutorial e em Duvida.
    /// </summary>
    public class Publicacao : Generic
    {
        public Autor Autor { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public string Tipo { get; set; }
        public string Status { get; set; }
        public string MediaDeVotos { get; set; }
        public List<Comentario> lstComentarios { get; set; }

    }
}