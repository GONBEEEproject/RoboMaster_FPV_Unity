using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Forms;
using System.Runtime.InteropServices;

public class InteractBuffer : MonoBehaviour
{
    private static InteractBuffer instance;
    public static InteractBuffer Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<InteractBuffer>();
            return instance;
        }

    }

    [SerializeField]
    private SteamVR_TrackedObject leftHand, rightHand;

    [SerializeField]
    private Transform head;
    private Vector3 prevHead;

    [SerializeField]
    private float headRot;

    [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
    static extern void SetCursorPos(int X, int Y);

    [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
    static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

    private const int MOUSE_LEFT_DOWN = 0x2;
    private const int MOUSE_LEFT_UP = 0x4;

    private const int MOUSE_RIGHT_DOWN = 0x8;
    private const int MOUSE_RIGHT_UP = 0x10;

    private int mouse_X, mouse_Y;

    private void Update()
    {
        mouse_X = System.Windows.Forms.Cursor.Position.X;
        mouse_Y = System.Windows.Forms.Cursor.Position.Y;

        var lDevice = SteamVR_Controller.Input((int)leftHand.index);
        var rDevice = SteamVR_Controller.Input((int)rightHand.index);


        if (Input.GetKeyDown(KeyCode.Return))
        {
            Shoot();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            SetCursorPos(mouse_X + 1, mouse_Y + 1);
        }

        var moveAxis = lDevice.GetAxis();
        if (moveAxis.x > 0.3f)
        {
            Right();
        }
        if (moveAxis.x < -0.3f)
        {
            Left();
        }
        if (moveAxis.y > 0.3f)
        {
            Forward();
        }
        if (moveAxis.y < -0.3f)
        {
            Backward();
        }

        var lookAxis = rDevice.GetAxis();
        SetCursorPos(mouse_X + (int)(lookAxis.x * 20), mouse_Y - (int)(lookAxis.y * 20));
        Debug.Log(lookAxis);

        //SetCursorPos(mouse_X + (int)((head.rotation.eulerAngles.y - prevHead.y)*headRot), mouse_Y + (int)((head.rotation.eulerAngles.x - prevHead.x)*headRot));

        if (rDevice.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            Shoot();
        }
        if (rDevice.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            StopShoot();
        }
    }

    private void LateUpdate()
    {
        prevHead = head.transform.rotation.eulerAngles;
    }

    public void Forward()
    {
        SendKeys.Send("w");
    }
    
    public void Backward()
    {
        SendKeys.SendWait("s");
    }

    public void Right()
    {
        SendKeys.SendWait("d");
    }

    public void Left()
    {
        SendKeys.SendWait("a");
    }

    public void Reload()
    {
        SendKeys.SendWait("r");
    }

    
    public void Shoot()
    {
        mouse_event(MOUSE_LEFT_DOWN, 0, 0, 0, 0);
        mouse_event(MOUSE_LEFT_UP, 0, 0, 0, 0);
        Debug.Log("Shoot!");
    }

    public void StopShoot()
    {
        mouse_event(MOUSE_LEFT_UP, 0, 0, 0, 0);
        Debug.Log("StopShoot!");
    }

    public void Zoom()
    {
        mouse_event(MOUSE_RIGHT_DOWN, 0, 0, 0, 0);
        mouse_event(MOUSE_RIGHT_UP, 0, 0, 0, 0);
        Debug.Log("Zoom!");
    }
}
