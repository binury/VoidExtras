using Cove.Server.Plugins;
using Cove.Server;
using Cove.Server.Actor;
using Steamworks;
using System;
using System.Text.Json;
using System.Runtime.CompilerServices;
using System.Xml;
/*

    The Void's Extras 1.3, Global Release
    Raviable's Network, 2024

*/

namespace Voidext
{
    public class VoidExt : CovePlugin
    {
        public VoidExt(CoveServer server) : base(server) { }

        public class CommandConf
        {
            // Command Configuration Variables
            public Boolean rules { get; set; }
            public Boolean discord { get; set; }
            public Boolean echo { get; set; }
            public Boolean eightball { get; set; }
            public string rulestext { get; set; }
            public string discordtext { get; set; }
            // Filter Configuration Variables
            public string[] flaggedwords { get; set; }
            public string filteraction { get; set; }
            public string actionmsg { get; set; }
            public Boolean ChatFilterToggle { get; set; }
            public Boolean chatgpt { get; set; }
            public string api_key { get; set; }
        }

        public override void onChatMessage(WFPlayer sender, string message)
        {
            
            base.onChatMessage(sender, message);
            // reading dat json tho
            string fileName = "voidsettings.json";
            string filterLog = "voidfilterlog.txt";
            string jsonString = File.ReadAllText(fileName);
            string filterString = File.ReadAllText(filterLog);
            CommandConf commandConf = System.Text.Json.JsonSerializer.Deserialize<CommandConf>(jsonString)!;

            string[] shitlist = commandConf.flaggedwords;
            // detection

            // if a chat message contains a word that the shitlist has, do the action
            foreach (string w in shitlist)
            {
                if (commandConf.ChatFilterToggle==false) return;
                if (message.Contains(w))
                {
                    switch(commandConf.filteraction)
                    {
                        case "log":
                            using (StreamWriter wr = File.AppendText(filterLog))
                            {
                                wr.WriteLine("----------------------------------------" + Environment.NewLine + "Player ID: " + sender.SteamId + 
                                Environment.NewLine + "Player Name: " + sender.Username + Environment.NewLine + "Player Message: " + message + 
                                Environment.NewLine + "Time: " + System.DateTime.Now + Environment.NewLine + "Action Taken: " + commandConf.filteraction + Environment.NewLine + "----------------------------------------");
                            }
                            break;
                        case "logwarn":
                            SendPlayerChatMessage(sender, commandConf.actionmsg);
                            using (StreamWriter wr = File.AppendText(filterLog))
                            {
                                wr.WriteLine("----------------------------------------" + Environment.NewLine + "Player ID: " + sender.SteamId + 
                                Environment.NewLine + "Player Name: " + sender.Username + Environment.NewLine + "Player Message: " + message + 
                                Environment.NewLine + "Time: " + System.DateTime.Now + Environment.NewLine + "Action Taken: " + commandConf.filteraction + Environment.NewLine + "----------------------------------------");
                            }
                            break;
                        case "kick":
                            using (StreamWriter wr = File.AppendText(filterLog))
                            {
                                wr.WriteLine("----------------------------------------" + Environment.NewLine + "Player ID: " + sender.SteamId + 
                                Environment.NewLine + "Player Name: " + sender.Username + Environment.NewLine + "Player Message: " + message + 
                                Environment.NewLine + "Time: " + System.DateTime.Now + Environment.NewLine + "Action Taken: " + commandConf.filteraction + Environment.NewLine + "----------------------------------------");
                            }
                            KickPlayer(sender);
                            break;
                        case "ban":
                            using (StreamWriter wr = File.AppendText(filterLog))
                            {
                                wr.WriteLine("----------------------------------------" + Environment.NewLine + "Player ID: " + sender.SteamId + 
                                Environment.NewLine + "Player Name: " + sender.Username + Environment.NewLine + "Player Message: " + message + 
                                Environment.NewLine + "Time: " + System.DateTime.Now + Environment.NewLine + "Action Taken: " + commandConf.filteraction + Environment.NewLine + "----------------------------------------");
                            }
                            BanPlayer(sender);

                            break;
                    }
                }
            }

        }
        
        
        
        // Commands function
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
                if (commandConf.rules==false) return;
                SendPlayerChatMessage(player, commandConf.rulestext);
            });
            SetCommandDescription("rules", "Shows Rules");

            // Version Info Command
            RegisterCommand("versioninfo", (player, args) =>
            {
                SendPlayerChatMessage(player, $"[VERSION 1.3] Changes Include: New Chat Filter, ChatGPT Command");
            });
            SetCommandDescription("versioninfo", "Shows Void's Extras' Version info, including current server changes");

            // Discord Command
            RegisterCommand("discord", (player, args) =>
            {
                if (commandConf.discord==false) return;
                SendPlayerChatMessage(player, commandConf.discordtext);
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
              //Xe wanted to steal the 8ball idea so I copied his code redo :)
              Random rand = new Random();
              string[] ResponseText = {"It is certain", "It is decidedly so", "Without a doubt", "Yes, definitely", "You may rely on it", "As I see it, yes", "Most likely", 
                                    "Outlook good", "Yes", "Signs point to yes", "Reply hazy, try again", "Ask again later", "Better not tell you now", "Cannot predict now",
                                    "Concentrate and ask again", "Don't count on it", "My reply is no", "My sources say no", "Outlook not so good", "Very doubtful"};
              string respmsg = ResponseText[rand.Next(ResponseText.Length)];
              SendGlobalChatMessage(respmsg);
            });
            SetCommandDescription("8ball", "Shake the 8 Ball, get it's Opinion!");
        /*          # Not Yet... #
             RegisterCommand("chatgpt", (player, args) =>
            {
              if (commandConf.chatgpt==false) return;
              
              string query = string.Join(" ", args);
              string key = commandConf.api_key;
              
              
            });
            SetCommandDescription("chatgpt", "Full-Blown ChatGPT implementation... Do you think I'm kidding brah? Try it!");
        */
        }

    }

    
}

