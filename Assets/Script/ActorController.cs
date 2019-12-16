/*
//keyboard
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ActorController : MonoBehaviour
{

    public GameObject model;
    public PlayerInput pi;
    public float walkSpeed = 1.4f;
    public float runMultiplier = 2.7f;
    public float jumpVelocity = 3.0f;
    public float rollVelocity = 3.0f;
    public float jabMultiplier= 1.0f;

    [Space(10)]
    [Header("==== Friction Settings ====")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;

    
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 planarVec;
    private Vector3 thrustVec;
    private bool canAttack;
    private bool lockPlanar = false;
    private CapsuleCollider col;
    private float lerpTarget;
    private Vector3 deltaPos;

    // Start is called before the first frame update
    void Awake()
    {
        anim = model.GetComponent<Animator>();
        pi = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        //if (rigid == null)
        //{
        //
        //}
    }



    // Update is called once per frame
    void Update()//Time.deltaTime
    {
        float targetRunMulti = ((pi.run) ? 2.0f: 1.0f);
        anim.SetFloat("forward", Mathf.Sqrt(pi.Dmag) * Mathf.Lerp(anim.GetFloat("forward"),targetRunMulti,0.5f));

        if (rigid.velocity.magnitude > 1.0f)
        {
            anim.SetTrigger("roll");
        }

        if (pi.jump)
        {
            anim.SetTrigger("jump");
            canAttack = false;
        }

        if (pi.attack&&CheckState("ground")&&canAttack)
        {
            anim.SetTrigger("attack");
        }

        if (pi.Dmag > 0.1f)
        {
            Vector3 targetForward = Vector3.Slerp(model.transform.forward, pi.Devc, 0.3f);
            model.transform.forward = targetForward;
        }

        if (lockPlanar == false)
        {
            planarVec = pi.Dmag * model.transform.forward * walkSpeed * (pi.run ? runMultiplier : 1.0f);
        }

        //print(CheckState("idle", "attack"));
        //print(CheckState("ground"));
    }

    void FixedUpdate()//Time.fixedDeltaTime
    {
        //rigid.position += planarVec * Time.fixedDeltaTime;
        //rigid.velocity = planarVec;//指派速度  下坡有问题
        rigid.position += deltaPos;

        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z)+thrustVec;
        thrustVec = Vector3.zero;

        deltaPos = Vector3.zero;
    }


    private bool CheckState(string stateName,string layerName="Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName);
    }

     ///
     ///Message processing block
     ///

    public void OnJumpEnter()
    {
        //print("On Jump Enter");
        thrustVec = new Vector3(0, jumpVelocity, 0);

        lockPlanar = true;
        pi.inputEnable = false;
    }

    public void IsNotGround()
    {
        //print("is not ground");
        anim.SetBool("isGround", false);
    }

    public void IsGround()
    {
        //print("Is Ground");
        anim.SetBool("isGround", true);
    }

    public void OnGroundEnter()
    {
        lockPlanar = false;
        pi.inputEnable = true;
        canAttack = true;
        col.material = frictionOne;
    }

    public void OnFallEnter()
    {
        lockPlanar = true;
        pi.inputEnable = false;
    }

    public void OnRollEnter()
    {
        thrustVec = new Vector3(0, rollVelocity, 0);

        lockPlanar = true;
        pi.inputEnable = false;
    }

    public void OnJabEnter()
    {
        lockPlanar = true;
        pi.inputEnable = false;
    }

    public void OnJabUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("jabVelocity")*jabMultiplier;
    }


    public void OnAttack1hAEnter()
    {
        //lockPlanar = true;
        pi.inputEnable = false;
        lerpTarget = 1.0f;
    }

    public void OnAttackIdleEnter()
    {
        ///lockPlanar = false;
        pi.inputEnable = true;
        //anim.SetLayerWeight(anim.GetLayerIndex("attack"), 0.0f);
        lerpTarget = 0f;
    }

    public void OnAttack1hAUpdata()
    {
        thrustVec = model.transform.forward * anim.GetFloat("attack1hAVelocity") ;

        anim.SetLayerWeight(anim.GetLayerIndex("attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), lerpTarget, 0.4f));
        // print(anim.GetFloat("attack1hAVelocity"));
    }

    public void OnGroundExit()
    {
        col.material = frictionZero;
    }

    public void OnAttackIdleUpdate()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), lerpTarget, 0.4f));
    }

    public void OnUpdateRM(object _deltaPos)
    {
        //print(_deltaPos);
        if (CheckState("attack1hC", "attack"))
        {
            deltaPos += (Vector3)_deltaPos;
        }
    }
}

*/



