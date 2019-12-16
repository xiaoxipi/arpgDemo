using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmAnimFix : MonoBehaviour
{
    // Start is called before the first frame update
    //void Start()
    //{
    //    
    //}
    //
    //// Update is called once per frame
    //void Update()
    //{
    //    
    //}

    private Animator anim;
    private ActorController ac;
    public Vector3 a;

    void Awake()
    {
        anim = GetComponent<Animator>();
        ac = GetComponentInParent<ActorController>();
    }

    private void OnAnimatorIK()
    {
        if (ac.leftIsShield)
        {
            if (anim.GetBool("defense") == false)
            {
                Transform leftLowerArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
                leftLowerArm.localEulerAngles += 0.75f * a;
                anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(leftLowerArm.localEulerAngles));
            }
        }
    }
}
