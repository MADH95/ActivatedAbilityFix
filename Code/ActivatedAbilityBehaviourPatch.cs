
using DiskCardGame;

using HarmonyLib;

namespace ActivatedAbilityFix;

[HarmonyPatch( typeof( ActivatedAbilityBehaviour ), nameof( ActivatedAbilityBehaviour.RespondsToResolveOnBoard ) )]
public class ActivatedAbilityBehaviourPatch
{
    [HarmonyPostfix]
    public static void PostFix( ref bool __result )
    {
        __result &= SaveManager.saveFile.IsPart2;
    }
}
