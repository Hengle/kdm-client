﻿
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


//  UI_TweenScale.cs
//  Author: Lu Zexi
//  2015-02-07


//ui tween scale
[AddComponentMenu("UI/Tween/UI_Tween Scale")]
public class UI_TweenScale : UI_Tween
{
    public bool fromnow = true;
    public Vector3 from = Vector3.zero;
    public Vector3 to = Vector3.one;
    public LoopType loop = LoopType.Once;
    public float duration = 1f;
    public EaseType easeType = EaseType.easeOutExpo;

    float m_fStartTime; //start time
    EasingFunction m_delEase;   //delegate function

    void Start()
    {
        if(tweenTarget == null) tweenTarget = this.gameObject;
        this.m_fStartTime = Time.realtimeSinceStartup;
        this.m_delEase = GetEasingFunction(easeType);
        if(fromnow)
        {
            from = tweenTarget.transform.localScale;
        }
    }

    void Update()
    {
        if( enabled )
        {
            float disTime = Time.realtimeSinceStartup - this.m_fStartTime;
            float rate = 0;
            switch( loop )
            {
                case LoopType.Once:
                    {
                        rate = disTime/duration;
                        if( rate > 1 ) {rate = 1;this.enabled = false;}
                        rate = this.m_delEase(0,1f,rate);
                        tweenTarget.transform.localScale = Vector3.Lerp(from , to , rate);
                    }
                    break;
                case LoopType.Loop:
                    {
                        disTime = (disTime - ((int)(disTime/duration))*duration);
                        rate = disTime / duration;
                        rate = this.m_delEase(0,1f,rate);
                        tweenTarget.transform.localScale = Vector3.Lerp(from , to , rate);
                    }
                    break;
                case LoopType.PingPong:
                    {
                        int loopNum = (int)(disTime/duration);
                        disTime = disTime - loopNum*duration;
                        rate = disTime / duration;
                        if(loopNum%2 == 1)
                            rate = 1f - rate;
                        rate = this.m_delEase(0,1f,rate);
                        tweenTarget.transform.localScale = Vector3.Lerp(from , to , rate);
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>
    static public UI_TweenScale Begin (GameObject go, float duration, Vector3 scale)
    {
        UI_TweenScale comp = UI_Tween.Begin<UI_TweenScale>(go);
        comp.duration = duration;
        comp.to = scale;

        if (duration <= 0f)
        {
            comp.tweenTarget.transform.localScale = scale;
            comp.enabled = false;
        }
        return comp;
    }

}

