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
    public class BLLCliente : IBLLCliente
    {
        public List<Cliente> GetClienteByFilter(string pDescripcion)
        {
            IDALCliente _DALCliente = new DALCliente();
            return _DALCliente.GetClienteByFilter(pDescripcion);
        }

        public Cliente GetClienteById(string pIdCliente)
        {
            IDALCliente _DALCliente = new DALCliente();
            return _DALCliente.GetClienteById(pIdCliente);
        }


        public Task<IEnumerable<Cliente>> GetAllCliente()
        {
            IDALCliente _DALCliente = new DALCliente();
            return _DALCliente.GetAllCliente();
        }

        public Task<Cliente> SaveCliente(Cliente pCliente)
        {
            IDALCliente _DALCliente = new DALCliente();
            Task<Cliente> oCliente = null;

            if (_DALCliente.GetClienteById(pCliente.IdCliente) == null)
                oCliente = _DALCliente.SaveCliente(pCliente);
            else
                oCliente = _DALCliente.UpdateCliente(pCliente);

            return oCliente;
        }

        public Task<bool> DeleteCliente(string pId)
        {
            IDALCliente _DALCliente = new DALCliente();

            return _DALCliente.DeleteCliente(pId);

        }
    }
}
