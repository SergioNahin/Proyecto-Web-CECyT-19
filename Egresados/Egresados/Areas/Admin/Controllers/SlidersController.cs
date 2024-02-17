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
    public class SlidersController : Controller
    {
        public readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostingEnviroment;   

        public SlidersController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostEnvironment)
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
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider)
        {
            if(ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnviroment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
               
                    //Nuevo Slider
                    string nombreArchivo =  Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes/sliders");
                    var extension = Path.GetExtension(archivos[0].FileName); //extrae la extencion

                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension),FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    slider.UrlImagen = @"/imagenes/sliders/" + nombreArchivo + extension;
                    

                    _contenedorTrabajo.Slider.Add(slider);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                
            }
            
            return View();
           
        }


        [HttpGet]
        public IActionResult Edit(int ? id)
        {
            if (id != null)
            {
                var slider = _contenedorTrabajo.Slider.Get(id.GetValueOrDefault());
                return View(slider);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Slider slider)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnviroment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                var sliderDesdeDb = _contenedorTrabajo.Slider.Get(slider.Id);

                if (archivos.Count() > 0)
                {
                    //Nueva imagen para slider
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes/sliders");
                    var extension = Path.GetExtension(archivos[0].FileName); //extrae la extencion
                    var nuevaExtension = Path.GetExtension(archivos[0].FileName);


                    var rutaImagen = Path.Combine(rutaPrincipal,sliderDesdeDb.UrlImagen.TrimStart('\\'));

                    if(System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }

                    //Subida nuevamente del archivo
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    slider.UrlImagen = @"/imagenes/sliders/" + nombreArchivo + extension;
                    
                    _contenedorTrabajo.Slider.Update(slider);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Cuando Se conserva la imagen es este else
                    slider.UrlImagen = sliderDesdeDb.UrlImagen;
                }
                _contenedorTrabajo.Slider.Update(slider);
                _contenedorTrabajo.Save();

                return RedirectToAction(nameof(Index));

            }

            return View();

        }

        

        #region Llamada a la API

        [HttpGet]
        public IActionResult GetAll()
        {
            //Opcion 1
            return Json(new { data = _contenedorTrabajo.Slider.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var sliderDesdeDb = _contenedorTrabajo.Slider.Get(id);
           

            if (sliderDesdeDb == null)
            {
                return Json(new { success = false, message = "Error Borrando Presentación" });
            }


            _contenedorTrabajo.Slider.Remove(sliderDesdeDb);
            _contenedorTrabajo.Save();

            return Json(new { success = true, message = "Presentación Borrada Correctamente" });

        }

        #endregion

    }
}
