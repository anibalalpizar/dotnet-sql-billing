using dotnet_facturacion.Interfaces;
using dotnet_facturacion.Layers.Entities.DTO;
using dotnet_facturacion.Layers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotnet_facturacion.Layers.DAL;

namespace dotnet_facturacion.Layers.BLL
{
    public class BLLElectronico : IBLLElectronico
    {
        public bool DeleteElectronico(double pId)
        {
            IDALElectronico _IDALElectronico = new DALElectronico();
            return _IDALElectronico.DeleteElectronico(pId);
        }

        public List<ElectronicoBodegaDTO> GetAllElectronico()
        {
            IDALElectronico _IDALElectronico = new DALElectronico();
            return _IDALElectronico.GetAllElectronico();

        }

        public List<Electronico> GetElectronicoByFilter(string pDescripcion)
        {
            IDALElectronico _IDALElectronico = new DALElectronico();
            return _IDALElectronico.GetElectronicoByFilter(pDescripcion);
        }

        public Electronico GetElectronicoById(double pId)
        {
            IDALElectronico _IDALElectronico = new DALElectronico();
            Electronico oElectronico = _IDALElectronico.GetElectronicoById(pId);
            return oElectronico;
        }

        public Electronico SaveElectronico(Electronico pElectronico)
        {
            IDALElectronico _IDALElectronico = new DALElectronico();
            Electronico oElectronico = null;
            if (_IDALElectronico.GetElectronicoById(pElectronico.IdElectronico) == null)
                oElectronico = _IDALElectronico.SaveElectronico(pElectronico);
            else
                oElectronico = _IDALElectronico.UpdateElectronico(pElectronico);

            return oElectronico;
        }


        /// <summary>
        /// VAlida la cantidad disponible en inventario
        /// </summary>
        /// <param name="pId">Codigo del artículo</param>
        /// <param name="pCantidadSolicitada">Cantidad solicitada</param>
        /// <returns></returns>
        public Electronico AvabilityStock(double pId, double pCantidadSolicitada)
        {
            IDALElectronico _IDALElectronico = new DALElectronico();
            Electronico oElectronico = _IDALElectronico.GetElectronicoById(pId);

            if (oElectronico == null)
            {
                throw new Exception($"El código {pId} no existe!");
            }

            if (oElectronico.Cantidad < pCantidadSolicitada)
                throw new Exception($"No hay cantidad suficiente en inventario para el producto {oElectronico.IdElectronico} {oElectronico.DescripcionElectronico}, cantidad disponible: {oElectronico.Cantidad}");
            else
                return oElectronico;
        }
    }
}
