
using DiskCardGame;

using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

namespace ActivatedAbilityFix;

[BepInPlugin( PluginGuid, PluginName, PluginVersion )]
public class Plugin : BaseUnityPlugin
{
    public const string PluginGuid = "MADH.inscryption.ActivatedAbilityFix";
    private const string PluginName = "ActivatedAbilityFix";
    private const string PluginVersion = "1.0.0.0";

    internal static ManualLogSource Log;

    private void Awake()
    {
        Logger.LogInfo( $"Loaded {PluginName}!" );
        Log = base.Logger;

        Harmony harmony = new(PluginGuid);
        harmony.PatchAll();
    }
}

public static class PlayableCardExtension
{
    public static bool HasActivatedAbility( this PlayableCard card )
    {
        return card.Info.abilities.Exists( elem => AbilitiesUtil.GetInfo( elem ).activated );
    }
}
