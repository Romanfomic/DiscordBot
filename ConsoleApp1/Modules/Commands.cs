using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using Discord.WebSocket;
using ConsoleApp1.Common;
using System.IO;
using Newtonsoft.Json;

namespace ConsoleApp1.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {

        [Command("Поздравляю, ты апнулся до уровня")]
        public async Task sayAsync(int lvl, SocketGuildUser user)
        {
            if(lvl % 5 == 0)
            {
                var user1 = Context.User as SocketGuildUser;
                var owner = Context.Guild.GetRole(748605009469505696);
                var bot = Context.Guild.GetRole(702513305197215795);
                if (user1.Roles.Contains(owner) || user1.Roles.Contains(bot))
                {
                    var role5 = Context.Guild.GetRole(730022949695782952);
                    var role10 = Context.Guild.GetRole(730023074346303730);
                    var role15 = Context.Guild.GetRole(730023153375641651);
                    var role20 = Context.Guild.GetRole(748957689085821030);
                    var role25 = Context.Guild.GetRole(748957856761380890);
                    var role30 = Context.Guild.GetRole(748957922016362587);
                    var role35 = Context.Guild.GetRole(748958012009611265);
                    var role40 = Context.Guild.GetRole(748958107304198155);
                    switch (lvl)
                    {
                        case 5:
                            await user.AddRoleAsync(role5);
                            break;
                        case 10:
                            await user.AddRoleAsync(role10);
                            await user.RemoveRoleAsync(role5);
                            break;
                        case 15:
                            await user.AddRoleAsync(role15);
                            await user.RemoveRoleAsync(role10);
                            break;
                        case 20:
                            await user.AddRoleAsync(role20);
                            await user.RemoveRoleAsync(role15);
                            break;
                        case 25:
                            await user.AddRoleAsync(role25);
                            await user.RemoveRoleAsync(role20);
                            break;
                        case 30:
                            await user.AddRoleAsync(role30);
                            await user.RemoveRoleAsync(role25);
                            break;
                        case 35:
                            await user.AddRoleAsync(role35);
                            await user.RemoveRoleAsync(role30);
                            break;
                        case 40:
                            await user.AddRoleAsync(role40);
                            await user.RemoveRoleAsync(role35);
                            break;
                    }
                }
                else Context.Channel.SendMessageAsync("У Вас недостаточно прав для этой команды.");
            }
        }
           

        [Command("c")]
        public async Task Purge(int amount)
        {
            var client = Context.Client;

            var user1 = Context.User as SocketGuildUser;
            var owner = Context.Guild.GetRole(748605009469505696);
            var work = Context.Guild.GetRole(828600827995357253); //модератор
            var leader = Context.Guild.GetRole(828600831020499004); //Руководство
            if (user1.Roles.Contains(owner) || user1.Roles.Contains(work) || user1.Roles.Contains(leader))
            {
                if (amount <= 1)
                {
                    await ReplyAsync("Количество сообщений должно быть больше 1!");
                    return;
                }

                var messages = await Context.Channel.GetMessagesAsync(Context.Message, Direction.Before, amount).FlattenAsync();

                var filteredMessages = messages.Where(x => (DateTimeOffset.UtcNow - x.Timestamp).TotalDays <= 14);

                var count = filteredMessages.Count();

                if (count == 0)
                    await ReplyAsync("Сообщений не обнаружено.");

                else
                {
                    await (Context.Channel as ITextChannel).DeleteMessagesAsync(filteredMessages);
                    string title = "Clean";
                    string description = $"Пользователь: {user1.Mention}({user1.Id})\nТекстовый канал: <#{Context.Channel.Id}>\nКоличество сообщений: {amount}";
                    Color color = Color.DarkerGrey;
                    Logs(title, description, color);
                    var m = await ReplyAsync($"Операция выполнена. Были удалены {count} {((count % 10 == 2) || (count % 10 == 3) || (count % 10 == 4) ? "сообщения" : "сообщений")}.");
                    await Task.Delay(5000);
                    Context.Channel.DeleteMessageAsync(m);
                }
                /*
                if (user1.Roles.Contains(owner) || user1.Roles.Contains(work) || user1.Roles.Contains(leader))
                    client.GetGuild(702498967199678554).GetTextChannel(764005576521809931).SendMessageAsync("Пользователь " + Context.Message.Author.Mention + " использовал команду `clean` и очистил " + $"{ count}{ ((count % 10 == 2) || (count % 10 == 3) || (count % 10 == 4) ? " сообщения" : " сообщений")}.");
                */
            }
            else
            {
                var m = await ReplyAsync("У Вас недостаточно прав для использования этой команды!");
                await Task.Delay(10000);
                Context.Channel.DeleteMessageAsync(m);
            }
            Context.Message.DeleteAsync();
        }

