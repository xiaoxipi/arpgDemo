//keyboard input
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerInput pi;
    public float horizontalSpeed=100.0f;
    public float verticalSpeed = 80.0f;
    public float cameraDampValue = 0.05f;

    private GameObject playerHandle;
    private GameObject cameraHandle;
    private GameObject model;
    private float tempEulerX;
    private GameObject mainCamera;

    private Vector3 cameraDampVelocity;
    // Start is called before the first frame update
    void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerX = 20;
        model = playerHandle.GetComponent<ActorController>().model;
        mainCamera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 tempModelEuler = model.transform.eulerAngles;

        playerHandle.transform.Rotate(Vector3.up, pi.Jright * horizontalSpeed* Time.fixedDeltaTime);
        //cameraHandle.transform.Rotate(Vector3.right, pi.Jup * -verticalSpeed * Time.deltaTime);
        //tempEulerX = cameraHandle.transform.eulerAngles.x;
        tempEulerX -= pi.Jup * verticalSpeed * Time.fixedDeltaTime;
        tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);
        cameraHandle.transform.localEulerAngles = new Vector3(tempEulerX,0,0);

        model.transform.eulerAngles = tempModelEuler;

        //mainCamera.transform.position =Vector3.Lerp(mainCamera.transform.position, transform.position, 0.2f);
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, transform.position, ref cameraDampVelocity,cameraDampValue);
        mainCamera.transform.eulerAngles = transform.eulerAngles;
    }
}
*/

//Joystick input
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public float horizontalSpeed = 100.0f;
    public float verticalSpeed = 80.0f;
    public float cameraDampValue = 0.05f;
    public Image lockDot;
    public bool lockState;
    public bool isAI = false;

    private IUserInput pi;
    private GameObject playerHandle;
    private GameObject cameraHandle;
    private float tempEulerX;
    [HideInInspector]
    private GameObject model;
    private GameObject mainCamera;
    private Vector3 cameraDampVelocity;
    [SerializeField]
    private LockTarget lockTarget;

    // Start is called before the first frame update
    void Start()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerX = 20;
        ActorController ac = playerHandle.GetComponent<ActorController>();
        model = ac.model;
        pi = ac.pi;

        if (!isAI)
        {
            mainCamera = Camera.main.gameObject;
            lockDot.enabled = false;
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.lockState = CursorLockMode.None;
        }

        lockState = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (lockTarget == null)
        {
            Vector3 tempModelEuler = model.transform.eulerAngles;

            playerHandle.transform.Rotate(Vector3.up, pi.Jright * horizontalSpeed * Time.fixedDeltaTime);
            //cameraHandle.transform.Rotate(Vector3.right, pi.Jup * -verticalSpeed * Time.deltaTime);
            //tempEulerX = cameraHandle.transform.eulerAngles.x;
            tempEulerX -= pi.Jup * verticalSpeed * Time.fixedDeltaTime;
            tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);
            cameraHandle.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);

            model.transform.eulerAngles = tempModelEuler;
        }
        else
        {
            //lockDot.transform.position = Camera.main.WorldToScreenPoint(lockTarget.transform.position);
            Vector3 tempForward = lockTarget.obj.transform.position - model.transform.position;
            tempForward.y = 0;
            playerHandle.transform.forward = tempForward;
            cameraHandle.transform.LookAt(lockTarget.obj.transform);
        }

        if(!isAI)
        {
            //mainCamera.transform.position =Vector3.Lerp(mainCamera.transform.position, transform.position, 0.2f);
            mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, transform.position, ref cameraDampVelocity, cameraDampValue);
            //mainCamera.transform.eulerAngles = transform.eulerAngles;
            mainCamera.transform.LookAt(cameraHandle.transform);

        }
    }

    void Update()
    {
        if (lockTarget != null)
        {
            //print(lockTarget.halfHeight);
            if (!isAI)
            {
                lockDot.rectTransform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position + new Vector3(0, lockTarget.halfHeight, 0));
            }

            if (Vector3.Distance(model.transform.position, lockTarget.obj.transform.position) > 10.0f)
            {
                LockProcessA(null, false, false, isAI);
            }


            if (lockTarget!=null&&lockTarget.am != null && lockTarget.am.sm.isDie)
            {

                LockProcessA(null, false, false, isAI);

            }
        }
    }

    private void LockProcessA(LockTarget _lockTarget,bool _lockDotEnable,bool _lockState,bool _isAI)
    {
        lockTarget = _lockTarget;
        if (!_isAI)
        {
            lockDot.enabled = _lockDotEnable;

        }
        lockState = _lockState;
    }

    public void LockUnlock()
    {
        Vector3 modelOrigin1 = model.transform.position;
        Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
        Vector3 boxCenter = modelOrigin2 + model.transform.forward * 5.0f;
        Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f, 5.0f), model.transform.rotation, LayerMask.GetMask("Enemy"));

        if (cols.Length == 0)
        {
            LockProcessA(null, false, false, isAI);
        }
        else
        {
            foreach (var col in cols)
            {
                if (lockTarget!=null&& lockTarget.obj == col.gameObject)
                {
                    LockProcessA(null, false, false, isAI);
                    break;
                }
                LockProcessA(new LockTarget(col.gameObject, col.bounds.extents.y), true, true, isAI);
                break;
            }
        }
        
    }


    private class LockTarget
    {
        public GameObject obj;
        public float halfHeight;
        public ActorManager am;

        public LockTarget(GameObject _obj,float _halfHeight)
        {
            obj = _obj;
            halfHeight = _halfHeight;
            am = _obj.GetComponent<ActorManager>();
        }
    };

}

