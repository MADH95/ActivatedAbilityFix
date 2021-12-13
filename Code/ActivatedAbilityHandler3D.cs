
using System;
using System.Collections.Generic;

using UnityEngine;

using DiskCardGame;

namespace ActivatedAbilityFix;

public class ActivatedAbilityHandler3D : ManagedBehaviour
{
    private readonly List< ActivatedAbilityIconInteractable > interactables = new();

    private PlayableCard playableCard;

    private bool CanPress
    {
        get
        {
            return Singleton<TurnManager>.Instance != null && this.playableCard != null && this.playableCard.OnBoard && this.playableCard.slot.IsPlayerSlot && !Singleton<BoardManager>.Instance.ChoosingSacrifices && !Singleton<BoardManager>.Instance.ChoosingSlot && Singleton<TurnManager>.Instance.IsPlayerMainPhase && Singleton<GlobalTriggerHandler>.Instance.StackSize == 0;
        }
    }

    //TODO: fix this to work with multiple
    public void AddInteractable( ActivatedAbilityIconInteractable interactable )
    {
        this.interactables.Add( interactable );

        interactable.CursorSelectEnded = ( Action<MainInputInteractable> ) Delegate.Combine( interactable.CursorSelectEnded, new Action<MainInputInteractable>( 
            delegate ( MainInputInteractable i )
        {
            if ( this.CanPress && i is ActivatedAbilityIconInteractable icon )
                this.OnButtonPressed( icon.Ability );
        }));
    }

    public void SetCard( PlayableCard card )
    {
        this.playableCard = card;
    }

    public void OnDestroy()
    {
        foreach( var interactable in this.interactables )
        {
            GameObject.Destroy( interactable );
        }
    }

    private void OnButtonPressed( Ability ability )
    {
        CustomCoroutine.Instance.StartCoroutine( this.playableCard.TriggerHandler.OnTrigger( Trigger.ActivatedAbility, new object[]
        {
            ability
        }));
    }
}
