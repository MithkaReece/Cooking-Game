using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    public IKitchenObjectParent GetKitchenObjectParent() { return kitchenObjectParent; }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent) {
        //Clear old parent
        if (this.kitchenObjectParent != null)
            this.kitchenObjectParent.ClearKitchenObject();
        //Add to new parent
        this.kitchenObjectParent = kitchenObjectParent;
        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;

    }

    public KitchenObjectSO GetKitchenObjectSO() {
        return kitchenObjectSO;
    }


}
