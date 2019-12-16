using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : IActorManagerInterface
{
    //public ActorManager am;
    [SerializeField]
    private Collider weaponColL;
    [SerializeField]
    private Collider weaponColR;

    public GameObject whL;
    public GameObject whR;

    public WeaponController wcL;
    public WeaponController wcR;

    void Start()
    {
        //weaponCol = whR.GetComponentInChildren<Collider>();
        //print(transform.DeepFind("weaponHandleL").name);

        try
        {
            whL = transform.DeepFind("weaponHandleL").gameObject;
            wcL = BindWeaponController(whL);
            weaponColL = whL.GetComponentInChildren<Collider>();
            weaponColL.enabled = false;
        }
        catch (System.Exception ex)
        {

        }

        try
        {
            whR = transform.DeepFind("weaponHandleR").gameObject;
            wcR = BindWeaponController(whR);
            weaponColR = whR.GetComponentInChildren<Collider>();
            weaponColR.enabled = false;
        }
        catch (System.Exception ex)
        {

        }



    }


    public void UpdateWeaponCollider(string side,Collider col)
    {
        if (side == "L")
        {
            weaponColL = col;
        }
        else if(side=="R")
        {
            weaponColR = col;
        }
    }

    public void UnloadWeapon(string side)
    {
        if (side == "L")
        {
            foreach(Transform trans in whL.transform)
            {
                weaponColL = null;
                wcL.wdata = null;
                Destroy(trans.gameObject);
            }
        }
        else if (side == "R")
        {
            foreach (Transform trans in whR.transform)
            {
                weaponColR = null;
                wcR.wdata = null;
                Destroy(trans.gameObject);
            }
        }
    }

    public WeaponController BindWeaponController(GameObject targetObj)
    {
        WeaponController tempWc;
        tempWc = targetObj.GetComponent<WeaponController>();
        if (tempWc == null)
        {
            tempWc= targetObj.AddComponent<WeaponController>();
        }
        tempWc.wm = this;
        return tempWc;
    }

    public void WeaponEnable()
    {
        //print("WeaponEnable");
        if (am.ac.CheckStateTag("attackR"))
        {
            weaponColR.enabled = true;
        }
        else
        {
            weaponColL.enabled = true;
        }
        
    }

    public void WeaponDisable()
    {
        //print("WeaponDisable");
        weaponColR.enabled = false;
        weaponColL.enabled = false;
    }
    public void CounterBackEnable()
    {
        am.setIsCounterBackEnable(true);
    }

    public void CounterBackDisable()
    {
        am.setIsCounterBackEnable(false);
    }


    public void ChangeDualHands(bool dualOn)
    {
        am.ChangeDualHands(dualOn);
    }

}