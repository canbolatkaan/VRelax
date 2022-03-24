using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class Body : MonoBehaviour
{
    [SerializeField] private Transform rootObject, followObject;
    [SerializeField] private Vector3 positionOffset, rotationOffset, headBodyOffset;


    // Update is called once per frame
    void LateUpdate()
    {

        transform.position = followObject.TransformPoint(positionOffset);

        // Rotation update edilmeli
        //transform.rotation = followObject.rotation * Quaternion.Euler(rotationOffset);
    }
}
