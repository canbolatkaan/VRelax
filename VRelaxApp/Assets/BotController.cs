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
        movements = new int[5];
        index = 0;
    }

    // Update is called once per frame
    private void Update()
    {
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
    }
    private void FixedUpdate()
    {
        
    }

}
