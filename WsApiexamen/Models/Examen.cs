using System;
using System.Collections.Generic;

namespace WsApiexamen.Models;

public partial class Examen
{
    public int IdExamen { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }
}
