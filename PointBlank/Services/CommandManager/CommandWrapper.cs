using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using Newtonsoft.Json.Linq;

namespace PointBlank.Services.CommandManager
{
    internal class CommandWrapper
    {
        #region Properties
        public Type Class { get; private set; }
        public CommandAttribute Attribute { get; private set; }
        public JObject Config { get; private set; }

        public Command CommandClass { get; private set; }

        public string[] Commands { get; private set; }
        public string Permission { get; private set; }
        public int Cooldown { get; private set; }
        public bool Enabled { get; private set; }
        #endregion

        public CommandWrapper(Type _class, CommandAttribute attribute, JObject config)
        {
            // Set the variables
            this.Class = _class;
            this.Attribute = attribute;
            this.Config = config;

            // Setup the variables
            CommandClass = (Command)Activator.CreateInstance(Class);

            // Run the code
            Reload();
        }

        #region Public Functions
        public void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }

        public void Reload()
        {
            if(Config["Name"] == null)
            {
                Config["Name"] = Attribute.Name;
                Config["Commands"] = JToken.FromObject(CommandClass.DefaultCommands);
                Config["Permission"] = CommandClass.DefaultPermission;
                Config["Cooldown"] = CommandClass.DefaultCooldown;
                Config["Enabled"] = Enabled;

                Commands = CommandClass.DefaultCommands;
                Permission = CommandClass.DefaultPermission;
                Cooldown = CommandClass.DefaultCooldown;
                Enabled = false;
            }
            else
            {
                Commands = Config["Commands"].ToObject<string[]>();
                Permission = (string)Config["Permission"];
                Cooldown = (int)Config["Cooldown"];
                Enabled = (bool)Config["Enabled"];
            }
        }

        public void Execute(UnturnedPlayer executor, string[] args)
        {
            try
            {
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
