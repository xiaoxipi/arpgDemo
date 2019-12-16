using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : IActorManagerInterface
{
    // Start is called before the first frame update
    //public ActorManager am;
    public float HP = 15.0f;
    public float HPMax = 15.0f;
    public float ATK = 10.0f;

    [Header("==== 1st order states flags ====")]
    public bool isGround;
    public bool isJump;
    public bool isFall;
    public bool isRoll;
    public bool isJab;
    public bool isAttack;
    public bool isHit;
    public bool isBlocked;
    public bool isDie;
    public bool isDefense;
    public bool isCounterBack;
    public bool isCounterBackEnable;

    [Header("==== 2nd order states flags ====")]
    public bool isAllowDefense;
    public bool isImmortal;
    public bool isCounterBackSuccess;
    public bool isCounterBackFailure;

    void Start()
    {
        HP = HPMax;
    }

    void Update()
    {
        isGround=am.ac.CheckState("ground");
        isJump= am.ac.CheckState("jump");
        isFall=am.ac.CheckState("fall");
        isRoll=am.ac.CheckState("roll");
        isJab=am.ac.CheckState("jab");
        isAttack=am.ac.CheckStateTag("attackR")||am.ac.CheckStateTag("attackL");
        isHit=am.ac.CheckState("hit");
        isDie=am.ac.CheckState("die");
        //isDefense = am.ac.CheckState("defense1h","defense");
        isCounterBack = am.ac.CheckState("counterBack");
        //isCounterBack = true;

        isCounterBackSuccess = isCounterBackEnable;
        isCounterBackFailure = isCounterBack && !isCounterBackEnable;

        isAllowDefense = isGround || isBlocked;
        isDefense = isAllowDefense && am.ac.CheckState("defense1h", "defense");
        isImmortal = isRoll || isJab;
    }

    public void Test()
    {
        print("sm test: HP is " + HP);
    }

    public void AddHP(float value)
    {
        HP += value;
        HP=Mathf.Clamp(HP, 0, HPMax);
        
    }

    
}
