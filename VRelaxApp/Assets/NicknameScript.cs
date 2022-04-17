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
using UnityEngine.UI;
using TMPro;

public class NicknameScript : MonoBehaviour
{
    [Header("Firebase")]
    public FirebaseAuth auth;
    public static FirebaseUser user;
    [Space(5f)]

    private String nickname = "Nickname";
    private TextMeshPro nicknameObject;
    // Start is called before the first frame update
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        nicknameObject = gameObject.GetComponent<TextMeshPro>();
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
              nickname = (rss.SelectToken("Users." + user.UserId + ".username").ToString());
              Debug.Log(nickname);
              nicknameObject.text = nickname.ToString();

          }
      });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
