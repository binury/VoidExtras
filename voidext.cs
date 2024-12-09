using Cove.Server.Plugins;
using Cove.Server;
using Cove.Server.Actor;
using Steamworks;
using System;
using System.Text.Json;
using System.Runtime.CompilerServices;
/*

    The Void's Extras 1.2, Global Release
    Raviable's Network, 2024


*/

namespace Voidext
{
    public class VoidExt : CovePlugin
    {
        public VoidExt(CoveServer server) : base(server) { }

        public class CommandConf
        {
            // FUCK YEAH! VARIABLES!
            public Boolean Rules { get; set; }
            public Boolean discord { get; set; }
            public Boolean echo { get; set; }
            public Boolean eightball { get; set; }
            public string Rulestext { get; set; }
            public string Discordtext { get; set; }
        }
    
        public override void onInit()
        {
           base.onInit();
            // COMMAND CONFIGURATION SETUP
        
           string fileName = "voidsettings.json";
            string jsonString = File.ReadAllText(fileName);
            CommandConf commandConf = JsonSerializer.Deserialize<CommandConf>(jsonString)!; 
                

            // Basic Commands

            //Rules Command
           RegisterCommand("rules", (player, args) =>
            {
                if (commandConf.Rules==false) return;
                SendPlayerChatMessage(player, commandConf.Rulestext);
            });
            SetCommandDescription("rules", "Shows Rules");

            // Version Info Command
            RegisterCommand("versioninfo", (player, args) =>
            {
                SendPlayerChatMessage(player, $"[VERSION 1.2] Changes Include: Magic 8 Ball Command (!8ball), Echo Command (!echo)");
            });
            SetCommandDescription("versioninfo", "Shows VoTA's Version info, including current server changes");

            // Discord Command
            RegisterCommand("discord", (player, args) =>
            {
                if (commandConf.discord==false) return;
                SendPlayerChatMessage(player, commandConf.Discordtext);
            });
            SetCommandDescription("discord", "Shows the link to the discord");
            Log("We're good..?");

            // The FUN shit starts here

             RegisterCommand("echo", (player, args) =>
            {
               if (commandConf.echo==false) return;
               string msg = string.Join(" ", args);
               SendGlobalChatMessage(msg);
            });
            SetCommandDescription("echo", "Echoes your Message in global chat! Makes for some funny situations");

             RegisterCommand("8ball", (player, args) =>
            {
               if (commandConf.eightball==false) return;
               //Random number Generator
                Random rnd = new Random();
               int randomNum = rnd.Next(1, 20);
                switch (randomNum) 
                {
                case 1:
                   SendGlobalChatMessage($"It is certain");
                    break;
                case 2:
                    SendGlobalChatMessage($"It is decidedly so");
                    break;
                case 3:
                    SendGlobalChatMessage($"Without a doubt");
                    break;
                case 4:
                    SendGlobalChatMessage($"Yes, definitely");
                    break;
                case 5:
                    SendGlobalChatMessage($"You may rely on it");
                    break;
                case 6:
                    SendGlobalChatMessage($"As I see it, yes");
                    break;
                case 7:
                    SendGlobalChatMessage($"Most likely");
                    break;
                case 8:
                    SendGlobalChatMessage($"Outlook good");
                    break;
                case 9:
                    SendGlobalChatMessage($"Yes");
                    break;
                case 10:
                    SendGlobalChatMessage($"Signs point to yes");
                    break;
                case 11:
                   SendGlobalChatMessage($"Reply hazy, try again");
                    break;
                case 12:
                    SendGlobalChatMessage($"Ask again later");
                    break;
                case 13:
                    SendGlobalChatMessage($"Better not tell you now");
                    break;
                case 14:
                    SendGlobalChatMessage($"Cannot predict now");
                    break;
                case 15:
                    SendGlobalChatMessage($"Concentrate and ask again");
                    break;
                case 16:
                    SendGlobalChatMessage($"Don't count on it");
                    break;
                case 17:
                    SendGlobalChatMessage($"My reply is no");
                    break;
                case 18:
                    SendGlobalChatMessage($"My sources say no");
                    break;
                case 19:
                    SendGlobalChatMessage($"Outlook not so good");
                    break;
                case 20:
                    SendGlobalChatMessage($"Very doubtful");
                    break;
                }
            });
            SetCommandDescription("8ball", "Shake the 8 Ball, get it's Opinion!");

        }

    }
}

