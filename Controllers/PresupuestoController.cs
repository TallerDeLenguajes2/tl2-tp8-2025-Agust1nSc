using Microsoft.AspNetCore.Mvc;

public class PresupuestosController : Controller
{
    private PresupuestoRepository repo = new();
    private ProductoRepository repoProd = new();

    public IActionResult Index()
    {
        var lista = repo.Listar();
        return View(lista);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Presupuesto p)
    {
        p.FechaCreacion = DateTime.Now;
        repo.Crear(p);
        return RedirectToAction("Index");
    }

    public IActionResult Details(int id)
    {
        var p = repo.ObtenerPorId(id);
        if (p == null) return NotFound();
        return View(p);
    }

    public IActionResult Edit(int id)
    {
        var p = repo.ObtenerPorId(id);
        if (p == null) return NotFound();
        return View(p);
    }

   
    [HttpPost]
    public IActionResult Edit(Presupuesto presupuesto)
    {
        repo.Modificar(presupuesto.IdPresupuesto, presupuesto);
        return RedirectToAction("Index");
    }

    
    public IActionResult Delete(int id)
    {
        var p = repo.ObtenerPorId(id);
        if (p == null) return NotFound();
        return View(p);
    }

    
    [HttpPost]
    public IActionResult Delete(Presupuesto presupuesto)
    {
        repo.Eliminar(presupuesto.IdPresupuesto);
        return RedirectToAction("Index");
    }

    
}
