using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private Transform iconTemplate;

    private PlateKitchenObject plateKitchenObject;

    private void Awake()
    {
        plateKitchenObject = transform.parent.GetComponent<PlateKitchenObject>();
        iconTemplate.gameObject.SetActive(false);
    }
    // Start is called before the first frame update]
    void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        UpdateVisual();
    }

    void UpdateVisual()
    {
        //Destroy previous children
        foreach(Transform child in transform) {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach(KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<PlateIconsSingleUI>().SetKitchenObjectSO(kitchenObjectSO); 
        }        
    }
}
