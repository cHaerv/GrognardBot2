using Discord.Commands;
using Discord.WebSocket;
using GrognardBot2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GrognardBot2.Handlers
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services)
        {
            _client = client;
            _commands = commands;
            _services = services;
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            // Pass in the service provider to resolve dependencies like DbContext
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: _services);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Cast the message to SocketUserMessage
            var message = messageParam as SocketUserMessage;

            // Check if the message is valid and not from a bot
            if (message == null || message.Author.IsBot) return;
            Console.WriteLine($"Message received: {message.Content}");

            // Create a variable to track where the prefix ends and the command begins
            int argPos = 0;

            Console.WriteLine($"Received message: '{message.Content}' from {message.Author.Username}");
            Console.WriteLine("Checking for command prefix or mention...");
            // Check for command prefix or mention prefix
            if (!(message.HasCharPrefix('!', ref argPos) ||
                  message.HasMentionPrefix(_client.CurrentUser, ref argPos)))
            {
                Console.WriteLine("Command prefix NOT detected.");
                return;
                
            }
                
            Console.WriteLine("Command prefix detected.");

            // Log that a command was received
            Console.WriteLine($"Command received: {message.Content}");

            // Create a command context from the message
            var context = new SocketCommandContext(_client, message);

            // Execute the command
            var result = await _commands.ExecuteAsync(context, argPos, _services);
            // Check if the command execution was unsuccessful
            if (!result.IsSuccess)
            {
                // Log the error message if the command failed
                Console.WriteLine($"Command execution failed: {result.ErrorReason}");
            }
        }
    }
}