        [Command("add")]
        public async Task Add(SocketGuildUser user, string post)
        {
            var client = Context.Client;
            EmbedBuilder builder = new EmbedBuilder();

            var user1 = Context.User as SocketGuildUser;
            var role1 = Context.Guild.GetRole(748605009469505696); //Основатель
            var role2 = Context.Guild.GetRole(828600831020499004); //Главный админ
            if (user1.Roles.Contains(role1) || user1.Roles.Contains(role2))
            {

                builder.AddField("Руководитель :", $"{user1.Mention}");
                builder.AddField("Администратор :", $"{user.Mention}");

                var work = Context.Guild.GetRole(828600827995357253); //модератор
                var isp = Context.Guild.GetRole(748630145497170010);
                var main = Context.Guild.GetRole(828600831020499004); //Главный админ
                var show = Context.Guild.GetRole(749378955038949447); //Представитель
                var role = "";
                Color color = new Color();

                bool a = false;
                switch (post)
                        {
                        case "isp":          //Испытательный срок(модер)
                            await user.AddRoleAsync(work);
                            await user.AddRoleAsync(isp);
                            builder.AddField("Должность :", "Модератор(Испытательный срок)");
                            color = isp.Color;
                            role = "Модератор(испытательный срок)";
                            a = true;
                        break;
                        case "mod":          //Модер
                            await user.AddRoleAsync(work);
                            builder.AddField("Должность :", "Модератор");
                            role = "Модератор";
                            a = true;
                        break;
                        case "pred":          //Представители
                            await user.AddRoleAsync(work);
                            await user.AddRoleAsync(show);
                            builder.AddField("Должность :", "Представители");
                            color = show.Color;
                            role = "Представитель";
                            a = true;
                        break;
                        case "main":
                            if (!user1.Roles.Contains(role2))
                            {
                                await user.AddRoleAsync(work);
                                await user.AddRoleAsync(main);
                                builder.AddField("Должность :", "Главный администратор");
                                color = main.Color;
                                role = "Главный администратор";
                                a = true;
                            }
                        break;
                        }
                if(a == true)
                {
                    builder.Color = color;
                    client.GetGuild(702498967199678554).GetTextChannel(748600569429491742).SendMessageAsync("", false, builder.Build());
                    string title = "Add Role";
                    string description = $"Администратор: {user1.Mention}({user1.Id})\nСотрудник: {user.Mention}\nРоль: `{role}`";
                    Logs(title, description, Color.Orange);
                    //client.GetGuild(702498967199678554).GetTextChannel(764005576521809931).SendMessageAsync($"Администратор {user1.Mention} выдал роль `{role}` пользователю {user.Mention}");
                }
            }
            else Context.Channel.SendMessageAsync("У Вас недостаточно прав для использования этой команды");
            Context.Message.DeleteAsync();
        }

        [Command("rem")]
        public async Task Remove(SocketGuildUser user, [Remainder]string reason)
        {
            var client = Context.Client;
            EmbedBuilder builder = new EmbedBuilder();

            var user1 = Context.User as SocketGuildUser;
            var owner = Context.Guild.GetRole(748605009469505696);
            var role2 = Context.Guild.GetRole(828600831020499004); //Руководитель
            if (user1.Roles.Contains(owner) || user1.Roles.Contains(role2))
            {
                var work = Context.Guild.GetRole(828600827995357253); //Модератор 
                var isp = Context.Guild.GetRole(748630145497170010);
                var main = Context.Guild.GetRole(828600831020499004); //Главный админ
                var show = Context.Guild.GetRole(749378955038949447); //Представитель

                await user.RemoveRoleAsync(work);
                await user.RemoveRoleAsync(isp);
                await user.RemoveRoleAsync(main);
                await user.RemoveRoleAsync(show);
                builder.AddField("Руководитель :", $"{user1.Mention}");
                builder.AddField("Администратор :", $"{user.Mention}");
                builder.AddField("Причина ", $"{reason}", true);
                builder.Color = Color.DarkRed;
                client.GetGuild(702498967199678554).GetTextChannel(748600975324872714).SendMessageAsync("", false, builder.Build());

                string title = "Remove Role";
                string description = $"Администратор: {user1.Mention}({user1.Id})\nСотрудник: {user.Mention}";
                Logs(title, description, Color.DarkRed);
                //client.GetGuild(702498967199678554).GetTextChannel(764005576521809931).SendMessageAsync($"Администратор {user1.Mention} убрал роли сотрудника пользователю {user.Mention}");
            }
            else Context.Channel.SendMessageAsync("У Вас недостаточно прав для использования этой команды");
            Context.Message.DeleteAsync();
        }

