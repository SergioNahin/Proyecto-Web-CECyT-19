using Egresados.AccesoDatos.Data.Repository.IRepository;
using Egresados.Data;
using Egresados.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Data;

namespace Egresados.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class CategoriasController : Controller
    {
        public readonly IContenedorTrabajo _contenedorTrabajo;
        //private readonly ApplicationDbContext _context;

        public CategoriasController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
            //_context = context;

        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {
            if(ModelState.IsValid)
            {
                _contenedorTrabajo.Categoria.Add(categoria);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Categoria categoria = new Categoria();
            categoria = _contenedorTrabajo.Categoria.Get(id);
            if(categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _contenedorTrabajo.Categoria.Update(categoria);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        #region Llamada a la API

        [HttpGet]
        public IActionResult GetAll()
        {
            //Opcion 1
            return Json(new {data = _contenedorTrabajo.Categoria.GetAll()});
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDB = _contenedorTrabajo.Categoria.Get(id);
            if (objFromDB == null)
            {
                return Json(new {success = false,message = "Error Borrando Categoria"});
            }
            _contenedorTrabajo.Categoria.Remove(objFromDB);
            _contenedorTrabajo.Save();

            return Json(new { success = true, message = "Categoria Borrada Correctamente" });

        }



        #endregion
    }
}
