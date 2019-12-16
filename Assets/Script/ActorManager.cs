﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    public ActorController ac;

    [Header("=== Auto Generate if Null ===")]
    public BattleManager bm;
    public WeaponManager wm;
    public StateManager sm;
    public DirectorManager dm;
    public InteractionManager im;

    [Header("==== Override Animators ====")]
    public AnimatorOverrideController oneHandAnim;
    public AnimatorOverrideController twoHandAnim;
    
    // Start is called before the first frame update
    void Awake()
    {
        ac = GetComponent<ActorController>();
        GameObject model = ac.model;
        GameObject sensor = null;
        try
        {
           sensor = transform.Find("sensor").gameObject;
        }
        catch(System.Exception ex)
        {

        }
        


        //bm = sensor.GetComponent<BattleManager>();
        //if (bm == null)
        //{
        //    bm=sensor.AddComponent<BattleManager>();
        //}
        //bm.am = this;
        //
        //wm = model.GetComponent<WeaponManager>();
        //if (wm == null)
        //{
        //    wm=model.AddComponent<WeaponManager>();
        //}
        //wm.am = this;

        //sm = gameObject.GetComponent<StateManager>();
        //if (sm == null)
        //{
        //    sm = gameObject.AddComponent<StateManager>();
        //}
        //sm.am = this;
        //sm.Test();
        bm = Bind<BattleManager>(sensor);
        wm = Bind<WeaponManager>(model);
        sm = Bind<StateManager>(gameObject);
        dm = Bind<DirectorManager>(gameObject);
        im = Bind<InteractionManager>(sensor);

        ac.OnAction += DoAction;
        //sm.Test();

    }

    public void DoAction()
    {
        if (im.overlapEcastms.Count != 0)
        {

            //
            if (im.overlapEcastms[0].active == true &&!dm.IsPlaying())
            {
                if (im.overlapEcastms[0].eventName == "frontStab")
                {
                    dm.PlayFrontStab("frontStab", this, im.overlapEcastms[0].am);
                }
                else if (im.overlapEcastms[0].eventName == "openBox")
                {
                    if (BattleManager.CheckAnglePlayer(ac.model, im.overlapEcastms[0].am.gameObject, 45))
                    {
                        im.overlapEcastms[0].active = false;
                        transform.position = im.overlapEcastms[0].am.transform.position + im.overlapEcastms[0].am.transform.TransformVector(im.overlapEcastms[0].offset);
                        ac.model.transform.LookAt(im.overlapEcastms[0].am.transform, Vector3.up);
                        dm.PlayFrontStab("openBox", this, im.overlapEcastms[0].am);
                    }
                }
                else if (im.overlapEcastms[0].eventName == "leverUp")
                {
                    if (BattleManager.CheckAnglePlayer(ac.model, im.overlapEcastms[0].am.gameObject, 45))
                    {
                        im.overlapEcastms[0].active = false;
                        transform.position = im.overlapEcastms[0].am.transform.position + im.overlapEcastms[0].am.transform.TransformVector(im.overlapEcastms[0].offset);
                        ac.model.transform.LookAt(im.overlapEcastms[0].am.transform, Vector3.up);
                        dm.PlayFrontStab("leverUp", this, im.overlapEcastms[0].am);
                    }
                }
            }
        }
    }

    private T Bind<T>(GameObject go) where T:IActorManagerInterface
    {
        T tempInstance;
        if (go == null)
        {
            return null;
        }
        tempInstance = go.GetComponent<T>();
        if (tempInstance == null)
        {
            tempInstance = go.AddComponent<T>();
        }
        tempInstance.am = this;
        return tempInstance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryDoDamage(WeaponController targetWc,bool attackValid,bool counterValid)
    {
        //ac.IssueTrigger("hit");
        //if (sm.HP > 0.0f)
        //{
        //    sm.AddHP(-5.0f);
        //}
        //ac.IssueTrigger("hit");
        if (sm.isCounterBackSuccess)
        {
            if (counterValid)
            {
                targetWc.wm.am.Stunned();
            }
        }
        else if (sm.isCounterBackFailure)
        {
            if (attackValid)
            {
                HitOrDie(targetWc,false);
            }
        }
        else if (sm.isImmortal)
        {
            //Do nothing
        }
        else if (sm.isDefense)
        {
            Blocked();
        }
        else
        {
            if (attackValid)
            {
                HitOrDie(targetWc,true);
            }
        }
    }

    public void HitOrDie(WeaponController targetWc,bool doHitAnimation)
    {
        if (sm.HP <= 0)
        {
            //Already dead
        }
        else
        {
            sm.AddHP(-1.0f*targetWc.GetATK());
            if (sm.HP > 0)
            {
                if (doHitAnimation)
                {
                    Hit();
                }
                //do some VFX,like splatter blood...
             }
            else
            {
                Die();
            }
        }
    }

    public void Stunned()
    {
        ac.IssueTrigger("stunned");
    }

    public void Blocked()
    {
        ac.IssueTrigger("blocked");
    }

    public void Hit()
    {
        ac.IssueTrigger("hit");
    }

    public void Die()
    {
        ac.IssueTrigger("die");
        ac.pi.inputEnable = false;//高耦合
        if (ac.camcon.lockState == true)
        {
            ac.camcon.LockUnlock();
            ac.camcon.enabled = false;
        }
    }

    public void setIsCounterBackEnable(bool value)
    {
        sm.isCounterBackEnable = value;
    }

    public void TestEcho()
    {
        print("Echo echo");
        
    }

    public void LockUnlockActorControllor(bool value)
    {
        ac.SetBool("lock", value);
    }

    public void ChangeDualHands(bool dualOn)
    {
        if (dualOn)
        {
            ac.anim.runtimeAnimatorController = twoHandAnim;
        }
        else
        {
            ac.anim.runtimeAnimatorController = oneHandAnim;
        }
    }

}
