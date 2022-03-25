using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Linq;

public class BotController : MonoBehaviour
{
    // Start is called before the first frame update
    private XRNode xrNode = XRNode.LeftHand;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;

    public Animator anim;
    private Rigidbody rb;
    public LayerMask layerMask;
    public bool ground;
    float myTime;
    public int[] movements;
    int index = 0;
    int falseCounter = 0;
    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(xrNode, devices);
        device = devices.FirstOrDefault();

    }
    void Start()
    {
        
        
        
    }

    // Update is called once per frame
    private void Update()
    {
        myTime = CountDownTimerForBot.getCurrentTime();
        if(myTime > 0 && myTime < 12)
        {
            anim.SetBool("crossJump", false);
            anim.SetBool("frontRise", true);
            anim.SetBool("airSquat", false);
            anim.SetBool("bicepsCurl", false);
            anim.SetBool("jumpingJacks", false);
        }    
        else if (myTime < 24 && myTime >= 12)
        {
            anim.SetBool("crossJump", false);
            anim.SetBool("frontRise", false);
            anim.SetBool("airSquat", true);
            anim.SetBool("bicepsCurl", false);
            anim.SetBool("jumpingJacks", false);
        }

        else if (myTime >=24 && myTime < 36)
        {
            anim.SetBool("crossJump", false);
            anim.SetBool("frontRise", false);
            anim.SetBool("airSquat", false);
            anim.SetBool("bicepsCurl", true);
            anim.SetBool("jumpingJacks", false);
        }
        else if (myTime >= 36 && myTime < 48)
        {
            anim.SetBool("crossJump", false);
            anim.SetBool("frontRise", false);
            anim.SetBool("airSquat", false);
            anim.SetBool("bicepsCurl", false);
            anim.SetBool("jumpingJacks", true);
        }
        else if (myTime >= 48 && myTime < 60)
        {
            anim.SetBool("crossJump", true);
            anim.SetBool("frontRise", false);
            anim.SetBool("airSquat", false);
            anim.SetBool("bicepsCurl", false);
            anim.SetBool("jumpingJacks", false);
        }
        else
        {
            anim.SetBool("crossJump", false);
            anim.SetBool("frontRise", false);
            anim.SetBool("airSquat", false);
            anim.SetBool("bicepsCurl", false);
            anim.SetBool("jumpingJacks", false);
        }
        /*
        if (!device.isValid)
        {
            GetDevice();
        }
        
        bool triggerButtonAction = false;

        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerButtonAction) && triggerButtonAction) {
            Debug.Log("triggered");

            this.ground = true;
        }
        else
            this.ground = false;
        
        anim.SetBool("crossJump", this.ground);
    */
    }
    private void FixedUpdate()
    {
        
    }

}
