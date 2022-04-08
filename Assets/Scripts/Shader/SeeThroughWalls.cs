using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeThroughWalls : MonoBehaviour
{
    public static int posID = Shader.PropertyToID("_Position");
    public static int sizeID = Shader.PropertyToID("_Size");


    public Material wallMaterial;
    private UnityEngine.Camera cam;
    

    [SerializeField]
    private LayerMask mask;


    private float curentValueToLerp;
    private float startValue = 0f;
    private float endValue = 0.5f;
    private float time;
    public float lerpDurationOpen = 1f;
    public float lerpDurationClose = 1f;

    private bool once;
    //private bool getMatOnce;

    //[SerializeField]
    //private GameManager gm;

    //[Header("See Through Walls")]
    //[SerializeField]
    //private List<Material> listSeeThroughMat;


    private void Start()
    {
        cam = UnityEngine.Camera.main;
        once = false;
        //getMatOnce = true;
        //wallMaterial = gm.seeThroughMat;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = cam.transform.position - transform.position;
        Ray ray = new Ray(transform.position, dir.normalized);
        //RaycastHit hit;

        if (Physics.Raycast(ray, 3000, mask))
        {
            //Debug.Log(hit.transform.gameObject.name);
            //wallMaterial = hit.transform.GetComponent<Renderer>().material;
            //if(wallMaterial != null)
            //{
            //    for(int i = 0; i < listSeeThroughMat.Count; i++)
            //    {
            //        if(listSeeThroughMat[i] = wallMaterial)
            //        {
                        once = false;
                        wallMaterial.SetFloat(sizeID, Lerp(startValue, endValue, lerpDurationOpen));
                        //getMatOnce = false;
                //    }
                //}


                //if (listSeeThroughMat.Contains(wallMaterial))
                //{
                    
                //}
                //foreach(Material mat in listSeeThroughMat)
                //{
                //    listSeeThroughMat.Contains(wallMaterial)
                //    if(mat == wallMaterial)
                //    {
                        
                //    }
                //}

                
            //}

        }
        else
        {
            
            if(sizeID > 0f)
            {
                if (!once)
                {
                    time = 0f;
                }
                wallMaterial.SetFloat(sizeID, Lerp(curentValueToLerp, startValue, lerpDurationClose));
                once = true;
            }
        }

        Vector3 view =  cam.WorldToViewportPoint(transform.position);
        wallMaterial.SetVector(posID, view);
    }

    private float Lerp(float start, float end, float lerpDuration)
    {
        //time = 0f;
        if (time < lerpDuration)
        {
            curentValueToLerp = Mathf.Lerp(start, end, time / lerpDuration);
            time += Time.deltaTime;
        }
        else
        {
            curentValueToLerp = end;

        }
        return curentValueToLerp;
    }
}
