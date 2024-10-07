using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrognardBot2.Models
{
    public class Enemy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Stats Stats { get; set; }
        public int MaxHP { get; set; }         // Maximum Hit Points
        public int CurrentHP { get; set; }     // Current Hit Points
        public List<InventoryItem> Loot { get; set; }

        public Enemy()
        {
            Loot = new List<InventoryItem>();
        }

    }
}