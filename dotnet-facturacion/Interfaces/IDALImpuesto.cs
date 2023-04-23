using dotnet_facturacion.Layers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_facturacion.Interfaces
{
    public interface IDALImpuesto
    {
        Impuesto GetImpuesto();
    }
}
