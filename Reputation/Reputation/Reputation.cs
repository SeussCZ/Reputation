using Rocket;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using Rocket.Unturned.Plugins;
using SDG;
using Steamworks;
using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using System;
using System.Collections.Generic;

namespace LuxarPL.Reputation
{
    class Reputation : RocketPlugin<ReputationConfiguration>
    {
        public DatabaseManager Database;
        public static Reputation Instance;

        protected override void Load()
        {
            Instance = this;
            Database = new DatabaseManager();
            U.Events.OnPlayerConnected += Events_OnPlayerConnected;
            UnturnedPlayerEvents.OnPlayerDeath += UnturnedPlayerEvents_OnPlayerDeath;
        }

        private void UnturnedPlayerEvents_OnPlayerDeath(UnturnedPlayer player, SDG.Unturned.EDeathCause cause, SDG.Unturned.ELimb limb, CSteamID kiler)
        {
            if (kiler == player.CSteamID)
            {

            }
            else
            {
                int vRep = Instance.Database.GetReputation(player.ToString());
                int p = Instance.Configuration.Instance.ReputationForKill;
                if (vRep < 0)
                {
                    int rep = Instance.Database.AddRep(kiler.ToString(), p);
                    UnturnedChat.Say(kiler, Instance.Translations.Instance.Translate("rep_kill_b", rep));
                }
                else
                {
                    int rep = Instance.Database.RemoveRep(kiler.ToString(), p);
                    UnturnedChat.Say(kiler, Instance.Translations.Instance.Translate("rep_kill_c", rep));
                }
            }
        }

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList(){
                {"rep_self_1","Your reputation: {0}"},
                {"rep_self_2","Your group: {0}"},
                {"rep_1","{0}'s reputation: {1}"},
                {"rep_2","{0}'s reputation: {1}"},
                {"rep_notfound","Player not found"},
                {"rep_kill_b","You killed bandit. You get 1 reputation point."},
                {"rep_kill_c","You killed civilian. You lost 1 reputation point."},
                };
            }
        }

        private void Events_OnPlayerConnected(UnturnedPlayer player)
        {
            Database.CheckSetupAccount(player.CSteamID);
        }
    }
}
