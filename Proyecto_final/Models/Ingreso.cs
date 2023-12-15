using System;
using System.Collections.Generic;

namespace Proyecto_final.Models;

public partial class Ingreso
{
    public int IngresoId { get; set; }

    public int? CuentaId { get; set; }

    public string Descripcion { get; set; } = null!;

    public decimal Monto { get; set; }

    public DateOnly FechaIngreso { get; set; }

    public virtual Cuenta? Cuenta { get; set; }
}
