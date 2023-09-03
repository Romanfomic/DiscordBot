using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ConsoleApp1.Common;
using System.IO;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            string token = "";

            _client.Log += _client_Log;

            await RegisterCommansAssync();

            await _client.LoginAsync(TokenType.Bot, token); 

            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommansAssync()
        {
            _client.MessageReceived += HandleCommandAssync;
            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), _services);
        }

        private async Task HandleCommandAssync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            //if (message.Author.IsBot) return;

            int argPos = 0;
            if (message.HasStringPrefix("+", ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
            }

            MuteHandler();
        }

        private async Task MuteHandler()
        {
            List<Mute> Remove = new List<Mute>();
            string jsonData = File.ReadAllText("DataFolder\\Mutes.json");
            List<Mute> Mutes = new List<Mute>();
            if (jsonData != "")
                Mutes = JsonConvert.DeserializeObject<List<Mute>>(jsonData);
            foreach (var mute in Mutes)
            {
                if (DateTime.Now >= mute.End)
                {
                    IGuild guild = _client.GetGuild(702498967199678554);
                    IGuildUser[] users = guild.GetUsersAsync().Result.ToArray();
                    foreach (var user in users)
                    {
                        if (mute.UserId == user.Id)
                        {
                            var muteRole = guild.GetRole(mute.RoleMute);
                            await user.RemoveRoleAsync(muteRole);
                            Remove.Add(mute);
                            Mutes = Mutes.Except(Remove).ToList();
                            break;
                        }
                    }
                }
            }
            jsonData = JsonConvert.SerializeObject(Mutes);
            File.WriteAllText("DataFolder\\Mutes.json", jsonData);

            await Task.Delay(1 * 60 * 1000);
            await MuteHandler();
        }
    }
}
