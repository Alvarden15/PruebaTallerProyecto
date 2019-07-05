using System;
using System.ComponentModel.DataAnnotations;

namespace PruebaTallerProyecto.Models
{
    public class Maceta{

        [Key]
        public int id { get; set; }

        public string nombre { get; set; }

        public string mensaje { get; set; }

        public double humedad { get; set; }

        [DisplayFormat(DataFormatString="{0:dd/MM/yyyy}",ApplyFormatInEditMode=true)]
        [DataType(DataType.Date)]
        public DateTime fecha { get; set; }

         [DisplayFormat(DataFormatString="{0:hh:mm}",ApplyFormatInEditMode=true)]
        public string hora {get; set;}

    }

}
