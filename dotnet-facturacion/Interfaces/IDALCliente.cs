using dotnet_facturacion.Layers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_facturacion.Interfaces
{
    public interface IDALCliente
    {
        List<Cliente> GetClienteByFilter(string pDescripcion);
        Cliente GetClienteById(string pId);
        Task<IEnumerable<Cliente>> GetAllCliente();
        Task<Cliente> SaveCliente(Cliente pCliente);
        Task<Cliente> UpdateCliente(Cliente pCliente);
        Task<bool> DeleteCliente(string pId);
    }
}
