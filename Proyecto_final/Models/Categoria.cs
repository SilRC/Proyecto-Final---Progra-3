using System;
using System.Collections.Generic;

namespace Proyecto_final.Models;

public partial class Categoria
{
    public int CategoriaId { get; set; }

    public string NombreCategoria { get; set; } = null!;

    public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
}
