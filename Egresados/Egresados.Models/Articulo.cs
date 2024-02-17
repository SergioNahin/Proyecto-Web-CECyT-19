using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egresados.Models
{
    public class Articulo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El Nombre es Obligatorio")]
        [Display(Name = "Nombre del Articulo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La Descripcion es Obligatoria")]
        public string Descripcion { get; set; }
        
        [Display(Name = "Fecha de Creación")]
        public string FechaCreacion { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]
        public string UrlImagen { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        [ForeignKey(nameof(CategoriaId))]
        public Categoria Categoria { get; set;}

    }
}