        /*
        [Command("cha")]
        public async Task Change(string user, string post)
        {
            var _user = Context.Guild.GetUser(Context.Message.MentionedUsers.First().Id);
            var client = Context.Client;
            EmbedBuilder builder = new EmbedBuilder();

            var user1 = Context.User as SocketGuildUser;
            var owner = Context.Guild.GetRole(748605009469505696); //Основатель
            var role2 = Context.Guild.GetRole(828600831020499004); //Руководитель
            if (user1.Roles.Contains(owner) || user1.Roles.Contains(role2))
            {
                builder.AddField("Руководитель :", $"{user1.Mention}");
                builder.AddField("Администратор :", $"{_user.Mention}");

                var work = Context.Guild.GetRole(828600827995357253); //Модератор
                var isp = Context.Guild.GetRole(748630145497170010);
                var show = Context.Guild.GetRole(749378955038949447); //Представитель
                var leader = Context.Guild.GetRole(828600831020499004); //Руководство

                var role = "";
                if (_user.Roles.Contains(mod) && _user.Roles.Contains(isp)) { role = "Модератор(Испытательный срок)"; }
                if (_user.Roles.Contains(mod) && !_user.Roles.Contains(isp)) { role = "Модератор";  }
                if (_user.Roles.Contains(adm)) { role = "Администратор";  }
                if (_user.Roles.Contains(main)) { role = "Главный администратор";  }

                await _user.RemoveRoleAsync(isp);
                await _user.RemoveRoleAsync(mod);
                await _user.RemoveRoleAsync(adm);
                await _user.RemoveRoleAsync(main);
                await _user.RemoveRoleAsync(show);

                var newrole = "";
                switch (post)
                {
                    case "mod":          //Модер
                        await _user.AddRoleAsync(mod);
                        builder.AddField("Новая должность: ", $"{role} => Модератор");
                        builder.Color = mod.Color;
                        newrole = "Модератор";
                        break;
                    case "adm":          //Админ
                        await _user.AddRoleAsync(adm);
                        builder.AddField("Новая должность: ", $"{role} => Администратор");
                        builder.Color = adm.Color;
                        newrole = "Администратор";
                        break;
                    case "pred":          //Представители
                        await _user.AddRoleAsync(show);
                        builder.AddField("Новая должность: ", $"{role} => Представители");
                        builder.Color = show.Color;
                        newrole = "Представитель";
                        break;
                    case "main":
                        await _user.AddRoleAsync(main);
                        builder.AddField("Новая должность: ", $"{role} => Главный администратор");
                        builder.Color = main.Color;
                        newrole = "Главный администратор";
                        break;
                }
                if (post == "mod" || post == "adm" || post == "pred" || post == "main")
                {
                    client.GetGuild(702498967199678554).GetTextChannel(748601450204102717).SendMessageAsync("", false, builder.Build());
                    client.GetGuild(702498967199678554).GetTextChannel(764005576521809931).SendMessageAsync($"Администратор {user1.Mention} поменял роль `{role}` на `{newrole}` пользователю {_user.Mention}");
                }
            }
            else Context.Channel.SendMessageAsync("У Вас недостаточно прав для использования этой команды");
            Context.Message.DeleteAsync();
        }
        */

        [Command("idea")]
        public async Task test(SocketGuildUser user, [Remainder] string y)
        {
            var user1 = Context.User as SocketGuildUser;
            var owner = Context.Guild.GetRole(748605009469505696); //Основатель
            var leader = Context.Guild.GetRole(828600831020499004); //Руководство
            if (user1.Roles.Contains(owner) || user1.Roles.Contains(leader))
            {
                    Context.Channel.SendMessageAsync($"Автор: {user.Mention}");
                    var message = await ReplyAsync(y);
                    var emotes = new[]
                    {
                    new Emoji("👍"),
                    new Emoji("👎")
                    };
                    await message.AddReactionsAsync(emotes);
            }
            Context.Message.DeleteAsync();
        }

