using System;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.API.Extensions;
using Rocket.Core.Plugins;
using Rocket.Core.Logging;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using Rocket.Unturned.Plugins;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using UnityEngine;
using SDG;
using Rocket.Unturned.Permissions;

namespace Whitelist
{
    public class Whitelist : RocketPlugin<Config>
    {
        protected override void Load()
        {
            Steamworks.SteamGameServer.SetKeyValue("maxprotectedslots", Convert.ToString(this.Configuration.Instance.WhitelistedSlots));
            if(Configuration.Instance.WhitelistedGroups.Count == 0 || Configuration.Instance.WhitelistedSlots == 0)
            {
                Logger.LogWarning("Whitelist is not configured. Therefore disabled.");
                return;
            }
            UnturnedPermissions.OnJoinRequested += UnturnedPermissions_OnJoinRequested;
        }

        private void UnturnedPermissions_OnJoinRequested(Steamworks.CSteamID player, ref ESteamRejection? rejectionReason)
        {
            if ((Steam.Players.Count + this.Configuration.Instance.WhitelistedSlots) >= Steam.MaxPlayers)
            {
                List<Rocket.Core.Serialisation.RocketPermissionsGroup> pGroups = Rocket.Core.R.Permissions.GetGroups(new WhitelistPlayer(player), true);
                bool whitelisted = pGroups.Where(pg => this.Configuration.Instance.WhitelistedGroups.Select(wg => wg.ToLower()).Contains(pg.Id.ToLower())).FirstOrDefault() != null;

                if(!whitelisted)
                    rejectionReason = ESteamRejection.WHITELISTED;
            }
        }

        protected override void Unload()
        {
            UnturnedPermissions.OnJoinRequested -= UnturnedPermissions_OnJoinRequested;
        }
    }

    public class WhitelistPlayer : IRocketPlayer
    {
        string id, displayname;
        public string Id
        {
            get {return id;}
            set {id = value;}
        }

        public string DisplayName
        {
            get {return displayname;}
            set {displayname = value;}
        }

        public WhitelistPlayer(Steamworks.CSteamID _id)
        {
            Id = _id.m_SteamID.ToString();
        }
    }
}