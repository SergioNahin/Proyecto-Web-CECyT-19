using ClosedXML.Excel;
using Egresados.AccesoDatos.Data.Repository.IRepository;
using Egresados.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class UsuariosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public UsuariosController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;

        }

        [HttpGet]
        public IActionResult Index()
        {
            //Opción 1: Obtener todos los usuarios
            //return View(_contenedorTrabajo.Usuario.GetAll());

            //Opción 2: Obtener todos los usuarios menos el que esté logueado, para no bloquearse el mismo
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var usuarioActual = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return View(_contenedorTrabajo.Usuario.GetAll(u => u.Id != usuarioActual.Value));
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _contenedorTrabajo.Usuario.EliminarUsuario(id);
            _contenedorTrabajo.Save();

            return Json(new { success = true, message = "Usuario eliminado correctamente" });
        }

        [HttpGet]
        public IActionResult Bloquear(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _contenedorTrabajo.Usuario.BloquearUsuario(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Desbloquear(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _contenedorTrabajo.Usuario.DesbloquearUsuario(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DownloadExcel()
        {
            // Obtén los datos que deseas exportar a Excel
            var data = ObtenerDatosParaExcel();

            // Crea un nuevo libro de Excel
            using (var workbook = new XLWorkbook())
            {
                // Agrega una hoja de Excel
                var worksheet = workbook.Worksheets.Add("Alumnos");

                // Agrega los encabezados de columna
                var headers = new List<string>
            {
                "Nombre", "Correo Institucional", "Boleta", "Escuela", "Carrera Superior", "Escuela_2", "Carrera Superior_2",
                "Escuela_3", "Carrera Superior_3", "Escuela Externa"
            };

                for (int i = 1; i <= headers.Count; i++)
                {
                    worksheet.Cell(1, i).Value = headers[i - 1];
                }

                // Agrega los datos a las filas
                for (int row = 2; row <= data.Count + 1; row++)
                {
                    var item = data[row - 2];
                    worksheet.Cell(row, 1).Value = item.Nombre;
                    worksheet.Cell(row, 2).Value = item.Correo_Institucional;
                    worksheet.Cell(row, 3).Value = item.Boleta;
                    worksheet.Cell(row, 4).Value = item.Escuela;
                    worksheet.Cell(row, 5).Value = item.CarreraSuperior;
                    worksheet.Cell(row, 6).Value = item.Escuela2;
                    worksheet.Cell(row, 7).Value = item.CarreraSuperior2;
                    worksheet.Cell(row, 8).Value = item.Escuela3;
                    worksheet.Cell(row, 9).Value = item.CarreraSuperior3;
                    worksheet.Cell(row, 10).Value = item.EscuelaExterna;
                }

                // Configura la respuesta para descargar el archivo Excel
                var stream = new System.IO.MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                var excelFileName = $"Usuarios_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelFileName);
            }
        }

        private List<ApplicationUser> ObtenerDatosParaExcel()
        {
            // Obtén todos los usuarios registrados en tu aplicación
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var usuarioActual = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var users = _contenedorTrabajo.Usuario.GetAll(u => u.Id != usuarioActual.Value);

            // Convierte los objetos ApplicationUser en una lista plana
            var data = users.Select(user => new ApplicationUser
            {
                Nombre = user.Nombre, 
                Correo_Institucional = user.Correo_Institucional, 
                Boleta = user.Boleta, 
                Escuela = user.Escuela, 
                CarreraSuperior = user.CarreraSuperior, 
                Escuela2 = user.Escuela2, 
                CarreraSuperior2 = user.CarreraSuperior2, 
                Escuela3 = user.Escuela3, 
                CarreraSuperior3 = user.CarreraSuperior3, 
                EscuelaExterna = user.EscuelaExterna 
            }).ToList();

            return data;

        }
    }
}
