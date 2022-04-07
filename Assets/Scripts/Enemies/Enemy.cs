using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public bool shoot = false;
    [SerializeField]
    private ParticleSystem projectileParticle;
    private ParticleSystem.EmissionModule emissionModule;
    private SphereCollider col;
    private Transform gfxTransform;

    public AudioManager aM;

    private LayerMask playerMask;
    private float animDuration;

    private Sequence animSequence;
    private Ease animEase;

    // Start is called before the first frame update
    void Start()
    {
        emissionModule = projectileParticle.emission;
        col = transform.GetComponent<SphereCollider>();
        gfxTransform = transform.GetChild(0).transform;
        animDuration = 0.2f;
        animEase = Ease.InOutBack;
        //animSequence = DOTween.Sequence();
    }

    // Update is called once per frame
    void Update()
    {
        //if (aM.doSomething)
        //{
        //    shoot = true;
        //}

        if (BPM._beatFull)
        {
            if(BPM._beatCountFull % 2 == 0)
            {
                shoot = true;
                BPM._soundBPM = true;
                gfxTransform
                    .DOScaleY(1.5f, animDuration)
                    .SetEase(animEase)
                    .SetLoops(2, LoopType.Yoyo);
                //gfxTransform
                //    .DOScaleY(-3f, animDuration)
                //    .SetEase(animEase)
                //    .SetLoops(1);
            }
        }

        if (shoot)
        {
            //Debug.Log("SHOOT !!!");
            Shooting();
            shoot = false;
        }
    }

    private void Shooting()
    {
        projectileParticle.Play();
    }

    private void OnDestroy()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check the position of the collision by the normals of the surfaces
        foreach(ContactPoint hitPos in collision.contacts)
        {
            Debug.Log(hitPos.normal);
            if(hitPos.normal.y < -0.3)
            {
                // make the particle wait before destroy otherwise the projectile disapear with the emitter

                Destroy(this.gameObject, 3f);
                emissionModule.enabled = false;
                col.enabled = false;
                transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                PlayerController playerCtrl = collision.transform.GetComponent<PlayerController>();
                if(playerCtrl != null)
                {
                    playerCtrl.isEnable = false;
                }
                //Destroy(collision.gameObject);
            }
        }
    }
}
