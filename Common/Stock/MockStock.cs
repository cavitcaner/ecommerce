using Common.Stock.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Stock
{
    public class MockStock
    {
        public static List<StockDto> Stocks = new List<StockDto>()
        {
            new StockDto{ ProductId = Guid.Parse("c4a37aaf-b13a-483b-9e0a-c748b0d29bfa"), ProductName ="Product (1)", UnitInStock = 100, UnitPrice = 250 },
            new StockDto{ ProductId = Guid.Parse("48a09e76-743a-4d88-a7cf-9dede4d669af"), ProductName ="Product (2)", UnitInStock = 100, UnitPrice = 310 },
            new StockDto{ ProductId = Guid.Parse("39a2d608-fac4-4b62-986d-ca638b69d257"), ProductName ="Product (3)", UnitInStock = 100, UnitPrice = 49 },
            new StockDto{ ProductId = Guid.Parse("4c571e85-13b7-4c82-aff2-83857cf6cde2"), ProductName ="Product (4)", UnitInStock = 100, UnitPrice = 2568 },
            new StockDto{ ProductId = Guid.Parse("fced8710-1584-4e23-9a11-7156fe7ec6d5"), ProductName ="Product (5)", UnitInStock = 100, UnitPrice = 3650 },
            new StockDto{ ProductId = Guid.Parse("5b3f1211-d349-41cd-9616-4e0bf85f346a"), ProductName ="Product (6)", UnitInStock = 100, UnitPrice = 20500 },
            new StockDto{ ProductId = Guid.Parse("6113f4d1-ab56-4dfa-9191-3535851d8738"), ProductName ="Product (7)", UnitInStock = 100, UnitPrice = 1750 },
            new StockDto{ ProductId = Guid.Parse("7a604eab-9339-4319-ac8c-8aed26c1dc92"), ProductName ="Product (8)", UnitInStock = 100, UnitPrice = 580 },
            new StockDto{ ProductId = Guid.Parse("f411d783-9cee-4301-a24f-8600655c2ecd"), ProductName ="Product (9)", UnitInStock = 100, UnitPrice = 699 },
            new StockDto{ ProductId = Guid.Parse("76db210d-a8b3-4dd7-ad51-d4b518275ba8"), ProductName ="Product (10)", UnitInStock = 100, UnitPrice = 25 },
        };
    }
}
