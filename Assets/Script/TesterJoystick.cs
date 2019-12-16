using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterJoystick : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        print(Input.GetAxis("RT"));
        //print(Input.GetAxis("padV"));
       //print(Input.GetButtonDown("LT"));
    }
}
