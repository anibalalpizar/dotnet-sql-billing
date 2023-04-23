using dotnet_facturacion.Layers.Entities.DTO;
using dotnet_facturacion.Layers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_facturacion.Interfaces
{
    public interface IDALElectronico
    {
        Electronico GetElectronicoById(double pId);
        List<ElectronicoBodegaDTO> GetAllElectronico();
        List<Electronico> GetElectronicoByFilter(string pDescripcion);
        Electronico SaveElectronico(Electronico pElectronico);
        Electronico UpdateElectronico(Electronico pElectronico);
        bool DeleteElectronico(double pId);
    }
}
