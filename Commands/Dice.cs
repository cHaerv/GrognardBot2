using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace GrognardBot2.Commands
{
    public class Dice : ModuleBase<SocketCommandContext>
    {
        private static readonly ThreadLocal<Random> random = new ThreadLocal<Random>(() => new Random());

        private readonly Dictionary<string, int> _diceSides = new Dictionary<string, int>
        {
            { "d20", 20 },
            { "d6", 6 },
            { "d4", 4 },
            { "d3", 3 }
        };

        [Command("roll")]
        [Summary("Rolls die of specified size and amount. Usage: !roll <sides> <amount> <showAll>")]
        public async Task RollCommand(int sides = 1, int amount = 1, bool showAll = false)
        {
            await ExecuteRoll(amount, sides, showAll);
        }

        [Command("d20")]
        [Summary("Roll D20")]
        public async Task D20Command(int numDice = 1, bool showAll = false) =>
            await ExecuteRoll(numDice, _diceSides["d20"], showAll);

        [Command("d6")]
        [Summary("Roll d6")]
        public async Task D6Command(int numDice = 1, bool showAll = false) =>
            await ExecuteRoll(numDice, _diceSides["d6"], showAll);

        [Command("d4")]
        [Summary("Roll d4")]
        public async Task D4Command(int numDice = 1, bool showAll = false) =>
            await ExecuteRoll(numDice, _diceSides["d4"], showAll);

        [Command("d3")]
        [Summary("Roll d3")]
        public async Task D3Command(int numDice = 1, bool showAll = false) =>
            await ExecuteRoll(numDice, _diceSides["d3"], showAll);

        private async Task ExecuteRoll(int numDice, int sides, bool showAll)
        {
            try
            {
                await RollDice(numDice, sides, showAll);
            }
            catch (Exception ex)
            {
                await ReplyAsync("An error occurred: Make sure sides and number of dice are given as integer values and the flag is either 'false' or 'true'.");
                Console.WriteLine(ex.Message);
            }
        }

        private async Task RollDice(int numDice, int sides, bool showAll)
        {
            StringBuilder result = new StringBuilder();
            string incorrectInput = "";
            int rollSum = 0;

            if (sides > 100)
            {
                sides = 100;
                incorrectInput += "Side limit is 100 :" + Environment.NewLine;
            }
            else if (sides <= 0)
            {
                incorrectInput += "Zero or less sides lower bound is 1" + Environment.NewLine;
            }

            if (numDice <= 0)
            {
                numDice = 1;
                incorrectInput += "Zero or less default is 1 dice :" + Environment.NewLine;
            }
            else if (numDice > 30)
            {
                numDice = 30;
                incorrectInput += $"The Limit is {numDice} dice :" + Environment.NewLine;
            }

            // Roll the dice
            for (int i = 0; i < numDice; i++)
            {
                int roll = random.Value.Next(1, sides + 1); // Use sides + 1 for inclusive range
                rollSum += roll;
                result.Append($"{roll}, ");
            }

            result.Append($"= {rollSum}");

            // Send the response
            await ReplyAsync(showAll ? incorrectInput + result.ToString() : incorrectInput + $"{rollSum}");
        }
    }
}
