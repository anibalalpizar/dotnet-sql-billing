using dotnet_facturacion.Interfaces;
using dotnet_facturacion.Layers.DAL;
using dotnet_facturacion.Layers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_facturacion.Layers.BLL
{
    public class BLLTarjeta : IBLLTarjeta
    {
        public List<Tarjeta> GetAllTarjeta()
        {
            IDALTarjeta _DALTarjeta = new DALTarjeta();
            return _DALTarjeta.GetAllTarjeta();
        }    
    }
}
