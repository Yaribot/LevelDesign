using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPM : MonoBehaviour
{
    private static BPM _bpmInstance;
    public float _bpm;
    private float _beatInterval, _beatTimer, _beatIntervalD8, _beatTimerD8;
    public static bool _beatFull, _beatD8;
    public static int _beatCountFull, _beatCountD8;

    //public float[] _tapTime = new float[4];
    //public static int _tap;
    //public static bool _customBeat;

    private void Awake()
    {
        if(_bpmInstance != null && _bpmInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _bpmInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BeatDetection();
        //Tapping();
    }

    //private void Tapping()
    //{
    //    if (_customBeat)
    //    {
    //        bool once = true;
    //        if (once)
    //        {
    //            if(_tap < 4)
    //            {
    //                _tapTime[_tap] = Time.realtimeSinceStartup;
    //                _tap++;
    //            }
    //            if(_tap == 4)
    //            {
    //                float averageTime = ((_tapTime[1] - _tapTime[0]) + (_tapTime[2] - _tapTime[1]) + (_tapTime[3] - _tapTime[2])) / 3;
    //                _bpm = (float)System.Math.Round((double)60 / averageTime, 2);
    //                _tap = 0;
    //                _beatTimer = 0;
    //                _beatTimerD8 = 0;
    //                _beatCountFull = 0;
    //                _beatCountD8 = 0;
    //                _customBeat = false;
    //            }
    //            once = false;
    //        }
    //    }
    //}

    private void BeatDetection()
    {
        // Full beat Count
        _beatFull = false;
        _beatInterval = 60 / _bpm;
        _beatTimer += Time.deltaTime;

        if(_beatTimer >= _beatInterval)
        {
            _beatTimer -= _beatInterval;
            _beatFull = true;
            _beatCountFull++;
            Debug.Log("Full");
        }

        // Divided by 8 Beat Count
        _beatD8 = false;
        _beatIntervalD8 = _beatInterval / 8;
        _beatTimerD8 += Time.deltaTime;

        if(_beatTimerD8 >= _beatIntervalD8)
        {
            _beatTimerD8 -= _beatIntervalD8;
            _beatD8 = true;
            _beatCountD8++;
            Debug.Log("D8");
        }
    }
}
