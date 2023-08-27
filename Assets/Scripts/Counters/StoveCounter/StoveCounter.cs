using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoveCounter : BaseCounter, IHasProgress {
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs { public State state;  }

    public enum State {
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
                    //Progress Timer
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax });
                    //Timer reaches max
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
                        fryingTimer = 0;
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                        state = State.Fried;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    }
                }
                break;
            case State.Fried:
                if (HasKitchenObject()) {
                    //Progress Timer
                    burningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });
                    //Timer reaches max
                    if (burningTimer > burningRecipeSO.burningTimerMax) {
                        //Fried
                        burningTimer = 0;
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        state = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0f
                        });
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
            if (HasKitchenObject()) {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        GetKitchenObject().DestroySelf();

                        state = State.Idle;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0f
                        });
                    }
                }
            } else{
                //Move from player to counter if you can cut it
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Frying;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    fryingTimer = 0f;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                }
            }
        } else {
            if (HasKitchenObject()) {
                //Move from counter to player
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = 0f
                });
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

    public bool IsFried() { return state == State.Fried; }
}
