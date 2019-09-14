using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
namespace Models
{
    /// <summary>
    /// Generic é minha classe que contém atributos Genéricos que todas minhas classes terão.
    /// </summary>
    public abstract class Generic
    {
        public Guid ID { get; set; } = Guid.NewGuid();

        [DisplayFormat(DataFormatString = "mm/dd/yyyy")]
        public DateTime Data { get; set; } = DateTime.Now;
    }
   }
   