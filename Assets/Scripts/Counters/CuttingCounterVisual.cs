using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private const string CUT = "Cut";

    private CuttingCounter cutterCounter;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        cutterCounter = transform.parent.GetComponent<CuttingCounter>();
    }

    private void Start()
    {
        cutterCounter.OnCut += ContainerCounter_OnCut;
    }

    private void ContainerCounter_OnCut(object sender, System.EventArgs e) {
        animator.SetTrigger(CUT);
    }
}
