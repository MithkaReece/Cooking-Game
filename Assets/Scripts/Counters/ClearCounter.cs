using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player) {
        // Player holding object
        if (player.HasKitchenObject()) {
            // Counter has object
            if (HasKitchenObject()) {
                // Player has plate
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    // Add counter ingredient to player plate
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                        GetKitchenObject().DestroySelf();
                } else {
                    // Counter has plate
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject)) {
                        // Add player ingredient to counter plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                            player.GetKitchenObject().DestroySelf();
                    }
                }
            } else {
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
