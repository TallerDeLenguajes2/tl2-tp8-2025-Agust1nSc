public class PresupuestoRepository
{
    private List<Presupuesto> ListaPresupuestos;
  public PresupuestoRepository()
{
    ListaPresupuestos = new List<Presupuesto>()
    {
        new Presupuesto
        {
            IdPresupuesto = 1,
            NombreDestinatario = "Juan Pérez",
            FechaCreacion = DateTime.Now,
            Detalle = new List<PresupuestoDetalle>()
            {
                new PresupuestoDetalle(
                    new Productos { IdProducto = 1, Descripcion = "Mouse", Precio = 2000 }, 
                    2
                ),
                new PresupuestoDetalle(
                    new Productos { IdProducto = 2, Descripcion = "Teclado", Precio = 4500 },
                    1
                )
            }
        },

        new Presupuesto
        {
            IdPresupuesto = 2,
            NombreDestinatario = "María Gómez",
            FechaCreacion = DateTime.Now,
            Detalle = new List<PresupuestoDetalle>()
            {
                new PresupuestoDetalle(
                    new Productos { IdProducto = 3, Descripcion = "Monitor", Precio = 65000 },
                    1
                )
            }
        }
    };
}

    public void Crear(Presupuesto presupuesto)
    {
        ListaPresupuestos.Add(presupuesto);
    }

    public List<Presupuesto> Listar()
    {
        return ListaPresupuestos;
    }

    public Presupuesto ObtenerPorId(int id)
    {
        return ListaPresupuestos.FirstOrDefault(p => p.IdPresupuesto == id);
    }

    public bool AgregarProductoAPresupuesto(int idPresupuesto, Productos producto, int cantidad)
    {

        var presupuesto = ListaPresupuestos.FirstOrDefault(p => p.IdPresupuesto == idPresupuesto);

        if (presupuesto == null)
        {
            return false;
        }

        var detalleExistente = presupuesto.Detalle.FirstOrDefault(d => d.Producto.IdProducto == producto.IdProducto);

        if (detalleExistente != null)
        {
            detalleExistente.Cantidad += cantidad;
        }
        else
        {
            var nuevoDetalle = new PresupuestoDetalle(producto, cantidad);
            presupuesto.Detalle.Add(nuevoDetalle);
        }

        return true;
    }

    public bool Eliminar(int id)
    {
        var producto = ListaPresupuestos.FirstOrDefault(p => p.IdPresupuesto == id);
        if (producto != null)
        {
            ListaPresupuestos.Remove(producto);
            return true;
        }
        return false;
    }

public bool Modificar(int id, Presupuesto presupuestoEditado)
{
    var presupuestoExistente = ListaPresupuestos.FirstOrDefault(p => p.IdPresupuesto == id);
    if (presupuestoExistente != null)
    {
        presupuestoExistente.NombreDestinatario = presupuestoEditado.NombreDestinatario;
        presupuestoExistente.FechaCreacion = presupuestoEditado.FechaCreacion;
        return true;
    }
    return false;
}













}