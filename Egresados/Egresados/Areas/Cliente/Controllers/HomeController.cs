using Egresados.AccesoDatos.Data.Repository.IRepository;
using Egresados.Models;
using Egresados.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Egresados.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        public readonly IContenedorTrabajo _contenedorTrabajo;

        public HomeController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }

        public IActionResult Index()
        {
            HomeVM homevm = new HomeVM()
            {
                Slider = _contenedorTrabajo.Slider.GetAll(),
                ListaArticulos = _contenedorTrabajo.Articulo.GetAll()
            };
            // saber si estamos en home o otra vista
            ViewBag.IsHome = true;
            return View(homevm);
        }

        public IActionResult Detalle(int id)
        {
            var articuloDesdeDb = _contenedorTrabajo.Articulo.Get(id);
            return View(articuloDesdeDb);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}