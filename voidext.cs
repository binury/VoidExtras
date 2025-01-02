using Cove.Server.Plugins;
using Cove.Server;
using Cove.Server.Actor;
using Steamworks;
using System;
using System.Text.Json;
using System.Runtime.CompilerServices;
using System.ClientModel;
using OpenAI.Chat;
/*

    The Void's Extras 1.3, Global Release
    Raviable's Network, 2024

    Credits:
    - Initial ideas and planning done by yours truly.
    - most commands are also done by yours truly
    - 8ball improvements suggested by Xe
    - ChatGPT heavily researched and implemented in by me using the OpenAI API, Some suggestions for Fixes came from Sera
    - Various other unlisted sources were adapted and used for the word detector, but not directly copied, moreso used as examples
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
        // ChatGPT Stuff, this was all straight up taken from the source identified within the credits.
        
        // Innapropriate Word Detection
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
        
        public async void sendgptmsg(string[] array) 
        {

            for (int i = 1; i < array.Length; i++) // God this is so fucking cursed.
            {
                SendGlobalChatMessage(array[i]);
                await Task.Delay(1000);
                Log(array[i]);
            }
        }
    
        
        // Commands function
        public override void onInit()
        {
           base.onInit();
            // COMMAND CONFIGURATION SETUP
            ChatClient client;
           string fileName = "voidsettings.json";
            string jsonString = File.ReadAllText(fileName);
            CommandConf commandConf = System.Text.Json.JsonSerializer.Deserialize<CommandConf>(jsonString)!; 
                if (commandConf.chatgpt==false) return;
                else
                {
                    client = new(model: "gpt-4o", apiKey: commandConf.api_key);
                }
        

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
        
             RegisterCommand("chatgpt", (player, args) =>
            {
              if (commandConf.chatgpt==false) return;
            
              string query = string.Join(" ", args);
              ChatCompletion completion = client.CompleteChat(query);
                
                //Create Message array
                if (completion.Content[0].Text.Contains('\n'))
                {
                    string[] mArray = completion.Content[0].Text.Split('\n');
                    SendGlobalChatMessage($"[ChatGPT]: {mArray[0]}");
                    if (mArray.Length > 1)
                    {
                        sendgptmsg(mArray);
                    }
                    
                }
                else 
                {
                    SendGlobalChatMessage($"[ChatGPT]: {completion.Content[0].Text}");
                }
                
                Log(completion.Content[0].Text);
                return;

            });
            SetCommandDescription("chatgpt", "Full-Blown ChatGPT implementation... Do you think I'm kidding brah? Try it!");
        
        }

    }

    
}

