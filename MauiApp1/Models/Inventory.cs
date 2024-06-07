using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public class Inventory
    {
        public int InventoryID { get; set; }
        public string InventoryDescription { get; set; } = string.Empty;
        public int Quantity { get; set; }
    
    }
}
