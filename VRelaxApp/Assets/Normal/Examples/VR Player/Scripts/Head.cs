using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class Head : MonoBehaviour
{
    //[SerializeField] private Transform rootObject, followObject;
    [SerializeField] private Transform followObject;
    [SerializeField] private RealtimeAvatarManager manager;
    [SerializeField] private Vector3 positionOffset, rotationOffset, headBodyOffset;

    private RealtimeAvatar.LocalPlayer root;

    // Update is called once per frame
    void LateUpdate()
    {
        root = manager.localAvatar.localPlayer;
        Transform rootObject = root.head;
        Debug.Log(rootObject.rotation.x + " " + rootObject.rotation.y + " " + rootObject.rotation.z);

        rootObject.position = transform.position + headBodyOffset;
        rootObject.forward = Vector3.ProjectOnPlane(followObject.up, Vector3.up).normalized;

        transform.position = followObject.TransformPoint(positionOffset);
        transform.rotation = followObject.rotation * Quaternion.Euler(rotationOffset);
     }
}