        [Command("achive")]
        public async Task Achivement(SocketGuildUser user, [Remainder] string a)
        {
            var user1 = Context.User as SocketGuildUser;
            var owner = Context.Guild.GetRole(748605009469505696); //создатель
            var work = Context.Guild.GetRole(828600827995357253); //Модератор
            var show = Context.Guild.GetRole(749378955038949447); //Представитель
            var leader = Context.Guild.GetRole(828600831020499004); //Руководство

            if (user1.Roles.Contains(owner) || user1.Roles.Contains(work) || user1.Roles.Contains(show) || user1.Roles.Contains(leader))
            {
                var client = Context.Client;
                EmbedBuilder builder = new EmbedBuilder();
                builder.AddField("Пользователь :", $"{user.Mention}");
                builder.AddField("Новое достижение: ", $"{a}");
                builder.Color = Color.Green;
                Context.Channel.SendMessageAsync("", false, builder.Build());
                Context.Message.DeleteAsync();
            }
        }
        
        [Command("temp")]
        public async Task Mute(string name, SocketGuildUser user, int minutes, [Remainder]string reason = null)
        {
            var client = Context.Client;
            var user1 = Context.User as SocketGuildUser;
            string timeStr = "минут";
            int timeEnd = minutes % 10;
            string punish = "";

            EmbedBuilder builder = new EmbedBuilder();

            var owner = Context.Guild.GetRole(748605009469505696); //создатель
            var work = Context.Guild.GetRole(828600827995357253); //сотрудники
            var leader = Context.Guild.GetRole(828600831020499004); //Руководство
            var mute = Context.Guild.GetRole(733326246704054302); //мут
            var ban = Context.Guild.GetRole(837638043975942164); //бан
            if (user1.Roles.Contains(owner) || user1.Roles.Contains(work) || user1.Roles.Contains(leader))
            {
                if (timeEnd == 1) timeStr = "минута";
                if ((timeEnd >= 2) && (timeEnd <= 4)) timeStr = "минуты";
                if (user.Roles.Contains(mute) || user.Roles.Contains(ban))
                {
                    var m = await Context.Channel.SendMessageAsync("Пользователь уже заблокирован");
                    await Task.Delay(3000);
                    Context.Channel.DeleteMessageAsync(m);
                    return;
                }
                switch(name)
                {
                    case "mute":
                        await user.AddRoleAsync(mute);
                        punish = "Мут";
                        builder.Color = Color.DarkGrey;
                        break;
                    case "ban":
                        await user.AddRoleAsync(ban);
                        punish = "Бан";
                        builder.Color = Color.DarkOrange;
                        break;
                }

                string jsonData = File.ReadAllText("DataFolder\\Mutes.json");
                List<Mute> Mutes = new List<Mute>();
                if (jsonData != "")
                    Mutes = JsonConvert.DeserializeObject<List<Mute>>(jsonData);

                Mutes.Add(new Mute { UserId = user.Id, End = DateTime.Now + TimeSpan.FromMinutes(minutes)});
                jsonData = JsonConvert.SerializeObject(Mutes);
                File.WriteAllText("DataFolder\\Mutes.json", jsonData);

                //Program.Mutes.Add(new Mute { Guild = Context.Guild, User = user, End = DateTime.Now + TimeSpan.FromMinutes(minutes), Role = mute });
                builder.AddField($"{punish}", $"Администратор : {user1.Mention}");
                builder.AddField($"{minutes} {timeStr}", $"Пользователь : {user.Mention}");
                builder.AddField("Причина :", $"{reason ?? "Отсутсвует"}");
                client.GetGuild(702498967199678554).GetTextChannel(748594094980464679).SendMessageAsync("", false, builder.Build());
                client.GetGuild(702498967199678554).GetTextChannel(764005576521809931).SendMessageAsync("Пользователь " + Context.Message.Author.Mention + $" использовал команду `temp {name}` \nПользователь { user.Mention} \nCрок - `{minutes}{timeStr}` \nПричина - `{reason ?? "отсутствует"}`.");
                Context.Message.DeleteAsync(); 
            }
        }

