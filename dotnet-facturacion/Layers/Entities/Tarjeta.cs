using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_facturacion.Layers.Entities
{
    public class Tarjeta
    {
        public int IdTarjeta { get; set; }
        public string DescripcionTarjeta { get; set; }
        public override string ToString() => $"{IdTarjeta} {DescripcionTarjeta}";
    }
}
