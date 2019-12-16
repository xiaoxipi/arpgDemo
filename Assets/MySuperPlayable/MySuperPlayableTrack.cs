using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.3641865f, 0.8301887f, 0.4790206f)]
[TrackClipType(typeof(MySuperPlayableClip))]
[TrackBindingType(typeof(ActorManager))]
public class MySuperPlayableTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<MySuperPlayableMixerBehaviour>.Create (graph, inputCount);
    }
}
