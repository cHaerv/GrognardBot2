using Discord;
using Discord.Commands;
using GrognardBot2.Data;
using GrognardBot2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrognardBot2.Commands
{
    public class CharacterCommands : ModuleBase<SocketCommandContext>
    {

        private readonly DbContextOptions<GameDbContext> _dbContextOptions;

        public CharacterCommands(DbContextOptions<GameDbContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        private Embed BuildCharacterEmbed(Character character)
        {
            var embedBuilder = new EmbedBuilder()
                .WithTitle($"{character.Name}")
                .WithColor(Color.DarkGreen) // Set a color for the embed
                .AddField("Class", character.Class, true)
                .AddField("Race", character.Race, true)
                .AddField("HP", $"{character.CurrentHP} / {character.Stats.MaxHp}", true)
                .AddField("Strength", character.Stats.Strength, true)
                .AddField("Dexterity", character.Stats.Dexterity, true)
                .AddField("Constitution", character.Stats.Constitution, true)
                .AddField("Intelligence", character.Stats.Intelligence, true)
                .AddField("Wisdom", character.Stats.Wisdom, true)
                .AddField("Charisma", character.Stats.Charisma, true)
                .WithFooter("Character data"); // Add a footer if needed

            return embedBuilder.Build();
            
        }

        [Command("register")]
        public async Task RegisterCommand()
        {

            // Check if the player is already registered
            using (var db = new GameDbContext(_dbContextOptions))
            {
                var existingPlayer = await db.Players.FirstOrDefaultAsync(p => p.DiscordId == Context.User.Id.ToString());

                if (existingPlayer != null)
                {
                    await ReplyAsync($"You are already registered as **{existingPlayer.Username}**.");
                    return;
                }

                // Create a new Player instance
                Player newPlayer = new Player
                {
                    DiscordId = Context.User.Id.ToString(),
                    Username = Context.User.Username,
                    Characters = new List<Character>() // Initialize the character list
                };

                // Save the new player to the database
                db.Players.Add(newPlayer);
                await db.SaveChangesAsync();

                await ReplyAsync($"You have been registered as **{newPlayer.Username}**!");
            }
        }


        [Command("createcharacter")]
        public async Task CreateCharachterCommand(string name, string characterClass, string race)
        {
            using (var db = new GameDbContext(_dbContextOptions))
            {
                // Retrieve or create the Player based on DiscordId
                var player = await db.Players.FirstOrDefaultAsync(p => p.DiscordId == Context.User.Id.ToString());

                if (player == null)
                {
                    player = new Player
                    {
                        DiscordId = Context.User.Id.ToString(),
                        Username = Context.User.Username
                    };
                    db.Players.Add(player);
                    await db.SaveChangesAsync(); // Save the new player to the database
                    Console.WriteLine($"Player ID: {player.Id}");
                }
                Console.WriteLine($"Player ID: {player.Id}");

                // Create default stats
                Stats stats = new Stats
                {
                    Strength = 10,
                    Dexterity = 10,
                    Constitution = 10,
                    Intelligence = 10,
                    Wisdom = 10,
                    Charisma = 10,
                    MaxHp = 20,
                };
                db.Stats.Add(stats);
                await db.SaveChangesAsync();

                // Create the new character
                Character newCharacter = new Character
                {
                    Name = name,
                    Class = characterClass,
                    Race = race,
                    CurrentHP = stats.MaxHp,
                    PlayerId = player.Id, // Associate the character with the player
                    StatsId = stats.Id
                };


                try
                {
                    // Save the newCharacter to your database here
                    db.Characters.Add(newCharacter);
                    await db.SaveChangesAsync();
                    await ReplyAsync($"Character created: **{newCharacter.Name}**, Class: **{newCharacter.Class}**, HP: **{newCharacter.CurrentHP}**");
                }
                catch (DbUpdateException ex)
                {
                    var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    Console.WriteLine($"Command execution failed: {errorMessage}");
                    await ReplyAsync("An error occurred while creating the character. Please try again.");
                }



            }
        }

        [Command("showcharacter")]
        public async Task ShowCharacterCommand(string characterName = null)
        {
            using (var db = new GameDbContext(_dbContextOptions))
            {
                // Print the path of the database
                var databaseFilePath = Path.Combine(AppContext.BaseDirectory, "game_data.db");
                Console.WriteLine($"Using database file: `{databaseFilePath}`");


                var player = await db.Players
                    .Include(p => p.Characters)
                    .ThenInclude(c => c.Stats)
                    .FirstOrDefaultAsync(p => p.DiscordId == Context.User.Id.ToString());

                if (player == null || !player.Characters.Any())
                {
                    await ReplyAsync("You don't have any charachters yet. Use the `createcharacter` command to create one.");
                    return;
                }

                Character character;

                if (string.IsNullOrEmpty(characterName))
                {
                    character = player.Characters.FirstOrDefault();
                }
                else
                {
                    character = player.Characters.FirstOrDefault(c => c.Name.Equals(characterName, StringComparison.OrdinalIgnoreCase));
                }



                if (character != null)
                { 

                    var embed = BuildCharacterEmbed(character);

                    await ReplyAsync(embed: embed);
                }
                else
                {
                    await ReplyAsync("No characters found for this player.");
                }
            }
        }


        [Command("listcharacters")]
        public async Task ListCharactersCommand()
        {
            using (var db = new GameDbContext(_dbContextOptions))
            {
                var player = await db.Players.Include(p => p.Characters)
                    .FirstOrDefaultAsync(p => p.DiscordId == Context.User.Id.ToString());

                if (player == null || !player.Characters.Any())
                {
                    await ReplyAsync("You have no characters registered.");
                    return;
                }

                StringBuilder characterList = new StringBuilder();

                foreach(var character in player.Characters) 
                {
                    characterList.Append($"Name: **{character.Name}**, Class: **{character.Class}**, Race: **{character.Race}**\n");
                }


                await ReplyAsync(characterList.ToString());
            }
        }

        private async Task StatAllocationAsync(Player player)
        {
            // set the base stats and the ammount of points needed.
            int baseStatvalue = 8;
            int maxStatValue = 16;
            int pointsToAllocate = 10;



        }


    }
}
