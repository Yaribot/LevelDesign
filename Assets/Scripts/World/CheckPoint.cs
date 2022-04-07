using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]
    private bool isInRadius;
    public bool isActive;
    [SerializeField]
    private float checkRadius;
    [SerializeField]
    private LayerMask playerMask;
    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        isInRadius = Physics.CheckSphere(transform.position, checkRadius, playerMask);
        if (isInRadius)
        {
            isActive = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
