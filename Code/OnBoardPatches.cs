
using DiskCardGame;

using HarmonyLib;

namespace ActivatedAbilityFix;

[HarmonyPatch( typeof( PlayableCard ) )]
public class OnBoardPatches
{
    [HarmonyPostfix]
    [HarmonyPatch( nameof( PlayableCard.OnPlayed ) )]
    public void OnPlayedPostFix( ref PlayableCard __instance )
    {
        if ( !__instance.HasActivatedAbility() )
            return;

        ActivatedAbilityButton3D buttonComponent = __instance.gameObject.GetComponent<ActivatedAbilityButton3D>();

        if ( buttonComponent == null )
            buttonComponent = __instance.gameObject.AddComponent<ActivatedAbilityButton3D>();

        Ability ability = __instance.Info.abilities.Find( elem => AbilitiesUtil.GetInfo( elem ).activated );

        buttonComponent.SetAbility( ability );

        buttonComponent.InstantiateButton();
    }

    [HarmonyPatch( nameof( PlayableCard.UnassignFromSlot ) )]
    public void UnassignFromSlotPostfix( ref PlayableCard __instance )
    {
        if ( !__instance.HasActivatedAbility() )
            return;

        ActivatedAbilityButton3D buttonComponent = __instance.gameObject.GetComponent<ActivatedAbilityButton3D>();

        buttonComponent.DestroyButton();
    }
}
