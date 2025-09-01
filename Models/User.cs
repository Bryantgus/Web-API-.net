using System;
using System.Collections.Generic;

namespace CRUDconSQL.Models;

public partial class User
{
    public int Id { get; set; }

    public string Usuario { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }
}
