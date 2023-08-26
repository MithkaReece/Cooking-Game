using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player) {
        if (player.HasKitchenObject()) {
            if (!HasKitchenObject()) {
                //Move from player to counter
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        } else {
            if (HasKitchenObject()) {
                //Move from counter to player
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

}
