using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ViewModels;

namespace ModelsProject
{
    public class Comentario : Heranca
    {
        [Required]
        public string Message { get; set; }

        [Range(0, 5)]
        public float? Nota { get; set; }

        public float? MediaDeVotos { get; set; }

        [NotMapped]
        public List<Comentario> Replicas { get; set; }

        public Guid AutorComentarioId { get; set; }
        public string PublicacaoId { get; set; }
        public string ComentarioId { get; set; }
        public string CitacaoId { get; set; }
        public string mensagem { get; set; }
    }  
}

