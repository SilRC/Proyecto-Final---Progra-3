using System;
using System.Collections.Generic;

namespace Proyecto_final.Models;

public partial class Usuario
{
    public int UserId { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public virtual ICollection<Cuenta> Cuenta { get; set; } = new List<Cuenta>();
}
