using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Collections;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Reputation", 1)]
    internal class CommandReputation : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Rep",
            "Reputation"
        };

        public override string Help => Translations["Reputation_Help"];

        public override string Usage => Commands[0] + Translations["Reputation_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.reputation";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(!int.TryParse(args[0], out int rep))
            {
                UnturnedChat.SendMessage(executor, Translations["Reputation_Invalid"], ConsoleColor.Red);
                return;
            }
            if(args.Length < 2 || !UnturnedPlayer.TryGetPlayer(args[1], out UnturnedPlayer ply))
            {
                if(executor == null)
                {
                    UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                    return;
                }

                ply = executor;
            }

            ply.Player.skills.askRep(rep);
            UnturnedChat.SendMessage(executor, string.Format(Translations["Reputation_Give"], ply.PlayerName, rep), ConsoleColor.Green);
        }
    }
}
