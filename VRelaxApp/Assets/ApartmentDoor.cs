using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class ApartmenDoor : XRGrabInteractable
{
    public Transform handler;
    protected override void Detach()
    {
        base.Detach();
        transform.position = handler.transform.position;
        transform.rotation = handler.transform.rotation;

        Rigidbody rbhandler = handler.GetComponent<Rigidbody>();
        rbhandler.velocity = Vector3.zero;
        rbhandler.angularVelocity = Vector3.zero;

    }


    protected override void Grab()
    {
        base.Grab();

        Debug.Log("Apartment door is grabbed");

    }

}
 