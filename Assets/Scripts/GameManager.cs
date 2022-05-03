using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private PlayerController playerScript;
    [SerializeField]
    private Transform spawnPlayerPos;
    [SerializeField]
    private GameObject playerPref;
    [SerializeField]
    private TextMeshProUGUI coinText, deathText;

    public List<Portal> portalList;
    public List<Transform> enemyPosList;
    public List<Transform> coinsPosList;
    public Transform enemyGroup;
    public Transform coinsGroup;
    private Transform playerPos;

    public GameObject enemyPref;
    public GameObject coinPref;

    //[SerializeField]
    //private AudioSource audioSource;
    //private AudioClip audioClip;

    [SerializeField]
    private AudioManager audioManager;

    public int MusicFrequency;

    public int moneyCount;
    public int deathCount;

    private float timerSpawn;
    private float ThresholdtimerSpawn = 1.3f;

    private bool isSpawningPlayer;

    [Header("Camera")]
    [SerializeField]
    private Camera cam;

    [Header("ReSpawn Chekpoints")]
    [SerializeField]
    private Transform groupCheckpoints;

    [SerializeField]
    private List<CheckPoint> listCheckPoints;

    [Header("Get SeeThrough Materials")]
    //[SerializeField]
    //private Transform levelGroup;
    public Material seeThroughMat;

    [Header("Scriptable Objects")]
    public IntScriptable totalDeath;
    public IntScriptable totalCoin;
    
    // Start is called before the first frame update
    void Start()
    {
        isSpawningPlayer = true;
        moneyCount = 0;
        deathCount = 0;
        //audioClip = audioSource.clip;
        Debug.Log(portalList.Count);
        int nbEnemy = enemyGroup.childCount;
        for(int i = 0; i < nbEnemy; i++)
        {
            enemyPosList.Add(enemyGroup.GetChild(i).transform);
            GameObject enemy = Instantiate(enemyPref, enemyPosList[i].position, enemyPosList[i].rotation);
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.aM = audioManager;
            enemyScript.gM = this;
        }
        int nbCoins = coinsGroup.childCount;
        for(int i = 0; i < nbCoins; i++)
        {
            coinsPosList.Add(coinsGroup.GetChild(i).transform);
            GameObject coin = Instantiate(coinPref, coinsPosList[i].position, coinsPosList[i].rotation);
            Coins coinScript = coin.GetComponent<Coins>();
            coinScript.gm = this;
        }

        //seeThroughMat = levelGroup.GetChild(0).GetComponent<Renderer>().material;

        GetCheckPointPos();
    }

    // Update is called once per frame
    void Update()
    {
        PortalTeleportation();

        if (isSpawningPlayer)
        {
            SpawningPlayer();
            
        }

        if(playerScript != null)
        {
            if (!playerScript.isEnable)
            {
                RespawnToLastCheckPoint();
            }
        }

        UpdateUi();
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
                    ParticleSystem particle = portalList[i + 1].transform.GetChild(0).transform.GetChild(1).GetComponent<ParticleSystem>();
                    particle.Play();
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
                    ParticleSystem particle = portalList[i - 1].transform.GetChild(0).transform.GetChild(1).GetComponent<ParticleSystem>();
                    particle.Play();
                    playerPos.position = portalList[i - 1].transform.position;
                    playerPos.rotation = portalList[i - 1].transform.rotation;
                    portalList[i].used = false;
                }
            }

        }
        
    }

    private void SpawningPlayer()
    {
        SeeThroughWalls playerSeeThrough;
        ParticleSystem particles1 = spawnPlayerPos.GetChild(0).transform.GetComponent<ParticleSystem>();
        ParticleSystem particles2 = spawnPlayerPos.GetChild(1).transform.GetComponent<ParticleSystem>();
        particles1.Play();
        particles2.Play();

        timerSpawn += Time.deltaTime;

        if(timerSpawn >= ThresholdtimerSpawn)
        {
            GameObject player = Instantiate(playerPref, spawnPlayerPos.position, spawnPlayerPos.rotation);
            cam.target = player.transform;
            playerPos = player.transform;
            timerSpawn = 0;
            isSpawningPlayer = false;
            playerScript = player.GetComponent<PlayerController>();
            playerSeeThrough = player.GetComponent<SeeThroughWalls>();
            playerSeeThrough.wallMaterial = seeThroughMat;
            playerScript.gm = this;
        }
    }

    private void GetCheckPointPos()
    {
        int checkPointNb = groupCheckpoints.childCount -1;
        for (int i = 0; i <= checkPointNb; i++)
        {
            listCheckPoints.Add(groupCheckpoints.GetChild(i).GetComponent<CheckPoint>());
        }
    }

    public void RespawnToLastCheckPoint()
    {
        int nbActive;
        nbActive = 0;
        if(listCheckPoints.Count > 1)
        {
            //int i = listCheckPoints.Count; i > 0; i--
            for (int i = 0; i < listCheckPoints.Count; i++)
            {
                if (listCheckPoints[i].isActive)
                {
                    nbActive = i;
                    //Debug.Log(nbActive);
                }

                if (i >= listCheckPoints.Count-1)
                {
                    ParticleSystem particle = listCheckPoints[nbActive].transform.GetChild(0).GetComponent<ParticleSystem>();
                    particle.Play();
                    playerScript.isEnable = true;
                    playerPos.position = listCheckPoints[nbActive].transform.position;
                    playerPos.rotation = listCheckPoints[nbActive].transform.rotation;                  
                }
                //else
                //{                    
                //    if (listCheckPoints[i].isActive)
                //    {
                //        nbActive = ++;
                //        //Debug.Log(nbActive);
                //    }
                //}
            }
            
        }
        else
        {
            ParticleSystem particle = listCheckPoints[0].transform.GetChild(0).GetComponent<ParticleSystem>();
            particle.Play();
            playerScript.isEnable = true;
            playerPos.position = listCheckPoints[0].transform.position;
            playerPos.rotation = listCheckPoints[0].transform.rotation;
        }

        
    }

    private void UpdateUi()
    {
        if(coinText.text != moneyCount.ToString())
        {
            coinText.text = moneyCount.ToString();
        }

        if(deathText.text != deathCount.ToString())
        {
            deathText.text = deathCount.ToString();
        }
    }
}
