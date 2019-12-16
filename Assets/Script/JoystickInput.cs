using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickInput : IUserInput
{

    [Header("==== Joystick Settings ====")]
    public string axisX="axisX";
    public string axisY="axisY";
    public string axisJright = "axis4";
    public string axisJup = "axis5";
    public string btnA = "btn0";
    public string btnB = "btn1";
    public string btnC = "btn2";
    public string btnD = "btn3";
    public string btnLB = "btn4";
    public string btnRB = "btn5";
    public string btnLT = "axis3";
    public string btnRT = "axis3";
    public string btnJstick = "btn9";

    public MyButton buttonA = new MyButton();
    public MyButton buttonB = new MyButton();
    public MyButton buttonC = new MyButton();
    public MyButton buttonD = new MyButton();
    public MyButton buttonLB = new MyButton();
    public MyButton buttonRB = new MyButton();
    public MyButton buttonLT = new MyButton();
    public MyButton buttonRT = new MyButton();
    public MyButton buttonJstick = new MyButton();
   

    //public MyButton btnX = new MyButton();

    //[Header("==== Output signals ====")]
    //public float Dup;
    //public float Dright;
    //public float Dmag;
    //public Vector3 Devc;
    //public float Jup;
    //public float Jright;
    //
    ////1.pressing signal
    //public bool run;
    ////2.trigger type signal
    //public bool jump;
    //private bool lastJump;
    //public bool attack;
    //private bool lastAttack;
    ////3.double trigger
    //
    //
    //[Header("==== Other ====")]
    //public bool inputEnable = true;//软开关
    //private float targetDup;
    //private float targetDright;
    //private float velocityDup;
    //private float velocityDright;
    //

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        buttonA.Tick(Input.GetButton(btnA));
        buttonB.Tick(Input.GetButton(btnB));
        buttonC.Tick(Input.GetButton(btnC));
        buttonD.Tick(Input.GetButton(btnD));
        buttonLB.Tick(Input.GetButton(btnLB));
        buttonRB.Tick(Input.GetButton(btnRB));
        buttonLT.Tick(Input.GetAxis(btnLT) < -0.1f ? true : false);
        buttonRT.Tick(Input.GetAxis(btnRT) > 0.1f ? true : false);
        buttonJstick.Tick(Input.GetButton(btnJstick));

        //print(buttonA.IsExtending&&buttonA.OnPressed);
        //print(buttonLT.OnPressed);

        Jup = -1.0f*Input.GetAxis(axisJup);
        Jright = Input.GetAxis(axisJright);

        targetDup = Input.GetAxis(axisY);
        targetDright = Input.GetAxis(axisX);


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

        //run = Input.GetButton(btnA);
        //defense = Input.GetButton(btnLB);
        run = (buttonA.IsPressing&&!buttonA.IsDelaying)||buttonA.IsExtending;
        defense = buttonLB.IsPressing;

        //bool newJump = Input.GetButton(btnB);
        //if (newJump != lastJump && newJump == true)
        //{
        //    jump = true;
        //    //print("jump trigger!!!!");
        //}
        //else
        //{
        //    jump = false;
        //}
        //lastJump = newJump;
        jump = buttonA.OnPressed&&buttonA.IsExtending;

        roll = buttonA.OnReleased && buttonA.IsDelaying;

        action = buttonC.OnPressed;



        //bool newAttack = Input.GetButton(btnC);
        //if (newAttack != lastAttack && newAttack == true)
        //{
        //    attack = true;
        //    //print("attack trigger!!!!");
        //}
        //else
        //{
        //    attack = false;
        //}
        //lastAttack = newAttack;
        //attack = buttonC.OnPressed;
        rb = buttonRB.OnPressed;
        rt = buttonRT.OnPressed;
        lb = buttonLB.OnPressed;
        lt = buttonLT.OnPressed;

        lockon = buttonJstick.OnPressed;
    }




    //private Vector2 SquareToCircle(Vector2 input)
    //{
    //    Vector2 output = Vector2.zero;
    //    output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
    //    output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
    //    return output;
    //}
}