        [Command("help")]
        public async Task Help()
        {
            var user1 = Context.User as SocketGuildUser;
            EmbedBuilder builder = new EmbedBuilder();

            var owner = Context.Guild.GetRole(748605009469505696); //создатель
            var work = Context.Guild.GetRole(828600827995357253); //Модератор
            var leader = Context.Guild.GetRole(828600831020499004); //Руководство
            if (user1.Roles.Contains(work))
            {
                builder.AddField("Команды для администрации", "Очистка нескольких сообщений `+c n` \nВыдача мута пользователю `+temp mute @user time reason` \nВыдача временнного бана пользователю `+temp ban @user time reason`");
                builder.Color = Color.DarkBlue;
            }
            else
            {
                if (user1.Roles.Contains(leader) || user1.Roles.Contains(owner))
                {
                    builder.AddField("Команды для всей администрации", "Очистка нескольких сообщений `+c n` \nВыдача мута пользователю `+temp mute @user time reason` \nВыдача временнного бана пользователю `+temp ban @user time reason`");
                    builder.AddField("Команды для высшего руководства", "Добавление сотрудников `+add @user post` \nИзменение должности `+cha @user work` \nУвольнение сотрудника `+rem @user reason`");
                }
                else   
                {
                    builder.AddField("Бот создан для помощи администрации", "Его команды Вам недоступны.");
                }
            }
            Context.Message.DeleteAsync();
            var m = await Context.Channel.SendMessageAsync("", false, builder.Build());
            await Task.Delay(10000);
            Context.Channel.DeleteMessageAsync(m);
        }

        [Command("emoji")]
        public async Task Emoji(string name = null)
        {
            var user = Context.User as SocketGuildUser;
            var founder = Context.Guild.GetRole(748605009469505696);

            if (user.Roles.Contains(founder))
            {
                Emote emoji = Emote.Parse("<a:a_aniflex:780049699138437121>");

                switch (name)
                {
                    case "zdarova":
                        emoji = Emote.Parse("<a:a_zdarova:780049699138437121>");
                        break;
                    case "suicide":
                        emoji = Emote.Parse("<a:a_suicide:774004846998061106>");
                        break;
                    case "linux":
                        emoji = Emote.Parse("<a:a_chistim_linux:780051935016910868>");
                        break;
                    case "flex":
                        emoji = Emote.Parse("<a:a_aniflex:773993045691203614>");
                        break;
                    case "aga":
                        emoji = Emote.Parse("<a:a_aga:780050519426859018>");
                        break;
                    case "ban":
                        emoji = Emote.Parse("<a:a_ebanutiy:773996779729059851>");
                        break;
                    case "love":
                        emoji = Emote.Parse("<a:lybly:773996178274648084>");
                        break;
                    case "bob":
                        emoji = Emote.Parse("<a:glazhu_boba:773992929433878549>");
                        break;
                }

                var m = await Context.Channel.SendMessageAsync($"{emoji}");
            }
            Context.Message.DeleteAsync();
        }

        [Command("emhelp")]
        public async Task EmojiHelp()
        {
            var client = Context.Client;
            var user = Context.User as SocketGuildUser;
            var founder = Context.Guild.GetRole(748605009469505696);
            EmbedBuilder builder = new EmbedBuilder();
            if (user.Roles.Contains(founder))
            {
                Emote zdarova = Emote.Parse("<a:a_aniflex:780049699138437121>");
                Emote suicide = Emote.Parse("<a:a_suicide:774004846998061106>");
                Emote linux = Emote.Parse("<a:a_chistim_linux:780051935016910868>");
                Emote flex = Emote.Parse("<a:a_aniflex:773993045691203614>");
                Emote aga = Emote.Parse("<a:a_aga:780050519426859018>");
                Emote ban = Emote.Parse("<a:a_ebanutiy:773996779729059851>");
                Emote love = Emote.Parse("<a:lybly:773996178274648084>");
                Emote bob = Emote.Parse("<a:glazhu_boba:773992929433878549>");
                builder.AddField("Команда : `+emoji name` ", $"zdarova - {zdarova} \nsuicide - {suicide} \nlinux - {linux} \nflex - {flex} \naga - {aga} \nban - {ban} \nlove - {love} \nbob - {bob}");
                var m = await Context.Channel.SendMessageAsync("", false, builder.Build());
                Context.Message.DeleteAsync();
                await Task.Delay(5 * 1000);
                await Context.Channel.DeleteMessageAsync(m);
            }
        }

        public async Task Logs(string title, string description, Color? color = null)
        {
            var client = Context.Client;
            EmbedBuilder builder = new EmbedBuilder();
            builder.Title = title;
            builder.Description = description;
            builder.Color = color;
            await client.GetGuild(702498967199678554).GetTextChannel(764005576521809931).SendMessageAsync("", false, builder.Build());
        }
    }
}
