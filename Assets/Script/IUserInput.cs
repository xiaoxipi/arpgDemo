﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInput : MonoBehaviour
{
    [Header("==== Output signals ====")]
    public float Dup;
    public float Dright;
    public float Dmag;
    public Vector3 Devc;
    public float Jup;
    public float Jright;

    //1.pressing signal
    public bool run;
    public bool defense;
    //2.trigger type signal
    public bool action;
    public bool jump;
    protected bool lastJump;
    public bool attack;
    protected bool lastAttack;
    public bool roll;
    public bool lockon;
    public bool lb;
    public bool lt;
    public bool rb;
    public bool rt;
    //3.double trigger


    [Header("==== Other ====")]
    public bool inputEnable = true;//软开关
    protected float targetDup;
    protected float targetDright;
    protected float velocityDup;
    protected float velocityDright;

    protected Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
        return output;
    }
    protected void UpdateDmagDvec(float Dup2,float Dright2)
    {
        Dmag = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));
        Devc = Dright2 * transform.right + Dup2 * transform.forward;

    }
}
