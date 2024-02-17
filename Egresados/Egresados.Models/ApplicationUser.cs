using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Egresados.Models
{
        public class ApplicationUser : IdentityUser
        {
        [Required(ErrorMessage = "El Nombre es Obligatorio")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La Boleta es Obligatoria")]
        [Display(Name = "Boleta")]
        public string Boleta { get; set; }

        [Required(ErrorMessage = "El Grupo es Obligatorio")]
        [Display(Name = "Grupo")]
        public string Grupo { get; set; }

        [Required(ErrorMessage = "La Generación es Obligatoria")]
        [Display(Name = "Generación")]
        public string Generacion { get; set; }

        [Required(ErrorMessage = "El CURP es Obligatorio")]
        [Display(Name = "CURP")]
        public string CURP { get; set; }

        [Required(ErrorMessage = "La Fecha de Nacimiento es Obligatoria")]
        [Display(Name = "Fecha de Nacimiento")]
        public string FechaNacimiento { get; set; }

        [Required(ErrorMessage = "La Edad es Obligatoria")]
        [Display(Name = "Edad")]
        public string Edad { get; set; }

        [Required(ErrorMessage = "El Sexo es Obligatorio")]
        [Display(Name = "Sexo")]
        public string Sexo { get; set; }

        //Email institucional
        [Required(ErrorMessage = "El Correo Institucional es Obligatorio")]
        [Display(Name = "Correo Institucional")]
        public string Correo_Institucional { get; set; }

        //Campos domicilio
        [Required(ErrorMessage = "El Domicilio es Obligatorio")]
        [Display(Name = "Domicilio Completo")]
        public string Domicilio { get; set; }

        [Required(ErrorMessage = "El celular es Obligatorio")]
        [Display(Name = "Celular")]
        public string Celular { get; set; }

        [Display(Name = "Telefono")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El Folio es Obligatorio")]
        [Display(Name = "Folio")]
        public string Folio { get; set; }

        //Datos Escolares
        [Required(ErrorMessage = "La Carrera es Obligatoria")]
        [Display(Name = "Carrera Egreso")]
        public string Carrera { get; set; }

        [Display(Name = "Superior")]
        public string Escuela { get; set; }
        public string Escuela2 { get; set; }
        public string Escuela3 { get; set; }

        [Display(Name = "Carrera")]
        public string CarreraSuperior { get; set; }
        public string CarreraSuperior2 { get; set; }
        public string CarreraSuperior3 { get; set; }

        [Display(Name = "Escuela Ajena")]
        public string EscuelaExterna { get; set; }
    }
    }

