public class ProductoRepository
{
    private List<Productos> ListaProductos;

    public ProductoRepository()
    {
        ListaProductos = new List<Productos>()
        {
            new Productos { IdProducto = 1, Descripcion = "Mouse", Precio = 2000 },
            new Productos { IdProducto = 2, Descripcion = "Teclado", Precio = 4500 },
            new Productos { IdProducto = 3, Descripcion = "Monitor", Precio = 65000 }
        };
    }

    

    public void Crear(Productos producto)
    {
        ListaProductos.Add(producto);
    }

    public bool Modificar(int id, Productos nuevoProducto)
    {
        var productoExistente = ListaProductos.FirstOrDefault(p => p.IdProducto == id);
        if (productoExistente != null)
        {
            productoExistente.Descripcion = nuevoProducto.Descripcion;
            productoExistente.Precio = nuevoProducto.Precio;
            return true;
        }
        return false;
    }

    public List<Productos> Listar()
    {
        return ListaProductos;
    }

    public Productos ObtenerPorId(int id)
    {
        return ListaProductos.FirstOrDefault(p => p.IdProducto == id);
    }

    public bool Eliminar(int id)
    {
        var producto = ListaProductos.FirstOrDefault(p => p.IdProducto == id);
        if (producto != null)
        {
            ListaProductos.Remove(producto);
            return true;
        }
        return false;
    }






}