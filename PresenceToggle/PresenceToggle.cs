using HarmonyLib;
using ResoniteModLoader;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using FrooxEngine;

namespace PresenceToggle
{
    public class PresenceToggle : ResoniteMod
    {
        public override string Name => "PresenceToggle";
        public override string Author => "art0007i";
        public override string Version => "2.0.0";
        public override string Link => "https://github.com/art0007i/PresenceToggle/";

        //[AutoRegisterConfigKey]
        //private static readonly ModConfigurationKey<bool> KEY_DISCORD_RPC = new ModConfigurationKey<bool>("discord_rpc_enable", "If true discord rpc will be disabled", () => true);
        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> KEY_STEAM_RPC = new ModConfigurationKey<bool>("steam_rpc_enable", "If true steam rpc will be disabled", () => true);

        private static ModConfiguration config;

        public override void OnEngineInit()
        {
            config = GetConfiguration();
            Harmony harmony = new Harmony("me.art0007i.PresenceToggle");
            harmony.PatchAll();
        }

        /* commented out for now in case discord support is added to resonite
        [HarmonyPatch(typeof(DiscordPlatformConnector))]
        class DiscordPlatformConnector_Patch
        {
            [HarmonyPrefix]
            [HarmonyPatch("SetCurrentStatus")]
            public static bool Prefix()
            {
                bool disable = config.GetValue(KEY_DISCORD_RPC);
                if (disable)
                {
                    var discord = Engine.Current.PlatformInterface.GetConnectors<DiscordPlatformConnector>().First();
                    discord.ClearCurrentStatus();
                }
                return !disable;
            }
        }
        */

        [HarmonyPatch(typeof(SteamConnector))]
        class SteamPlatformConnector_Patch
        {
            [HarmonyPrefix]
            [HarmonyPatch("SetCurrentStatus")]
            public static bool Prefix()
            {
                bool disable = config.GetValue(KEY_STEAM_RPC);
                if (disable)
                {
                    var discord = Engine.Current.PlatformInterface.GetConnectors<SteamConnector>().First();
                    discord.ClearCurrentStatus();
                }
                return !disable;
            }
        }
    }
}