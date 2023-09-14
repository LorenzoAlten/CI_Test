using System.ServiceModel;
using WebServiceSoapToyota;

namespace DataStore.Server.Interfaces
{
    [ServiceContract]
    public interface IOperations
    {
        //[OperationContract]
        //RegisterResponseModel Register(RegisterRequestModel data);
        [OperationContract]
        public int TransportOrderStatusUpdate(string orderId, int statusId, WebServiceOrderRow[] orderRows, int palletType, string udcCode, int palletWidth, int palletHeight, int palletLenght, int palletWeight, int vehicleId, int errorCode, int palletAmount);
    }
}
