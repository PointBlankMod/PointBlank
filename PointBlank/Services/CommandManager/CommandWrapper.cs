using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Commands;
using CMD = PointBlank.API.Commands.PointBlankCommand;
using CM = PointBlank.API.Unturned.Chat.UnturnedChat;
using PointBlank.API.Unturned.Player;
using Newtonsoft.Json.Linq;
using SDG.Unturned;
using UnityEngine;
using PointBlank.Framework.Translations;
using PointBlank.API.Collections;

namespace PointBlank.Services.CommandManager
{
    internal class CommandWrapper
    {
        #region Variables
        private TranslationList Translations;
        #endregion

        #region Properties
        public Type Class { get; private set; }
        public PointBlankCommandAttribute Attribute { get; private set; }
        public JObject Config { get; private set; }

        public CMD CommandClass { get; private set; }

        public string[] Commands { get; private set; }
        public string Permission { get; private set; }
        public int Cooldown { get; private set; }
        public bool Enabled { get; private set; }
        #endregion

        public CommandWrapper(Type _class, PointBlankCommandAttribute attribute, JObject config)
        {
            // Set the variables
            this.Class = _class;
            this.Attribute = attribute;
            this.Config = config;

            // Setup the variables
            CommandClass = (CMD)Activator.CreateInstance(Class);
            Translations = Enviroment.ServiceTranslations[typeof(ServiceTranslations)].Translations;

            // Run the code
            Reload();
        }

        #region Public Functions
        public void Enable()
        {
            Enabled = true;
            CommandEvents.RunCommandEnable(CommandClass);
        }

        public void Disable()
        {
            Enabled = false;
            CommandEvents.RunCommandDisable(CommandClass);
        }

        public void Reload()
        {
            string name = Attribute.GetType().Assembly.GetName().Name + "." + Attribute.Name;
            if (Config["Name"] == null)
            {
                Config["Name"] = name;
                Config["Commands"] = JToken.FromObject(CommandClass.DefaultCommands);
                Config["Permission"] = CommandClass.DefaultPermission;
                Config["Cooldown"] = CommandClass.DefaultCooldown;
                Config["Enabled"] = Enabled;

                Commands = CommandClass.DefaultCommands;
                Permission = CommandClass.DefaultPermission;
                Cooldown = CommandClass.DefaultCooldown;
                Enabled = true;
            }
            else
            {
                Commands = Config["Commands"].ToObject<string[]>();
                Permission = (string)Config["Permission"];
                Cooldown = (int)Config["Cooldown"];
                Enabled = (bool)Config["Enabled"];
            }
        }

        public void Save() => Config["Enabled"] = Enabled;

        public void Execute(UnturnedPlayer executor, string[] args)
        {
            try
            {
                if (CommandClass.AllowedServerState == EAllowedServerState.LOADING && Provider.isServer)
                {
                    CM.SendMessage(executor, Translations["CommandWrapper_Running"], ConsoleColor.Red);
                    return;
                }
                if (CommandClass.AllowedServerState == EAllowedServerState.RUNNING && !Provider.isServer)
                {
                    CM.SendMessage(executor, Translations["CommandWrapper_NotRunning"], ConsoleColor.Red);
                    return;
                }
                if (CommandClass.AllowedCaller == EAllowedCaller.SERVER && executor != null)
                {
                    executor.SendMessage(Translations["CommandWrapper_NotConsole"], Color.red);
                    return;
                }
                if (CommandClass.AllowedCaller == EAllowedCaller.PLAYER && executor == null)
                {
                    executor.SendMessage(Translations["CommandWrapper_NotPlayer"], Color.red);
                    return;
                }
                if (Attribute.MinParams > args.Length)
                {
                    CM.SendMessage(executor, Translations["CommandWrapper_Arguments"], ConsoleColor.Red);
                    return;
                }
                if(executor != null && executor.HasCooldown(CommandClass))
                {
                    executor.SendMessage(Translations["CommandWrapper_Cooldown"], Color.red);
                    return;
                }
                bool shouldExecute = true;

                CommandEvents.RunCommandExecute(CommandClass, args, executor, ref shouldExecute);
                if (!shouldExecute) return;
                executor?.SetCooldown(CommandClass, DateTime.Now);
                CommandClass.Execute(executor, args);
            }
            catch (Exception ex)
            {
                Logging.LogError("Error when running command: " + Attribute.Name, ex);
            }
        }
        #endregion
    }
}
