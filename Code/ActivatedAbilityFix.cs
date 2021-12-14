
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
    private const string PluginVersion = "1.0.1.0";

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
        if ( !card.Info.Abilities.Exists( elem => AbilitiesUtil.GetInfo( elem ).activated ) )
        {
            return card.temporaryMods.Exists( elem => elem.abilities.Exists( elem2 => AbilitiesUtil.GetInfo( elem2 ).activated ) );
        }

        return true;
    }
}
