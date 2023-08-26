using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private float footstepVolume = 1f;

    private Player player;
    private float footstepTimer;
    private float footStepTimerMax = .13f;

    void Awake()
    {
        player = GetComponent<Player>();    
    }

    // Update is called once per frame
    void Update()
    {
        //Always play footsteps when you are moving
        //Timer dictates how often they play when contuously moving

        footstepTimer -= Time.deltaTime;
        if (!player.IsWalking())
            footstepTimer = 0f;

        if(footstepTimer <= 0f) {
            if (player.IsWalking()) {
                SoundManager.Instance.PlayFootStepsSound(player.transform.position, footstepVolume);
                footstepTimer = footStepTimerMax;
            }
        }
    }
}
