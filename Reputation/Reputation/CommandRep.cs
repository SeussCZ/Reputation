using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;

namespace LuxarPL.Reputation
{
    public class ComandRep : IRocketCommand
    {
        public string Help
        {
            get { return "Shows player's reputation"; }
        }

        public string Name
        {
            get { return "rep"; }
        }

        public AllowedCaller AllowedCaller
        {
            get
            {
                return AllowedCaller.Player;
            }
        }

        public string Syntax
        {
            get { return "<playername>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "rep" };
            }
        }

        public void Execute(IRocketPlayer caller, params string[] command)
        {
            if (command.Length != 1)
            {
                int rep = Reputation.Instance.Database.GetReputation(caller.ToString());
                string ranga = Reputation.Instance.Database.GetGroup(caller.ToString());
                UnturnedChat.Say(caller, Reputation.Instance.Translations.Instance.Translate("rep_self_1", rep));
                UnturnedChat.Say(caller, Reputation.Instance.Translations.Instance.Translate("rep_self_2", ranga));
                return;
            }

            else if (command.Length == 1)
            {
                UnturnedPlayer otherPlayer = UnturnedPlayer.FromName(command[0]);
                if (otherPlayer != null)
                {
                    if (caller.Id == otherPlayer.Id)
                    {
                        int rep = Reputation.Instance.Database.GetReputation(caller.ToString());
                        string ranga = Reputation.Instance.Database.GetGroup(caller.ToString());
                        UnturnedChat.Say(caller, Reputation.Instance.Translations.Instance.Translate("rep_self_1", rep));
                        UnturnedChat.Say(caller, Reputation.Instance.Translations.Instance.Translate("rep_self_2", ranga));
                        return;
                    }
                    else
                    {
                        int rep = Reputation.Instance.Database.GetReputation(otherPlayer.Id.ToString());
                        string ranga = Reputation.Instance.Database.GetGroup(otherPlayer.Id.ToString());
                        UnturnedChat.Say(caller, Reputation.Instance.Translations.Instance.Translate("rep_1", otherPlayer.DisplayName, rep));
                        UnturnedChat.Say(caller, Reputation.Instance.Translations.Instance.Translate("rep_2", otherPlayer.DisplayName, ranga));
                        return;
                    }

                }
                else
                {
                    UnturnedChat.Say(caller, Reputation.Instance.Translations.Instance.Translate("rep_notfound"));
                }
            }

            else
            {
                UnturnedChat.Say(caller, Reputation.Instance.Translations.Instance.Translate("rep_notfound"));
            }
        }
    }
}
