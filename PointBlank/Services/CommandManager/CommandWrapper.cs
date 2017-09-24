using System;
using System.Linq;
using PointBlank.API;
using PointBlank.API.Commands;
using Newtonsoft.Json.Linq;
using UnityEngine;
using PointBlank.Framework.Translations;
using PointBlank.API.Collections;
using PointBlank.API.Player;
using PointBlank.API.Server;
using PointBlank.API.Permissions;

namespace PointBlank.Services.CommandManager
{
    internal class CommandWrapper
    {
        #region Variables
        private TranslationList _translations;

        private CommandManager _cmd = (CommandManager)PointBlankEnvironment.Services["CommandManager.CommandManager"].ServiceClass;
        #endregion

        #region Properties
        public Type Class { get; private set; }
        public JObject Config { get; private set; }

        public PointBlankCommand CommandClass { get; private set; }

        public string[] Commands { get; private set; }
        public PointBlankPermission Permission { get; private set; }
        public bool Enabled { get; private set; }
        #endregion

        public CommandWrapper(Type _class)
        {
            // Set the variables
            this.Class = _class;

            // Setup the variables
            CommandClass = (PointBlankCommand)Activator.CreateInstance(Class);
            _translations = PointBlankEnvironment.ServiceTranslations[typeof(ServiceTranslations)].Translations;
            Config = ((JArray)_cmd.JsonConfig.Document["Commands"]).FirstOrDefault(a => (string)a["Name"] == Class.Assembly.GetName().Name + "." + CommandClass.Name) as JObject;

            // Run the code
            Reload();
            PointBlankLogging.Log("Loaded command: " + Commands[0]);
        }
        public CommandWrapper(PointBlankCommand command)
        {
            // Set the variables
            Class = command.GetType();
            CommandClass = command;

            // Setup the variables
            _translations = PointBlankEnvironment.ServiceTranslations[typeof(ServiceTranslations)].Translations;
            Config = ((JArray)_cmd.JsonConfig.Document["Commands"]).FirstOrDefault(a => (string)a["Name"] == Class.Assembly.GetName().Name + "." + CommandClass.Name) as JObject;

            // Run the code
            Reload();
            PointBlankLogging.Log("Loaded command: " + Commands[0]);
        }

        #region Public Functions
        public void Enable()
        {
            Enabled = true;
            PointBlankCommandEvents.RunCommandEnable(CommandClass);
        }

        public void Disable()
        {
            Enabled = false;
            PointBlankCommandEvents.RunCommandDisable(CommandClass);
        }

        public void Reload()
        {
            if (Config == null)
            {
                Config = new JObject();
                ((JArray)_cmd.JsonConfig.Document["Commands"]).Add(Config);
            }
            string name = Class.Assembly.GetName().Name + "." + CommandClass.Name;
            if (Config["Name"] == null)
            {
                Config["Name"] = name;
                Config["Commands"] = JToken.FromObject(CommandClass.DefaultCommands);
                Config["Permission"] = CommandClass.DefaultPermission;
                Config["Enabled"] = Enabled;

                Commands = CommandClass.DefaultCommands;
                Permission = new PointBlankPermission(CommandClass.DefaultPermission);
                Enabled = true;
            }
            else
            {
                Commands = Config["Commands"].ToObject<string[]>();
                Permission = new PointBlankPermission((string)Config["Permission"]);
                Enabled = (bool)Config["Enabled"];
            }
        }

        public void Save() => Config["Enabled"] = Enabled;

        public ECommandRunError Execute(PointBlankPlayer executor, string[] args)
        {
            try
            {
                if (CommandClass.AllowedServerState == EAllowedServerState.Loading && PointBlankServer.IsRunning)
                {
                    PointBlankPlayer.SendMessage(executor, _translations["CommandWrapper_Running"], ConsoleColor.Red);
                    return ECommandRunError.ServerRunning;
                }
                if (CommandClass.AllowedServerState == EAllowedServerState.Running && !PointBlankServer.IsRunning)
                {
                    PointBlankPlayer.SendMessage(executor, _translations["CommandWrapper_NotRunning"], ConsoleColor.Red);
                    return ECommandRunError.ServerLoading;
                }
                if (CommandClass.AllowedCaller == EAllowedCaller.Server && executor != null)
                {
                    executor.SendMessage(_translations["CommandWrapper_NotConsole"], Color.red);
                    return ECommandRunError.NotConsole;
                }
                if (CommandClass.AllowedCaller == EAllowedCaller.Player && executor == null)
                {
                    executor.SendMessage(_translations["CommandWrapper_NotPlayer"], Color.red);
                    return ECommandRunError.NotPlayer;
                }
                if (CommandClass.MinimumParams > args.Length)
                {
                    PointBlankPlayer.SendMessage(executor, _translations["CommandWrapper_Arguments"], ConsoleColor.Red);
                    return ECommandRunError.ArgumentCount;
                }
                if(executor != null && executor.HasCooldown(CommandClass.Permission))
                {
                    executor.SendMessage(_translations["CommandWrapper_Cooldown"], Color.red);
                    return ECommandRunError.Cooldown;
                }
                bool shouldExecute = true;

                PointBlankCommandEvents.RunCommandExecute(CommandClass, args, executor, ref shouldExecute);
                if (!shouldExecute) return ECommandRunError.NoExecute;
                executor?.AddCooldown(CommandClass.Permission);
                CommandClass.Execute(executor, args);
                return ECommandRunError.None;
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Error when running command: " + Class.Name, ex);
                return ECommandRunError.Exception;
            }
        }
        #endregion
    }
}
