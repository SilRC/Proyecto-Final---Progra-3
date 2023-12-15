using System;
using System.Collections.Generic;

namespace Proyecto_final.Models;

public partial class Cuenta
{
    public int CuentaId { get; set; }

    public int? UserId { get; set; }

    public string NombreCuenta { get; set; } = null!;

    public decimal? SaldoInicial { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();

    public virtual ICollection<Ingreso> Ingresos { get; set; } = new List<Ingreso>();

    public virtual Usuario? User { get; set; }
}
