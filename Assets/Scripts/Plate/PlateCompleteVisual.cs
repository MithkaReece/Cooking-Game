using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateCompleteVisual : MonoBehaviour {
    [Serializable]    
    public struct KitchenObjectSO_GameObject {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSoGameObjectList;

    private PlateKitchenObject plateKitchenObject;
    private void Awake()
    {
        plateKitchenObject = transform.parent.GetComponent<PlateKitchenObject>();
    }

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSoGameObjectList) {
            kitchenObjectSOGameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach(KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSoGameObjectList) {
            if(kitchenObjectSOGameObject.kitchenObjectSO == e.KitchenObjectSO) {
                kitchenObjectSOGameObject.gameObject.SetActive(true);
            }
        }
    }
}
