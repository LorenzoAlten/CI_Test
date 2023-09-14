using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ToyotaClientWebServiceSoapMgr.Models
{

    public enum OperationTypes
    {
        Drop,
        Fetch,
        CheckPoint
    }
    public class ToyotaOrder
    {
        [Required]
        public long OrderId { get; set; }
        //UDC
        [Required]
        public string GoodsId { get; set; }  
        //Mission detail
        public List<ToyotaDetailsOrder> LineMission {  get; set; } 
        //Pallet Type
        public int GoodsCarrier { get; set; }
        public int Priority { get; set; }
    }
    public class ToyotaDetailsOrder
    {
        public int TaskId { get; set; }
        public int OperationType { get; set;}
        public string Location { get; set;}
        public int Quantity { get; set; }                   
    }
}