//Joystick Input
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ActorController : MonoBehaviour
{

    public GameObject model;
    public CameraController camcon;
    public IUserInput pi;
    public float walkSpeed = 1.4f;
    public float runMultiplier = 2.7f;
    public float jumpVelocity = 3.0f;
    public float rollVelocity = 3.0f;
    public float jabMultiplier = 1.0f;

    [Space(10)]
    [Header("==== Friction Settings ====")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;


    public Animator anim;
    private Rigidbody rigid;
    private Vector3 planarVec;
    private Vector3 thrustVec;
    private bool canAttack;
    private bool lockPlanar = false;
    private bool trackDirection = false;
    private CapsuleCollider col;
    //private float lerpTarget;
    private Vector3 deltaPos;

    [SerializeField]
    public bool leftIsShield=true;

    public delegate void OnActionDelegate();
    public event OnActionDelegate OnAction;

    // Start is called before the first frame update
    void Awake()
    {
        anim = model.GetComponent<Animator>();
        //pi = GetComponent<IUserInput>();

        IUserInput[] inputs = GetComponents<IUserInput>();
        foreach(var input in inputs)
        {
            if (input.enabled == true)
            {
                pi = input;
                break;
            }
        }

        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        /*if (rigid == null)
        {

        }*/
    }

    // Update is called once per frame
    void Update()//Time.deltaTime
    {
        if (pi.lockon)
        {
            camcon.LockUnlock();
        }

        float targetRunMulti = ((pi.run) ? 2.0f : 1.0f);

        if(camcon.lockState==false)
        {
            anim.SetFloat("forward", Mathf.Sqrt(pi.Dmag) * Mathf.Lerp(anim.GetFloat("forward"), targetRunMulti, 0.5f));
            anim.SetFloat("right", 0);
        }
        else
        {
            Vector3 loaclDVec = transform.InverseTransformVector(pi.Devc);
            anim.SetFloat("forward", loaclDVec.z * ((pi.run) ? 2.0f : 1.0f));
            anim.SetFloat("right", loaclDVec.x * ((pi.run) ? 2.0f : 1.0f));
        }
        
        //anim.SetBool("defense", pi.defense);

        if (pi.roll||rigid.velocity.magnitude>7.0f)
        {
            anim.SetTrigger("roll");
            canAttack = false;
        }

        if (pi.jump)
        {
            anim.SetTrigger("jump");
            canAttack = false;
        }

        if ((pi.lb||pi.rb) && (CheckState("ground")||(CheckStateTag("attackR")||CheckStateTag("attackL"))) && canAttack)
        {
            if (pi.rb)
            {
                anim.SetBool("R0L1", false);
                anim.SetTrigger("attack");
            }
            else if (pi.lb&&!leftIsShield)
            {
                anim.SetBool("R0L1", true);
                anim.SetTrigger("attack");
            }
            
        }


        if ((pi.rt || pi.lt) && (CheckState("ground") || CheckState("attackR") || CheckState("attackL") && canAttack))
        {
            if (pi.rt)
            {
                //do right heavy attack
            }
            else
            {
                if (!leftIsShield)
                {
                    //do left heavy attack
                }
                else
                {
                    //
                    anim.SetTrigger("counterBack");
                }
            }
        }
        
        if (leftIsShield)
        {
            if (CheckState("ground")||CheckState("blocked"))
            {
                anim.SetBool("defense", pi.defense);
                anim.SetLayerWeight(anim.GetLayerIndex("defense"), 1);
            }
            else
            {
                //anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);
                anim.SetBool("defense", false);
                anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);
            }
        }
        else
        {
            anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);
        }

        //if (CheckState("ground")&&leftIsShield)
        //{
        //    anim.SetLayerWeight(anim.GetLayerIndex("defense"), 1);
        //    if (pi.defense)
        //    {
        //        anim.SetBool("defense", pi.defense);
        //    }
        //    else
        //    {
        //        anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);
        //    }
        //}
        //else
        //{
        //    anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);
        //}

        if (camcon.lockState == false)
        {
            if (pi.inputEnable == true)
            {
                if (pi.Dmag > 0.1f)
                {
                    Vector3 targetForward = Vector3.Slerp(model.transform.forward, pi.Devc, 0.3f);
                    model.transform.forward = targetForward;
                }
            }

            if (lockPlanar == false)
            {
                planarVec = pi.Dmag * model.transform.forward * walkSpeed * (pi.run ? runMultiplier : 1.0f);
            }
        }
        else
        {
            if (trackDirection==false)
            {
                model.transform.forward = transform.forward;
            }
            else
            {
                model.transform.forward = planarVec.normalized;
            }
            if (lockPlanar == false)
            {
                planarVec = pi.Devc * walkSpeed * (pi.run ? runMultiplier : 1.0f);
            }
        }

        if (pi.action)
        {
            OnAction.Invoke();
        }

        //print(CheckState("idle", "attack"));
        //print(CheckState("ground"));
    }

    void FixedUpdate()//Time.fixedDeltaTime
    {
        //rigid.position += planarVec * Time.fixedDeltaTime;
        //rigid.velocity = planarVec;//指派速度  下坡有问题
        rigid.position += deltaPos;

        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrustVec;
        thrustVec = Vector3.zero;

        deltaPos = Vector3.zero;
    }


    public bool CheckState(string stateName, string layerName = "Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName);
    }

    public bool CheckStateTag(string tagName, string layerName = "Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsTag(tagName);
    }

    ///
    ///Message processing block
    ///

    public void OnJumpEnter()
    {
        //print("On Jump Enter");
        thrustVec = new Vector3(0, jumpVelocity, 0);

        lockPlanar = true;
        trackDirection = true;
        pi.inputEnable = false;
    }

    public void IsNotGround()
    {
        //print("is not ground");
        anim.SetBool("isGround", false);
    }

    public void IsGround()
    {
        //print("Is Ground");
        anim.SetBool("isGround", true);
    }

    public void OnGroundEnter()
    {
        lockPlanar = false;
        trackDirection = false;
        pi.inputEnable = true;
        canAttack = true;
        col.material = frictionOne;
    }

    public void OnFallEnter()
    {
        lockPlanar = true;
        pi.inputEnable = false;
    }

    public void OnRollEnter()
    {
        thrustVec = new Vector3(0, rollVelocity, 0);

        lockPlanar = true;
        trackDirection = true;
        pi.inputEnable = false;
    }

    public void OnJabEnter()
    {
        lockPlanar = true;
        pi.inputEnable = false;
    }

    public void OnJabUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("jabVelocity") * jabMultiplier;
    }


    public void OnAttack1hAEnter()
    {
        //lockPlanar = true;
        pi.inputEnable = false;
        //lerpTarget = 1.0f;
    }

    //public void OnAttackIdleEnter()
    //{
    //    ///lockPlanar = false;
    //    pi.inputEnable = true;
    //    //anim.SetLayerWeight(anim.GetLayerIndex("attack"), 0.0f);
    //   //lerpTarget = 0f;
    //}

    public void OnAttack1hAUpdata()
    {
        thrustVec = model.transform.forward * anim.GetFloat("attack1hAVelocity");

        //anim.SetLayerWeight(anim.GetLayerIndex("attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), lerpTarget, 0.4f));
        // print(anim.GetFloat("attack1hAVelocity"));
    }

    //public void OnAttackIdleUpdate()
    //{
    //    //anim.SetLayerWeight(anim.GetLayerIndex("attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), lerpTarget, 0.4f));
    //}

    public void OnAttackExit()
    {
        model.SendMessage("WeaponDisable");
    }

    public void OnGroundExit()
    {
        col.material = frictionZero;
    }


    public void OnHitEnter()
    {
        pi.inputEnable = false;
        planarVec = Vector3.zero;
    }


    public void OnUpdateRM(object _deltaPos)
    {
        //print(_deltaPos);
        if (CheckState("attack1hC"))
        {
            deltaPos += ((Vector3)_deltaPos+deltaPos)/2.0f;
        }
    }

    public void IssueTrigger(string triggerName)
    {
        anim.SetTrigger(triggerName);
    }

    public void OnBlockedEnter()
    {
        pi.inputEnable = false;
    }

    public void OnDieEnter()
    {
        pi.inputEnable = false;
        planarVec = Vector3.zero;
        model.SendMessage("WeaponDisable");
    }

    public void OnStunnedEnter()
    {
        pi.inputEnable = false;
        planarVec = Vector3.zero;
    }

    public void OnCounterBackEnter()
    {
        pi.inputEnable = false;
        planarVec = Vector3.zero;
    }

    public void OnCounterBackExit()
    {
        model.SendMessage("CounterBackDisable");
    }

    public void SetBool(string boolName,bool value)
    {
        anim.SetBool(boolName, value);
    }

    public void OnLockEnter()
    {
        pi.inputEnable = false;
        planarVec = Vector3.zero;
        model.SendMessage("WeaponDisable");
    }
}




