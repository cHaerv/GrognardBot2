using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrognardBot2.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string DiscordId { get; set; }
        public string Username { get; set; }

        public List<Character> Characters { get; set; }
    }
}
