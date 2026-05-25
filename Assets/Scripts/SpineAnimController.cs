using UnityEngine;
using Spine.Unity;


public class SpineAnimController : MonoBehaviour
{
    private SkeletonGraphic skeletonGraphic;
    [SerializeField]
    private string animName;
    bool isPlaying;


    void Start()
    {
        skeletonGraphic = GetComponent<SkeletonGraphic>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Play(true);
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            Stop();
        }
    }

    // ▶️ Play
    internal void Play(bool loop)
    {
        if(!isPlaying)
        {
            skeletonGraphic.AnimationState.SetAnimation(0, animName, loop);
            isPlaying = true;
        }
    }

    // ⏹️ Stop (clears animation completely)
    internal void Stop()
    { 
        if(isPlaying)
        {
            var track = skeletonGraphic.AnimationState.GetCurrent(0);
            if (track != null)
            {
                track.TrackTime = track.Animation.Duration; // jump to last frame
                track.TimeScale = 0f;                       // freeze there
            }
            isPlaying = false;
        }
    }
}
