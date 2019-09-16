using ModelsProject;

namespace Models
{
    public class Publicacao : Heranca
    {
        public Usuario Autor { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public string Tipo { get; set; }
        public string Status { get; set; }
        public double? MediaDeVotos { get; set; }
    }
}