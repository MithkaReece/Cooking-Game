using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject[] visualGameObjectArray;
    private BaseCounter baseCounter;
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
        baseCounter = gameObject.transform.parent.GetComponent<BaseCounter>();
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e) {
        if(e.selectedCounter == baseCounter) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        foreach(GameObject visualGameObject in visualGameObjectArray)
            visualGameObject.SetActive(true);
    }

    private void Hide() {
        foreach (GameObject visualGameObject in visualGameObjectArray)
            visualGameObject.SetActive(false);
    }
}
