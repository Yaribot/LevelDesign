using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //public int sampleDataLenght = 1024;

    //public float updateStep = 0.1f;
    //private float currentUpdateTime = 0f;

    //public float clipLoudness;
    //private float[] clipSampleData;

    //public bool doSomething;

    //public float soundThresholdMax = 0.1f;
    //public float soundThresholdMin = 0.1f;

    //private float time;
    //public float timerThreshold = 0.5f;

    public AudioSource audioSource;
    public int _bankSize;
    private List<AudioSource> _soundClip;

    private float audioClipLenght;
    private float beatChangeThreshold = 10f;
    private float nextBeat = 10f;
    private bool crRunning;

    //public bool beat1, beat2, beat3;

    [SerializeField]
    private BPM bpm;

    [SerializeField]
    private float customBeat1, customBeat2, customBeat3;

    enum changeBeat
    {
        beat1,
        beat2,
        beat3,
    }
    [SerializeField]
    private changeBeat beatChanger;

    private void Awake()
    {
       // clipSampleData = new float[sampleDataLenght];
    }
    // Start is called before the first frame update
    void Start()
    {
        //doSomething = false;

        crRunning = false;

        _soundClip = new List<AudioSource>();
        for(int i = 0; i < _bankSize; i++)
        {
            GameObject soundInstance = new GameObject("sound");
            soundInstance.AddComponent<AudioSource>();
            soundInstance.transform.parent = this.transform;
            _soundClip.Add(soundInstance.GetComponent<AudioSource>());
        }

        audioClipLenght = audioSource.clip.length;
        Debug.Log(audioClipLenght);
    }

    // Update is called once per frame
    void Update()
    {
        //currentUpdateTime += Time.deltaTime;
        //if(currentUpdateTime >= updateStep)
        //{
        //    currentUpdateTime = 0f;
        //    audioSource.clip.GetData(clipSampleData, audioSource.timeSamples);
        //    clipLoudness = 0f;

        //    foreach(float sample in clipSampleData)
        //    {
        //        clipLoudness += Mathf.Abs(sample);
        //    }

        //    clipLoudness /= sampleDataLenght;
        //}

        //if(clipLoudness >= soundThresholdMin && clipLoudness <= soundThresholdMax)
        //{
        //    time += Time.deltaTime;
        //    doSomething = true;             
        //}


        //if(time >= timerThreshold)
        //{
        //    time = 0f;
        //    doSomething = false;
        //}
        //Debug.Log(doSomething);

        //Debug.Log("Time : " + audioSource.time + " NextBeat : " + nextBeat);
        //Debug.Log(crRunning);

        if (audioSource.time > nextBeat)
        {
            nextBeat = audioSource.time + beatChangeThreshold;
            StartCoroutine(ExecuteBeat1(10f));
            //Debug.Log("Beat 1 !!!");
        }
        else
        {
            if (!crRunning)
            {
                beatChanger = changeBeat.beat3;
            }
        }
        //}else if(audioSource.time == audioClipLenght % 4)
        //{
        //    beatChanger = changeBeat.beat1;
        //    Debug.Log("Multiple de 10 !!");
        //}

        switch (beatChanger)
        {
            case changeBeat.beat1:
                bpm._bpm = customBeat1;
                break;
            case changeBeat.beat2:
                bpm._bpm = customBeat2;
                break;
            case changeBeat.beat3:
                bpm._bpm = customBeat3;
                break;
            default:
                bpm._bpm = 60f;
                break;
        }
        
    }

    public void PlaySound(AudioClip clip, float volume)
    {
        for(int i = 0; i < _soundClip.Count; i++)
        {
            if (!_soundClip[i].isPlaying)
            {
                _soundClip[i].clip = clip;
                _soundClip[i].volume = volume;
                _soundClip[i].Play();

                return;
            }
        }

        GameObject soundInstance = new GameObject("sound");
        soundInstance.AddComponent<AudioSource>();
        soundInstance.transform.parent = this.transform;
        soundInstance.GetComponent<AudioSource>().clip = clip;
        soundInstance.GetComponent<AudioSource>().volume = volume;
        soundInstance.GetComponent<AudioSource>().Play();
        _soundClip.Add(soundInstance.GetComponent<AudioSource>());
    }

    IEnumerator ExecuteBeat1(float time)
    {
        crRunning = true;
        float counter = 0f;
        while(counter <= time)
        {
            counter += audioSource.time;
            beatChanger = changeBeat.beat1;
            yield return null;
        }
            crRunning = false;
    }
}
