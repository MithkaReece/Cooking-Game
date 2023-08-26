using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player) {
        if (player.HasKitchenObject()) {
            Debug.Log("Player has object");
            if (!HasKitchenObject()) {
                Debug.Log("Counter receives object");
                //Move from player to counter
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        } else {
            Debug.Log("Player has no object");
            if (HasKitchenObject()) {
                Debug.Log("Counter gives object");
                //Move from counter to player
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

}
