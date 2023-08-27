using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
    public static readonly float burnShowWarningAmount = .5f;

    private StoveCounter stoveCounter;

    private void Awake()
    {
        stoveCounter = transform.parent.GetComponent<StoveCounter>();
    }

    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
        Hide();
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowWarningAmount;
        if (show) Show();
        else Hide();
    }

    private void Show() { gameObject.SetActive(true); }
    private void Hide() { gameObject.SetActive(false); }
}
