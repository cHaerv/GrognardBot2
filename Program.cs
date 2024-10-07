using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.IO;
using System.Threading.Tasks;
using GrognardBot2.Handlers;
using GrognardBot2.Commands;
using DotNetEnv;
using GrognardBot2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace GrognardBot2
{
    internal class Program
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services; // Service provider for DI
        private string _rootDirectory;



        static async Task Main(string[] args)
        {
            
            var program = new Program();
            await program.RunBotAsync();
        }
        public async Task RunBotAsync()
        {
            _rootDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            var clientConfig = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.All | GatewayIntents.MessageContent
            };

            // Initialize the DiscordSocketClient with the configured intents
            _client = new DiscordSocketClient(clientConfig);

            _commands = new CommandService();

            // Setup services
            _services = ConfigureServices();

            _client.Log += Log;


            Console.WriteLine("Attempting to resolve CommandHandler...");
            // Register command handler with services
            var commandHandler = _services.GetRequiredService<CommandHandler>();
            Console.WriteLine("CommandHandler resolved.");
            await commandHandler.InstallCommandsAsync();

            
            var envFilePath = Path.Combine(_rootDirectory, ".env");
            Env.Load(envFilePath);
            var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN"); // Load from .env or config
            

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        // This sets up dependency injection
        private IServiceProvider ConfigureServices()
        {
            var databaseFilePath = Path.Combine(_rootDirectory, "game_data.db"); // Ensure this is the correct file
            Console.WriteLine($"Using database file: {databaseFilePath}");
            var services = new ServiceCollection()
                .AddSingleton(_client) // Adds DiscordSocketClient
                .AddSingleton(_commands) // Adds CommandService
                .AddDbContext<GameDbContext>(options =>
                    options.UseSqlite($"Data Source={databaseFilePath}")) // Add DbContext
                .AddSingleton<CommandHandler>(); // Register CommandHandler

            return services.BuildServiceProvider();
        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

    }

}
