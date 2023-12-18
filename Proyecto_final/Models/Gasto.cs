using System;
using System.Collections.Generic;

namespace Proyecto_final.Models;

public partial class Gasto
{
    public int GastoId { get; set; }

    public int? CuentaId { get; set; }

    public int? CategoriaId { get; set; }

    public string Descripcion { get; set; } = null!;

    public decimal Monto { get; set; }

    public DateTime? FechaGasto { get; set; }

    public virtual Categoria? Categoria { get; set; }

    public virtual Cuenta? Cuenta { get; set; }
}
