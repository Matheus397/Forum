using System.Collections.Generic;
using ViewModels;

namespace Models
{
    public class Publicacao : Heranca
    {
        
        public string Status { get; set; }
        public string MediaDeVotos { get; set; }
        public List<Comentario> lstComentarios { get; set; }
        public Autor Autor { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public string Tipo { get; set; }
    }
}