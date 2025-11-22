using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using MVC.Interfaces;
using SistemaVentas.Web.ViewModels; 
using MVC.Models; 

namespace MVC.Controllers
{
    public class PresupuestosController : Controller
    {
        private readonly IPresupuestoRepository repo;
        private readonly IProductoRepository repoProd;
        private readonly IAuthenticationService _authService;

        public PresupuestosController(IPresupuestoRepository r, IProductoRepository rProd, IAuthenticationService authService)
        {
            repo = r;
            repoProd = rProd;
            _authService = authService;
        }

        // --- MÉTODO PRIVADO PARA VALIDAR PERMISOS DE ESCRITURA (Punto 9 - Solo Admin) ---
        private IActionResult VerificarPermisosAdmin()
        {
            if (!_authService.IsAuthenticated())
            {
                return RedirectToAction("Index", "Login");
            }
            if (!_authService.HasAccessLevel("Administrador"))
            {
                return RedirectToAction("AccesoDenegado");
            }
            return null; 
        }

        // --- MÉTODO PRIVADO PARA VALIDAR PERMISOS DE LECTURA (Admin o Cliente) ---
        private IActionResult VerificarPermisosLectura()
        {
            if (!_authService.IsAuthenticated())
            {
                return RedirectToAction("Index", "Login");
            }
            if (!_authService.HasAccessLevel("Administrador") && !_authService.HasAccessLevel("Cliente"))
            {
                return RedirectToAction("AccesoDenegado");
            }
            return null;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // PROTECCIÓN DE LECTURA
            var check = VerificarPermisosLectura();
            if (check != null) return check;

            var lista = repo.Listar();
            return View(lista);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            // PROTECCIÓN DE LECTURA
            var check = VerificarPermisosLectura();
            if (check != null) return check;

            var p = repo.ObtenerPorId(id);
            if (p == null) return NotFound();
            return View(p);
        }

        // ---------------------------------------------------------
        // A PARTIR DE AQUÍ SON ACCIONES DE MODIFICACIÓN (SOLO ADMIN)
        // ---------------------------------------------------------

        [HttpGet]
        public IActionResult Create()
        {
            var check = VerificarPermisosAdmin();
            if (check != null) return check;

            var modelo = new PresupuestoViewModel
            {
                FechaCreacion = DateTime.Now
            };
            return View(modelo);
        }

        [HttpPost]
        public IActionResult Create(PresupuestoViewModel presupuestoVM)
        {
            var check = VerificarPermisosAdmin();
            if (check != null) return check;

            if (!ModelState.IsValid)
            {
                return View(presupuestoVM);
            }

            if (presupuestoVM.FechaCreacion > DateTime.Now)
            {
                ModelState.AddModelError("FechaCreacion", "La fecha no puede ser futura.");
                return View(presupuestoVM);
            }

            var nuevoPresupuesto = new Presupuesto
            {
                NombreDestinatario = presupuestoVM.NombreDestinatario,
                FechaCreacion = presupuestoVM.FechaCreacion,
                Detalle = new List<PresupuestoDetalle>()
            };

            repo.Crear(nuevoPresupuesto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var check = VerificarPermisosAdmin();
            if (check != null) return check;

            var presupuesto = repo.ObtenerPorId(id);
            if (presupuesto == null) return NotFound();

            var presupuestoVM = new PresupuestoViewModel
            {
                IdPresupuesto = presupuesto.IdPresupuesto,
                NombreDestinatario = presupuesto.NombreDestinatario,
                FechaCreacion = presupuesto.FechaCreacion
            };

            return View(presupuestoVM);
        }

        [HttpPost]
        public IActionResult Edit(int id, PresupuestoViewModel presupuestoVM)
        {
            var check = VerificarPermisosAdmin();
            if (check != null) return check;

            if (id != presupuestoVM.IdPresupuesto) return NotFound();
            if (!ModelState.IsValid) return View(presupuestoVM);

            var presupuestoAEditar = new Presupuesto
            {
                IdPresupuesto = presupuestoVM.IdPresupuesto,
                NombreDestinatario = presupuestoVM.NombreDestinatario,
                FechaCreacion = presupuestoVM.FechaCreacion
            };

            repo.Modificar(id, presupuestoAEditar);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var check = VerificarPermisosAdmin();
            if (check != null) return check;

            var p = repo.ObtenerPorId(id);
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpPost]
        public IActionResult Delete(Presupuesto presupuesto)
        {
            var check = VerificarPermisosAdmin();
            if (check != null) return check;

            repo.Eliminar(presupuesto.IdPresupuesto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AgregarProducto(int id)
        {
            
            var check = VerificarPermisosAdmin();
            if (check != null) return check;

            var presupuesto = repo.ObtenerPorId(id);
            if (presupuesto == null) return NotFound();

            ViewBag.Productos = repoProd.Listar();
            return View(presupuesto);
        }

        [HttpPost]
        public IActionResult AgregarProducto(int IdPresupuesto, int IdProducto, int Cantidad)
        {
            var check = VerificarPermisosAdmin();
            if (check != null) return check;

            if (IdProducto <= 0 || Cantidad <= 0)
            {
                var presupuesto = repo.ObtenerPorId(IdPresupuesto);
                ViewBag.Productos = repoProd.Listar();
                ViewBag.Error = "Debe seleccionar un producto y una cantidad válida.";
                return View(presupuesto);
            }

            var producto = repoProd.ObtenerPorId(IdProducto);
            repo.AgregarProductoAPresupuesto(IdPresupuesto, producto, Cantidad);

            return RedirectToAction(nameof(Details), new { id = IdPresupuesto });
        }
        
        [HttpGet]
        public IActionResult AccesoDenegado()
        {
            return View();
        }
    }
}