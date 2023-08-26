using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter
{
    private enum State {
        Idle,
        Frying,
        Fried,
        Burned,
    }
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;

    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        switch(state) {
            case State.Idle:
                break;
            case State.Frying:
                if (HasKitchenObject()) {
                    fryingTimer += Time.deltaTime;
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
                        //Fried
                        fryingTimer = 0;
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                        Debug.Log("Fried");
                    }
                }
                break;
            case State.Fried:
                if (HasKitchenObject()) {
                    burningTimer += Time.deltaTime;
                    if (burningTimer > burningRecipeSO.burningTimerMax) {
                        //Fried
                        burningTimer = 0;
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        state = State.Fried;
                        Debug.Log("Burnt");
                    }
                }
                break;
            case State.Burned:
                break;
        }

    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject()) {
            if (!HasKitchenObject()) {
                //Move from player to counter if you can cut it
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Frying;
                    fryingTimer = 0f;
                }
            }
        } else {
            if (HasKitchenObject()) {
                //Move from counter to player
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        return GetFryingRecipeSOWithInput(inputKitchenObjectSO) != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        return GetFryingRecipeSOWithInput(inputKitchenObjectSO)?.output;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
                return fryingRecipeSO;
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray) {
            if (burningRecipeSO.input == inputKitchenObjectSO)
                return burningRecipeSO;
        }
        return null;
    }
}
