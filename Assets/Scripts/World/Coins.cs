using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coins : MonoBehaviour
{
    public GameManager gm;
    private Transform gfxTransform;
    public float animDuration;
    public Ease animEase;

    // Start is called before the first frame update
    void Start()
    {
        gfxTransform = transform.GetChild(0).transform.GetChild(0).transform;

        //gfxTransform
        //    .DOMoveY(0f, animDuration)
        //    .SetEase(animEase) // In Out Expo
        //    .SetLoops(-1, LoopType.Yoyo);
        gfxTransform
            .DORotate(new Vector3(0f, 360f, 0f), animDuration, RotateMode.FastBeyond360)
            .SetEase(animEase)
            .SetLoops(-1, LoopType.Restart);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6)
        {
            //Debug.Log("Destroy !!");
            gm.moneyCount++;
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("Not the right Layer !!");
        }
    }
}
