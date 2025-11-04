using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
    public IActionResult Create(Productos producto)
    {
        producto.IdProducto = productoRepository.Listar().Max(p => p.IdProducto) + 1;
        productoRepository.Crear(producto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var producto = productoRepository.ObtenerPorId(id);
        if (producto == null) return NotFound();

        return View(producto);
    }

    [HttpPost]
    public IActionResult Edit(Productos producto)
    {
        productoRepository.Modificar(producto.IdProducto, producto);
        return RedirectToAction("Index");
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
