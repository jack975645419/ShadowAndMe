using HedgehogTeam.EasyTouch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerEffectManager : Singleton<FingerEffectManager> {

    public GameObject ParticleClass = null;
    public GameObject A = null;
    //[Tooltip("和手指Id对应，请确保FingerEffect的该值是唯一的")]
    private Dictionary<int, ParticleSystem> FingerEffects = new Dictionary<int, ParticleSystem>();

    // Use this for initialization
    override public void Start () {
        base.Start();
        
        EasyTouch.On_SwipeStart += OnSwipeStart;
        EasyTouch.On_Swipe += OnSwipe;
        EasyTouch.On_SwipeEnd += OnSwipeEnd;
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void CreateEffectByGestureIfNotExist(Gesture g)
    {
        if (!FingerEffects.ContainsKey(g.fingerIndex))
        {
            Vector3 p = g.position;
            p.z = this.transform.position.z;
            var ps = Instantiate(ParticleClass, MTool.ScreenToWorld(p), Quaternion.identity);
            FingerEffects.Add(g.fingerIndex, ps.GetComponent<ParticleSystem>());
        }
    }

    public void OnSwipeStart(Gesture g)
    {
        CreateEffectByGestureIfNotExist(g);
    }

    public void OnSwipe(Gesture g)
    {
        CreateEffectByGestureIfNotExist(g);
        Vector3 p = g.position;
        p.z = this.transform.position.z;
        FingerEffects[g.fingerIndex].gameObject.transform.position = MTool.ScreenToWorld(p);
        var emission =FingerEffects[g.fingerIndex].emission;
        emission.rateOverDistance = 80;
    }

    public void OnSwipeEnd(Gesture g)
    {
        if(FingerEffects.ContainsKey(g.fingerIndex))
        {
            var emission = FingerEffects[g.fingerIndex].emission;
            emission.rateOverDistance = 0;
        }
    }
}
