﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

//https://www.shibuya24.info/entry/timeline_basis

public class TesterDirector : MonoBehaviour
{
    public PlayableDirector pd;

    public Animator attacker;
    public Animator victim;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            foreach(var track in pd.playableAsset.outputs)
            {
                //print(track.streamName);
                if(track.streamName=="Attack Animation")
                {
                    pd.SetGenericBinding(track.sourceObject, attacker);
                }
                else if(track.streamName=="Victim Animation")
                {
                    pd.SetGenericBinding(track.sourceObject, victim);
                }
            }

            //pd.time = 0;
            //pd.Stop();
            //pd.Evaluate();
            pd.Play();
        }
    }
}
