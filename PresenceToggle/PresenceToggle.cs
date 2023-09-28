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

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> KEY_STEAM_RPC = new ModConfigurationKey<bool>("steam_rpc_enable", "If true steam rpc will be disabled", () => true);

        private static ModConfiguration config;

        public override void OnEngineInit()
        {
            config = GetConfiguration();
            Harmony harmony = new Harmony("me.art0007i.PresenceToggle");
            harmony.PatchAll();
        }

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
                    var steamConnector = Engine.Current.PlatformInterface.GetConnectors<SteamConnector>().First();
                    steamConnector.ClearCurrentStatus();
                }
                return !disable;
            }
        }
    }
}