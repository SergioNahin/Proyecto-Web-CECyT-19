using Egresados.AccesoDatos.Data.Repository.IRepository;
using Egresados.Data;
using Egresados.Models;
using Egresados.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using System;
using System.Data;

namespace Egresados.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ArticulosController : Controller
    {
        public readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostingEnviroment;   

        public ArticulosController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostEnvironment)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _hostingEnviroment = hostEnvironment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {

            ArticuloVM artivm = new ArticuloVM()
            {         
              Articulo = new Egresados.Models.Articulo(),
              ListaCategorias = _contenedorTrabajo.Categoria.GetSelectCategoria()
            };

            return View(artivm);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArticuloVM artiVM)
        {
            if(ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnviroment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                if(artiVM.Articulo.Id == 0)
                {
                    //Nuevo Articulo
                    string nombreArchivo =  Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes/articulo");
                    var extension = Path.GetExtension(archivos[0].FileName); //extrae la extencion

                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension),FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    artiVM.Articulo.UrlImagen = @"/imagenes/articulo/" + nombreArchivo + extension;
                    artiVM.Articulo.FechaCreacion = DateTime.Now.ToString();
                    _contenedorTrabajo.Articulo.Add(artiVM.Articulo);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
            }

            artiVM.ListaCategorias = _contenedorTrabajo.Categoria.GetSelectCategoria();
            return View(artiVM);
           
        }


        [HttpGet]
        public IActionResult Edit(int ? id)
        {

            ArticuloVM artivm = new ArticuloVM()
            {
                Articulo = new Egresados.Models.Articulo(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetSelectCategoria()
            };

            if (id != null)
            {
                artivm.Articulo = _contenedorTrabajo.Articulo.Get(id.GetValueOrDefault());
            }

            return View(artivm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArticuloVM artiVM)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnviroment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                var articuloDesdeDb = _contenedorTrabajo.Articulo.Get(artiVM.Articulo.Id);

                if (archivos.Count() > 0)
                {
                    //Nueva imagen para el articulo
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes/articulo");
                    var extension = Path.GetExtension(archivos[0].FileName); //extrae la extencion
                    var nuevaExtension = Path.GetExtension(archivos[0].FileName);


                    var rutaImagen = Path.Combine(rutaPrincipal,articuloDesdeDb.UrlImagen.TrimStart('\\'));

                    if(System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }

                    //Subida nuevamente del archivo
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    artiVM.Articulo.UrlImagen = @"/imagenes/articulo/" + nombreArchivo + extension;
                    artiVM.Articulo.FechaCreacion = DateTime.Now.ToString();
                    _contenedorTrabajo.Articulo.Update(artiVM.Articulo);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Cuando Se conserva la imagen es este else
                    artiVM.Articulo.UrlImagen = articuloDesdeDb.UrlImagen;
                }
                _contenedorTrabajo.Articulo.Update(artiVM.Articulo);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }

           return View(artiVM);

        }

        

        #region Llamada a la API

        [HttpGet]
        public IActionResult GetAll()
        {
            //Opcion 1
            return Json(new { data = _contenedorTrabajo.Articulo.GetAll(includeProperties: "Categoria") });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var articuloDesdeDb = _contenedorTrabajo.Articulo.Get(id);
            string rutaDirectorioPrincipal = _hostingEnviroment.WebRootPath;
            var rutaImagen = Path.Combine(rutaDirectorioPrincipal, articuloDesdeDb.UrlImagen.TrimStart('\\'));

            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }

            if (articuloDesdeDb == null)
            {
                return Json(new { success = false, message = "Error Borrando Artículo" });
            }


            _contenedorTrabajo.Articulo.Remove(articuloDesdeDb);
            _contenedorTrabajo.Save();

            return Json(new { success = true, message = "Artículo Borrada Correctamente" });

        }

        #endregion

    }
}
