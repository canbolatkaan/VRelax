using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


[RequireComponent(typeof(ActionBasedController))]
public class HandController : MonoBehaviour
{
    public GameObject normcoreObject;
    RealtimeAvatarManager manager;
    ActionBasedController controller;
    Hand hand;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<ActionBasedController>();
        manager = normcoreObject.GetComponent<RealtimeAvatarManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("b");
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
            Debug.Log("Local Avatar Not Ready");
            return;
        }

        Debug.Log("Hand Object: " + manager.localAvatar.transform.GetChild(1).GetChild(0).name);
        //Debug.Log();
        hand = manager.localAvatar.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Hand>(); /// burada nasıl olacak
    }
}
