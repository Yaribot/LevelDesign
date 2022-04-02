using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Portal> portalList;
    public List<Transform> enemyPosList;
    public Transform enemyGroup;
    public Transform playerPos;

    public GameObject enemyPref;

    [SerializeField]
    private AudioSource audioSource;
    private AudioClip audioClip;

    [SerializeField]
    private AudioManager audioManager;

    public int MusicFrequency;
    
    // Start is called before the first frame update
    void Start()
    {
        audioClip = audioSource.clip;
        Debug.Log(portalList.Count);
        int nbEnemy = enemyGroup.childCount;
        for(int i = 0; i < nbEnemy; i++)
        {
            enemyPosList.Add(enemyGroup.GetChild(i).transform);
            GameObject enemy = Instantiate(enemyPref, enemyPosList[i].position, enemyPosList[i].rotation);
            enemy.GetComponent<Enemy>().aM = audioManager;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PortalTeleportation();
        //MusicFrequency = audioClip.frequency;
        //Debug.Log(MusicFrequency);
    }

    private void PortalTeleportation()
    {
        for (int i = 0; i < portalList.Count; i++)
        {
            if (i < portalList.Count -1)
            {
                if(portalList[i].teleport && portalList[i].forward)
                {                    
                    playerPos.position = portalList[i + 1].transform.position;
                    playerPos.rotation = portalList[i + 1].transform.rotation;
                    portalList[i + 1].forward = false;
                    portalList[i + 1].used = true;
                    
                }
            }
            if(i > 0)
            {
                if (portalList[i].teleport && portalList[i].used && !portalList[i].forward)
                {
                    playerPos.position = portalList[i - 1].transform.position;
                    playerPos.rotation = portalList[i - 1].transform.rotation;
                    portalList[i].used = false;
                }
            }

        }
        
    }
}
