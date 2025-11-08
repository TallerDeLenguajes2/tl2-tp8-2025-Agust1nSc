using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SistemaVentas.Web.ViewModels;

public class ProductosController : Controller
{
    private ProductoRepository productoRepository;

    public ProductosController()
    {
        productoRepository = new ProductoRepository();
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Productos> productos = productoRepository.Listar();
        return View(productos);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(ProductoViewModel productoVM)
    {
        // 1. CHEQUEO DE SEGURIDAD DEL SERVIDOR
        if (!ModelState.IsValid)
        {
            // Si falla: Devolvemos el ViewModel con los datos y errores a la Vista
            return View(productoVM);
        }

        // 2. SI ES VÁLIDO: Mapeo Manual de VM a Modelo de Dominio
        var nuevoProducto = new Productos
        {
            Descripcion = productoVM.Descripcion,
            Precio = productoVM.Precio
        };
        // 3. Llamada al Repositorio
        productoRepository.Crear(nuevoProducto);

        return RedirectToAction(nameof(Index));
    }



    [HttpGet]
    public IActionResult Edit(int id)
    {
        var producto = productoRepository.ObtenerPorId(id);
        if (producto == null) return NotFound();

        return View(producto);
    }

    [HttpPost]
    public IActionResult Edit(int id, ProductoViewModel productoVM)
    {
        if (id != productoVM.IdProducto) return NotFound();// 1. CHEQUEO DE SEGURIDAD DEL SERVIDOR
        if (!ModelState.IsValid)
        {
            return View(productoVM);
        }
        // 2. Mapeo Manual de VM a Modelo de Dominio
        var productoAEditar = new Productos
        {
            IdProducto = productoVM.IdProducto, // Necesario para el UPDATE
            Descripcion = productoVM.Descripcion,
            Precio = productoVM.Precio
        };
        // 3. Llamada al Repositorio
        productoRepository.Modificar(productoVM.IdProducto, productoAEditar);
        return RedirectToAction(nameof(Index));
    }



[HttpGet]
public IActionResult Delete(int id)
{
    var producto = productoRepository.ObtenerPorId(id);
    if (producto == null) return NotFound();

    return View(producto);
}

[HttpPost]
public IActionResult Delete(Productos producto)
{
    productoRepository.Eliminar(producto.IdProducto);
    return RedirectToAction("Index");
}
}
