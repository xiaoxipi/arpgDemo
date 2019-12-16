using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://arxiv.org/ftp/arxiv/papers/1509/1509.06344.pdf

public class KeyboardInput : IUserInput
{
    [Header("==== Key settings ====")]
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyLeft = "a";
    public string keyRight = "d";

    public string keyA;
    public string keyB;
    public string keyC;
    public string keyD;

    public string keyJRight = "right";
    public string keyJLeft = "left";
    public string keyJUp = "up";
    public string keyJDown = "down";

    [Header("==== Mouse settings ====")]
    public bool mouseEnable = false;
    public float mouseSensitivityX = 1.0f;
    public float mouseSensitivityY = 1.0f;
    


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (mouseEnable == true)
        {
            Jup = Input.GetAxis("Mouse Y")*mouseSensitivityY*1.5f;
            Jright = Input.GetAxis("Mouse X")*mouseSensitivityX*3.0f;
        }
        else
        {
            Jup = (Input.GetKey(keyJUp) ? 1.0f : 0) - (Input.GetKey(keyJDown) ? 1.0f : 0);
            Jright = (Input.GetKey(keyJRight) ? 1.0f : 0) - (Input.GetKey(keyJLeft) ? 1.0f : 0);
        }


        targetDup = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);


        if (inputEnable == false)
        {
            targetDup = targetDright = 0;
        }

        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
        float Dright2 = tempDAxis.x;
        float Dup2 = tempDAxis.y;


        Dmag = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));
        Devc = Dright2 * transform.right + Dup2 * transform.forward;

        run = Input.GetKey(keyA);
        defense = Input.GetKey(keyD);


        bool newJump = Input.GetKey(keyB);
        if (newJump != lastJump && newJump == true)
        {
            jump = true;
            //print("jump trigger!!!!");
        }
        else
        {
            jump = false;
        }
        lastJump = newJump;



        bool newAttack = Input.GetKey(keyC);
        if (newAttack != lastAttack && newAttack == true)
        {
            rb = true;
            //print("attack trigger!!!!");
        }
        else
        {
            rb = false;
        }
        lastAttack = newAttack;
    }

    //private Vector2 SquareToCircle(Vector2 input)
    //{
    //    Vector2 output = Vector2.zero;
    //    output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
    //    output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
    //    return output;
    //}
}
