
using System.Collections;

using DiskCardGame;

using HarmonyLib;

namespace ActivatedAbilityFix;

[HarmonyPatch( typeof( ActivatedAbilityBehaviour ) )]
public static class ActivatedAbilityBehaviourPatch
{
    [HarmonyPostfix]
    [HarmonyPatch( nameof( ActivatedAbilityBehaviour.RespondsToResolveOnBoard ) )]
    public static void RespondsToResolveOnBoard_PostFix( ref bool __result )
    {
        __result &= SaveManager.saveFile.IsPart2;
    }
}
