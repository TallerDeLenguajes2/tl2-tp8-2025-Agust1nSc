using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

public class PresupuestosController : Controller
{
    private PresupuestoRepository presupuestoRepository;

    public PresupuestosController()
    {
        presupuestoRepository = new PresupuestoRepository();
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Presupuesto> presupuestos = presupuestoRepository.Listar();
        return View(presupuestos);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var presupuesto = presupuestoRepository.ObtenerPorId(id);
        if (presupuesto == null) return NotFound();
        return View(presupuesto);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Presupuesto presupuesto)
    {
        presupuesto.IdPresupuesto = presupuestoRepository.Listar().Max(p => p.IdPresupuesto) + 1;
        presupuestoRepository.Crear(presupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var presupuesto = presupuestoRepository.ObtenerPorId(id);
        if (presupuesto == null) return NotFound();
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult Edit(Presupuesto presupuesto)
    {
        presupuestoRepository.Modificar(presupuesto.IdPresupuesto, presupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var presupuesto = presupuestoRepository.ObtenerPorId(id);
        if (presupuesto == null) return NotFound();
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult Delete(Presupuesto presupuesto)
    {
        presupuestoRepository.Eliminar(presupuesto.IdPresupuesto);
        return RedirectToAction("Index");
    }
}
