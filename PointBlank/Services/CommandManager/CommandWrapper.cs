using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Commands;
using CMD = PointBlank.API.Commands.PointBlankCommand;
using Newtonsoft.Json.Linq;
using UnityEngine;
using PointBlank.Framework.Translations;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using PointBlank.API.Server;

namespace PointBlank.Services.CommandManager
{
    internal class CommandWrapper
    {
        #region Variables
        private TranslationList Translations;
        #endregion

        #region Properties
        public Type Class { get; private set; }
        public JObject Config { get; private set; }

        public CMD CommandClass { get; private set; }

        public string[] Commands { get; private set; }
        public string Permission { get; private set; }
        public int Cooldown { get; private set; }
        public bool Enabled { get; private set; }
        #endregion

        public CommandWrapper(Type _class, JObject config)
        {
            // Set the variables
            this.Class = _class;
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
            string name = Class.Assembly.GetName().Name + "." + Class.Name;
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

        public ECommandRunError Execute(PointBlankPlayer executor, string[] args)
        {
            try
            {
                if (CommandClass.AllowedServerState == EAllowedServerState.LOADING && Server.IsRunning)
                {
                    PointBlankPlayer.SendMessage(executor, Translations["CommandWrapper_Running"], ConsoleColor.Red);
                    return ECommandRunError.SERVER_RUNNING;
                }
                if (CommandClass.AllowedServerState == EAllowedServerState.RUNNING && !Server.IsRunning)
                {
                    PointBlankPlayer.SendMessage(executor, Translations["CommandWrapper_NotRunning"], ConsoleColor.Red);
                    return ECommandRunError.SERVER_LOADING;
                }
                if (CommandClass.AllowedCaller == EAllowedCaller.SERVER && executor != null)
                {
                    executor.SendMessage(Translations["CommandWrapper_NotConsole"], Color.red);
                    return ECommandRunError.NOT_CONSOLE;
                }
                if (CommandClass.AllowedCaller == EAllowedCaller.PLAYER && executor == null)
                {
                    executor.SendMessage(Translations["CommandWrapper_NotPlayer"], Color.red);
                    return ECommandRunError.NOT_PLAYER;
                }
                if (CommandClass.MinimumParams > args.Length)
                {
                    PointBlankPlayer.SendMessage(executor, Translations["CommandWrapper_Arguments"], ConsoleColor.Red);
                    return ECommandRunError.ARGUMENT_COUNT;
                }
                if(executor != null && executor.HasCooldown(CommandClass))
                {
                    executor.SendMessage(Translations["CommandWrapper_Cooldown"], Color.red);
                    return ECommandRunError.COOLDOWN;
                }
                bool shouldExecute = true;

                CommandEvents.RunCommandExecute(CommandClass, args, executor, ref shouldExecute);
                if (!shouldExecute) return ECommandRunError.NO_EXECUTE;
                executor?.SetCooldown(CommandClass, DateTime.Now);
                CommandClass.Execute(executor, args);
                return ECommandRunError.NONE;
            }
            catch (Exception ex)
            {
                Logging.LogError("Error when running command: " + Class.Name, ex);
                return ECommandRunError.EXCEPTION;
            }
        }
        #endregion
    }
}
