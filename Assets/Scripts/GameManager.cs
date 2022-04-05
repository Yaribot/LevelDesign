using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Portal> portalList;
    public List<Transform> enemyPosList;
    public List<Transform> coinsPosList;
    public Transform enemyGroup;
    public Transform coinsGroup;
    public Transform playerPos;

    public GameObject enemyPref;
    public GameObject coinPref;

    [SerializeField]
    private AudioSource audioSource;
    private AudioClip audioClip;

    [SerializeField]
    private AudioManager audioManager;

    public int MusicFrequency;

    public int moneyCount;
    
    // Start is called before the first frame update
    void Start()
    {
        moneyCount = 0;
        audioClip = audioSource.clip;
        Debug.Log(portalList.Count);
        int nbEnemy = enemyGroup.childCount;
        for(int i = 0; i < nbEnemy; i++)
        {
            enemyPosList.Add(enemyGroup.GetChild(i).transform);
            GameObject enemy = Instantiate(enemyPref, enemyPosList[i].position, enemyPosList[i].rotation);
            enemy.GetComponent<Enemy>().aM = audioManager;
        }
        int nbCoins = coinsGroup.childCount;
        for(int i = 0; i < nbCoins; i++)
        {
            coinsPosList.Add(coinsGroup.GetChild(i).transform);
            GameObject coin = Instantiate(coinPref, coinsPosList[i].position, coinsPosList[i].rotation);
            Coins coinScript = coin.GetComponent<Coins>();
            coinScript.gm = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PortalTeleportation();
        //MusicFrequency = audioClip.frequency;
        //Debug.Log(MusicFrequency);
        //Debug.Log(moneyCount);
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
