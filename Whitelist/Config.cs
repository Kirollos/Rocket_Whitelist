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
    public class Config : IRocketPluginConfiguration
    {
        public int WhitelistedSlots;
        [XmlArrayItem(ElementName = "WhitelistGroup")]
        public List<string> WhitelistedGroups;

        public void LoadDefaults()
        {
            WhitelistedGroups = new List<string>();
            WhitelistedSlots = 2;
        }
    }
}
