using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GrognardBot2.Models
{
    public class Character
    {
        public int Id { get; set; }         // Primary Key
        public string Name { get; set; }     // Character Name
        public string Race { get; set; }     // Character Race (as string)
        public string Class { get; set; }    // Character Class (as string)

        public int StatsId { get; set; }     // Foreign Key for Stats
        public Stats Stats { get; set; }     // Navigation property for Stats

        public List<InventoryItem> Inventory { get; set; } = new List<InventoryItem>(); // Character's Inventory

        public int PlayerId { get; set; }    // Foreign Key to Player
        public Player Player { get; set; }    // Navigation property for Player
        public int CurrentHP { get; set; }    // Current Hit Points 
    }
}
