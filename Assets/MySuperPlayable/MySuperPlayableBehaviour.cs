using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class MySuperPlayableBehaviour : PlayableBehaviour
{
    public ActorManager am;
    public float myFloat;

    //PlayableDirector pd;

    public override void OnPlayableCreate (Playable playable)
    {
        
    }

    public override void OnGraphStart(Playable playable)
    {
        //Debug.Log("Graph Start");
        //base.OnGraphStart(playable);
        //pd = (PlayableDirector)playable.GetGraph().GetResolver();
        ////Debug.Log(pd);
        //foreach(var track in pd.playableAsset.outputs)
        //{
        //    if(track.streamName == "Attacker Script" || track.streamName == "Victim Script")
        //    {
        //         ActorManager am = (ActorManager)pd.GetGenericBinding(track.sourceObject);
        //        am.LockUnlockActorControllor(true);
        //    }
        //}
        //pd = (PlayableDirector)playable.GetGraph().GetResolver();
    }

    public override void OnGraphStop(Playable playable)
    {
        //Debug.Log("Graph Stop");
        //base.OnGraphStop(playable);
        //foreach (var track in pd.playableAsset.outputs)
        //{
        //    if (track.streamName == "Attacker Script"|| track.streamName == "Victim Script")
        //    {
        //        ActorManager am = (ActorManager)pd.GetGenericBinding(track.sourceObject);
        //        am.LockUnlockActorControllor(false);
        //    }
        //}

        
        //if (pd != null)
        //{
        //    pd.playableAsset = null;
        //}
        
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        //Debug.Log("Graph Play");
        //base.OnBehaviourPlay(playable, info);
        
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        //Debug.Log("Behaviour pause...");
        //base.OnBehaviourPause(playable, info);
        am.LockUnlockActorControllor(false);
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        //Debug.Log("Prepare frame===>");
        //base.PrepareFrame(playable, info);
        am.LockUnlockActorControllor(true);
    }
}
