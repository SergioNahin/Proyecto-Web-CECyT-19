using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Egresados.Models.ViewModels
{
    public class ArticuloVM
    {
        public Articulo Articulo { get; set; }
        //Lista desplegable de categorias
        public IEnumerable<SelectListItem> ListaCategorias { get; set; }
    }
}
