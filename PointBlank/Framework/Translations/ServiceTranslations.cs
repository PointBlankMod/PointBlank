using System;
using System.Collections.Generic;
using PointBlank.API.Collections;
using PointBlank.API.Interfaces;

namespace PointBlank.Framework.Translations
{
    internal class ServiceTranslations : ITranslatable
    {
        public override string TranslationDirectory => "";

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            // CommandManager
            { "CommandManager_Invalid", "Invalid command! Use the help command to get the list of commands!" },
            { "CommandManager_NotEnoughPermissions", "You do not have enough permissions to execute this command!" },

            // CommandWrapper
            { "CommandWrapper_Arguments", "Not enough arguments!" },
            { "CommandWrapper_Cooldown", "This command currently has a cooldown!" },
            { "CommandWrapper_NotConsole", "This command can only be executed from the console!" },
            { "CommandWrapper_NotPlayer", "This command can only be executed by a player!" },
            { "CommandWrapper_NotRunning", "This command can only be executed while the server is running!" },
            { "CommandWrapper_Running", "This command can only be executed while the server is loading!" }
        };

        public override Dictionary<Type, ITranslatable> TranslationDictionary => Enviroment.ServiceTranslations;
    }
}
