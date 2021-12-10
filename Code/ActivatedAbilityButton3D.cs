
using System;
using System.IO;

using UnityEngine;

using DiskCardGame;

namespace ActivatedAbilityFix;

public class ActivatedAbilityButton3D : ManagedBehaviour
{

    private static Vector3 buttonPosition = new( 0f, 0f, -2f); //TODO: figure out

    private static Vector3 colliderSize = new( 1.4f, .2f, 2.1f );

    private static int instanceAccum = 0;

    private ConfirmStoneButton button;

    private PlayableCard playableCard;

    private Ability ability;

    private bool wasEnabled;


    private bool CanPress
    {
        get
        {
            return Singleton<TurnManager>.Instance != null && this.playableCard != null && this.playableCard.OnBoard && !Singleton<BoardManager>.Instance.ChoosingSacrifices && !Singleton<BoardManager>.Instance.ChoosingSlot && Singleton<TurnManager>.Instance.IsPlayerMainPhase && Singleton<GlobalTriggerHandler>.Instance.StackSize == 0;
        }
    }

    public void InstantiateButton()
    {
        GameObject obj = ( GameObject ) Instantiate( Resources.Load( Path.Combine( "prefabs", "specialnodesequences", "ConfirmStoneButton" ) ) );

        obj.name = string.Concat( "ActivatedAbilityButton_", instanceAccum++ );

        obj.transform.parent = this.gameObject.transform;

        obj.transform.localPosition = buttonPosition;

        obj.AddComponent<BoxCollider>().size = colliderSize;

        button = obj.GetComponentInChildren<ConfirmStoneButton>();
    }

    public void DestroyButton()
    {
        GameObject.Destroy( button );
    }
    
    private void Start()
    {
        this.playableCard = base.GetComponent<PlayableCard>();

        ConfirmStoneButton interactable = this.button;

        interactable.CursorSelectEnded = ( Action<MainInputInteractable> ) Delegate.Combine( interactable.CursorSelectEnded, new Action<MainInputInteractable>( delegate
         ( MainInputInteractable i )
        {
              this.OnButtonPressed();
        }));
    }

    public void SetAbility(Ability ability)
    {
        this.ability = ability;
    }

    public override void ManagedUpdate()
    {
        bool canPress = this.CanPress;

        if ( canPress != this.wasEnabled )
        {
            this.button.SetEnabled( canPress );
        }
        
        this.wasEnabled = canPress;
    }

    private void OnButtonPressed()
    {
        CustomCoroutine.Instance.StartCoroutine( this.playableCard.TriggerHandler.OnTrigger( Trigger.ActivatedAbility, new object[]
        {
            this.ability
        }));
    }
}
