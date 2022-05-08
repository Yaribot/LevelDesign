using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool isWinTeleport = false;

    public bool activeTeleport = false, activeWinTeleport = false;
    [SerializeField]
    private float teleportRadius;
    [SerializeField]
    private LayerMask playerMask;

    private float timeToTeleport;
    [SerializeField]
    private float timeThresholdTeleport, thresholdWinTeleport;

    public bool teleport, winTeleport, used, forward;

    [SerializeField]
    private LevelLoader levelLoader;

    // Start is called before the first frame update
    void Start()
    {
        timeToTeleport = 0f;
        teleport = false;
        winTeleport = false;
        timeThresholdTeleport = 2f;
        used = false;
        forward = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWinTeleport)
        {
            NormalTeleport();
        }
        else
        {
            WinTeleport();
        }
    }

    private void NormalTeleport()
    {
        activeTeleport = Physics.CheckSphere(transform.position, teleportRadius, playerMask);
        if (activeTeleport)
        {          
            timeToTeleport += Time.deltaTime;
            if (timeToTeleport >= timeThresholdTeleport)
            {
                teleport = true;
                timeToTeleport = 0f;
            }
        }
        else
        {
            teleport = false;
        }
    }

    private void WinTeleport()
    {
        activeWinTeleport = Physics.CheckSphere(transform.position, teleportRadius, playerMask);
        if (activeWinTeleport)
        {
            timeToTeleport += Time.deltaTime;
            levelLoader.LoadNextLevel();
            if (timeToTeleport >= thresholdWinTeleport)
            {
                winTeleport = true;
                timeToTeleport = 0f;
            }
        }
        else
        {
            winTeleport = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, teleportRadius);
    }
}
