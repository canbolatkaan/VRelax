using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private RealtimeAvatarManager manager;
    private bool spawned = false;
    private Vector3 position_client;
    private Vector3 position_doctor;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        manager = GetComponent<RealtimeAvatarManager>();
        position_client = new Vector3(45.7f, 1, 37.66f);
        position_doctor = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawned && manager.localAvatar != null)
        {
            Debug.Log("ok");
            spawned = true;
            manager.localAvatar.localPlayer.root.transform.position = position_client; 
        }
    }
}
