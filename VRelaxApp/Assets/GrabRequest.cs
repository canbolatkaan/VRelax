using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Normal.Realtime;


public class GrabRequest : MonoBehaviour
{
    private RealtimeTransform realTimeTransform;
    private XRGrabInteractable xRGrabInteractable;

    // Start is called before the first frame update
    void Start()
    {
        realTimeTransform = GetComponent<RealtimeTransform>();
        xRGrabInteractable = GetComponent<XRGrabInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (xRGrabInteractable.isSelected)
        {
            realTimeTransform.RequestOwnership();
        }
    }
}
