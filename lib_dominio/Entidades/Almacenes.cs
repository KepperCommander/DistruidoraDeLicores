using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_dominio.Entidades
{
    public class Almacenes
    {
        [Key] public int IdAlmacen { get; set; } 
        public string Nombre { get; set; } = string.Empty;
        public string? Direccion { get; set; }
        public bool EsPrincipal { get; set; } = false;
    }
}