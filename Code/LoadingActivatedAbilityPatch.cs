
using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using DiskCardGame;

using HarmonyLib;

namespace ActivatedAbilityFix;

[HarmonyPatch( typeof( Card ) )]
public static class LoadingActivatedAbilityPatch
{
    [HarmonyPostfix]
    [HarmonyPatch( nameof( Card.UpdateInteractableIcons ) )]
    public static void UpdateInteractableIcons_PostFix( ref Card __instance )
    {
        if ( SaveManager.saveFile.IsPart2 || __instance is not PlayableCard card || !card.HasActivatedAbility() )
            return;

        var abilityHandler = __instance.gameObject.GetComponent<ActivatedAbilityHandler3D>();

        if ( abilityHandler is null )
        {
            abilityHandler = __instance.gameObject.AddComponent<ActivatedAbilityHandler3D>();
            abilityHandler.SetCard( __instance as PlayableCard );
        }

        var abilityIcons = __instance.gameObject.GetComponentsInChildren<AbilityIconInteractable>().Where( elem => AbilitiesUtil.GetInfo( elem.Ability ).activated ).ToList();

        var activatedAbilityComponents = __instance.gameObject.GetComponentsInChildren<ActivatedAbilityIconInteractable>(true).ToList();

        if ( abilityIcons.Count() == activatedAbilityComponents.Count() )
            return;


        abilityIcons.RemoveAll( elem => activatedAbilityComponents.Exists( elem2 => elem.Ability == elem2.Ability ) );

        foreach ( var icon in abilityIcons )
        {
            var go = icon.gameObject;
            go.layer = 0;

            var interactable = go.AddComponent<ActivatedAbilityIconInteractable>();
            interactable.AssigneAbility( icon.Ability );

            abilityHandler.AddInteractable( interactable );

        }
    }
}
