using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
namespace Models
{
    public abstract class Heranca
    {
        [DisplayFormat(DataFormatString = "mm/dd/yyyy")]
        public DateTime Data { get; set; } = DateTime.Now;
        public Guid ID { get; set; } = Guid.NewGuid();
    }
}
   