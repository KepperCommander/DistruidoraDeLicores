using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class ClientesPrueba
    {
        private readonly IConexion? iConexion;
        private Clientes? entidad;
        private List<Clientes>? lista;

        public ClientesPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.IsTrue(Guardar(), "Guardar() falló");
            Assert.IsTrue(Modificar(), "Modificar() falló");
            Assert.IsTrue(Listar(), "Listar() falló");
            Assert.IsTrue(Borrar(), "Borrar() falló");
        }

        public bool Guardar()
        {
            entidad = new Clientes
            {
                RazonSocial = "Cliente Prueba",
                NIT = $"NIT-{Guid.NewGuid():N}".Substring(0, 20), // único
                Email = "cliente.prueba@demo.local",
                Telefono = "3200000000",
                Direccion = "Cll 123 #45-67",
                Activo = true
            };

            iConexion!.Clientes!.Add(entidad);
            iConexion.SaveChanges();
            return entidad.IdCliente > 0;
        }

        public bool Modificar()
        {
            Assert.IsNotNull(entidad, "No hay entidad para modificar.");
            entidad!.RazonSocial += " Editado";
            entidad.Activo = !entidad.Activo;
            iConexion!.SaveChanges();
            return true;
        }

        public bool Listar()
        {
            lista = iConexion!.Clientes!.AsNoTracking().Take(50).ToList();
            return lista.Count > 0;
        }

        public bool Borrar()
        {
            if (entidad is null) return true;
            iConexion!.Clientes!.Remove(entidad);
            iConexion.SaveChanges();
            return true;
        }
    }
}
