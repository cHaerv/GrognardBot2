using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrognardBot2.Models
{
    public class InventoryItem
    {
        public int Id { get; set; } // Primary key
        public string Name { get; set; } // Item Name
        public string Description { get; set; } // Item description
        public int Quantity { get; set; } // Quantity of this item
        public int CharacterId { get; set; } //Foreign key to character

        // Navigation property
        public Character Character { get; set; }

    }
}
