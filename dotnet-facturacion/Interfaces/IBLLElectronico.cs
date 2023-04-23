using dotnet_facturacion.Layers.Entities;
using dotnet_facturacion.Layers.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_facturacion.Interfaces
{
    public interface IBLLElectronico
    {
        Electronico GetElectronicoById(double pId);
        List<ElectronicoBodegaDTO> GetAllElectronico();
        List<Electronico> GetElectronicoByFilter(string pDescripcion);
        Electronico SaveElectronico(Electronico pElectronico);
        bool DeleteElectronico(double pId);
        Electronico AvabilityStock(double pId, double pCantidadSolicitada);
    }
}
