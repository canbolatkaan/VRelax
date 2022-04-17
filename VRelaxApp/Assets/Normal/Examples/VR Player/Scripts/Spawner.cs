using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json.Linq;
using Firebase.Auth;
using Newtonsoft.Json;
using System;

public class Spawner : MonoBehaviour
{
    [Header("Firebase")]
    public FirebaseAuth auth;
    public static FirebaseUser user;
    [Space(5f)]

    private RealtimeAvatarManager manager;
    private bool spawned = false;
    private Vector3 position_client;
    private Vector3 position_doctor;

    private Quaternion rotation_doctor;
    private Quaternion rotation_client;

    private int type;
    

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        manager = GetComponent<RealtimeAvatarManager>();
        position_client = new Vector3(45.7f, 1, 37.66f);
        rotation_client = new Quaternion(0, 0, 0, 0);
        position_doctor = new Vector3(45.7f, 1, 40f);
        rotation_doctor = new Quaternion(0, 180, 0, 0);

        auth = FirebaseAuth.DefaultInstance;
        

        user = auth.CurrentUser;
        


        DatabaseReference reference = FirebaseDatabase.GetInstance("https://vrelaxapp-fabe6-default-rtdb.europe-west1.firebasedatabase.app/").RootReference;
        reference
      .GetValueAsync().ContinueWithOnMainThread(task => {
          if (task.IsFaulted)
          {
              // Handle the error...
              
          }
          else if (task.IsCompleted)
          {
              DataSnapshot snapshot = task.Result;

              JObject rss = JObject.Parse(snapshot.GetRawJsonValue());
              type = Int32.Parse(rss.SelectToken("Users." + user.UserId + ".type").ToString());

          }
      });
    }


// Update is called once per frame
void Update()
    {
        if (!spawned && manager.localAvatar != null)
        {
            Debug.Log("ok");
            spawned = true;
            //if person is doctor
            if(type == 1) {
                manager.localAvatar.localPlayer.root.transform.position = position_doctor;
                manager.localAvatar.localPlayer.root.transform.rotation = rotation_doctor;
            }
            else if (type == 0)
            {
                manager.localAvatar.localPlayer.root.transform.position = position_client;
                manager.localAvatar.localPlayer.root.transform.rotation = rotation_client;
            }

        }
    }
}
