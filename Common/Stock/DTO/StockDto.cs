using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Stock.DTO
{
    public class StockDto
    {
        public int StockCode { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
        public int UnitInStock { get; set; }
    }
}
