using System.ComponentModel.DataAnnotations;

public class Productos
{

    int idProducto;
    string descripcion;
    decimal precio;

    public Productos()
    {
    }

    public Productos(int idProducto, string descripcion, decimal precio)
    {
        this.descripcion = descripcion;
        this.idProducto = idProducto;
        this.precio = precio;
    }

    public int IdProducto { get => idProducto; set => idProducto = value; }
    public string Descripcion { get => descripcion; set => descripcion = value; }
    public decimal Precio { get => precio; set => precio = value; }
}