using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrognardBot2.Commands
{
    public class BasicCommands : ModuleBase<SocketCommandContext>
    {
        [Command("hello")]
        [Summary("Says hello")]
        public async Task HelloCommand()
        {
            await ReplyAsync("Hello");
        }
    }
}
