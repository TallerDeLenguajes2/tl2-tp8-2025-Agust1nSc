using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MVC.Interfaces;
using MVC.Models;
using MVC.ViewModels;
using SistemaVentas.Web.ViewModels;

namespace MVC.Controllers // Asegurate que el namespace sea Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoRepository productoRepository;
        private readonly IAuthenticationService _authService;

        public ProductosController(IProductoRepository repo, IAuthenticationService authService)
        {
            productoRepository = repo;
            _authService = authService;
        }

        // --- VALIDACIÓN PARA VER (Admins y Clientes) ---
        private IActionResult CheckLecturaPermissions()
        {
            if (!_authService.IsAuthenticated()) return RedirectToAction("Index", "Login");
            
            // Si NO es Admin Y NO es Cliente -> Fuera
            if (!_authService.HasAccessLevel("Administrador") && !_authService.HasAccessLevel("Cliente"))
            {
                return RedirectToAction("AccesoDenegado");
            }
            return null;
        }

        // --- VALIDACIÓN PARA MODIFICAR (Solo Admins) ---
        private IActionResult CheckEscrituraPermissions()
        {
            if (!_authService.IsAuthenticated()) return RedirectToAction("Index", "Login");

            // Solo Admin
            if (!_authService.HasAccessLevel("Administrador"))
            {
                return RedirectToAction("AccesoDenegado");
            }
            return null;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Permitimos lectura a Clientes y Admins
            var securityCheck = CheckLecturaPermissions();
            if (securityCheck != null) return securityCheck;

            List<Productos> productos = productoRepository.Listar();
            return View(productos);
        }

        // --- A PARTIR DE AQUI USAMOS CheckEscrituraPermissions (Solo Admin) ---

        [HttpGet]
        public IActionResult Create()
        {
            var securityCheck = CheckEscrituraPermissions();
            if (securityCheck != null) return securityCheck;

            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductoViewModel productoVM)
        {
            var securityCheck = CheckEscrituraPermissions();
            if (securityCheck != null) return securityCheck;

            if (!ModelState.IsValid) return View(productoVM);

            var nuevoProducto = new Productos
            {
                Descripcion = productoVM.Descripcion,
                Precio = productoVM.Precio
            };

            productoRepository.Crear(nuevoProducto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var securityCheck = CheckEscrituraPermissions();
            if (securityCheck != null) return securityCheck;

            var producto = productoRepository.ObtenerPorId(id);
            if (producto == null) return NotFound();

            return View(producto);
        }

        [HttpPost]
        public IActionResult Edit(int id, ProductoViewModel productoVM)
        {
            var securityCheck = CheckEscrituraPermissions();
            if (securityCheck != null) return securityCheck;

            if (id != productoVM.IdProducto) return NotFound();
            if (!ModelState.IsValid) return View(productoVM);

            var productoAEditar = new Productos
            {
                IdProducto = productoVM.IdProducto,
                Descripcion = productoVM.Descripcion,
                Precio = productoVM.Precio
            };

            productoRepository.Modificar(productoVM.IdProducto, productoAEditar);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var securityCheck = CheckEscrituraPermissions();
            if (securityCheck != null) return securityCheck;

            var producto = productoRepository.ObtenerPorId(id);
            if (producto == null) return NotFound();

            return View(producto);
        }

        [HttpPost]
        public IActionResult Delete(Productos producto)
        {
            var securityCheck = CheckEscrituraPermissions();
            if (securityCheck != null) return securityCheck;

            productoRepository.Eliminar(producto.IdProducto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AccesoDenegado()
        {
            // Esta vista mostrará el botón de volver a Productos
            return View();
        }
    }
}