
using Microsoft.AspNetCore.Http.Features;
using System.ComponentModel.DataAnnotations;

public class Presupuesto
{
    private int idPresupuesto;
    private string nombreDestinatario;
    private DateTime fechaCreacion;
    private List<PresupuestoDetalle> detalle;

    public Presupuesto(int idPresupuesto, string nombreDestinatario, DateTime fechaCreacion, List<PresupuestoDetalle> detalle)
    {
        this.idPresupuesto = idPresupuesto;
        this.nombreDestinatario = nombreDestinatario;
        this.fechaCreacion = fechaCreacion;
        this.detalle = detalle ?? new List<PresupuestoDetalle>();
    }

    public Presupuesto()
    {
    }

    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
    public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }
    public double MontoPresupuesto()
    {
        double subtotal = 0;
        foreach (var aux in detalle)
        {
            subtotal += aux.Producto.Precio * aux.Cantidad;
        }
        return subtotal;
    }

    public double MontoPresupuestoConIva()
    {

        return MontoPresupuesto() * 1.21;
    }

    public int CantidadProductos()
    {
        int cantidad = 0;
        foreach (var aux in detalle)
        {
            cantidad += aux.Cantidad;
        }
        return cantidad;
    }
}

