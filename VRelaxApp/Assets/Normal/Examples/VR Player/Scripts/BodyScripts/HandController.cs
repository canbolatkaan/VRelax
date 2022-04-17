using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


[RequireComponent(typeof(ActionBasedController))]
public class HandController : MonoBehaviour
{
    public GameObject normcoreObject;
    public bool isRight;
    RealtimeAvatarManager manager;
    ActionBasedController controller;
    Hand hand;
    string hand_type;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<ActionBasedController>();
        manager = normcoreObject.GetComponent<RealtimeAvatarManager>();
        hand_type = isRight ? "Right Hand" : "Left Hand";
    }

    // Update is called once per frame
    void Update()
    {
        if (hand != null)
        {
            hand.SetGrip(controller.selectAction.action.ReadValue<float>());
            hand.SetTrigger(controller.activateAction.action.ReadValue<float>());
        }
        else
        {
            GetHand();
        }
    }

    void GetHand()
    {
        if (manager.localAvatar == null)
        {
            //Debug.Log("Local Avatar Not Ready");
            return;
        }

        hand = manager.localAvatar.transform.Find(hand_type).GetChild(0).GetComponent<Hand>();
        //Debug.Log(hand.name);

    }
}
