using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using TMPro;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Mail;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;

    [Header("Firebase")]
    public FirebaseAuth auth;
    public static FirebaseUser user;
    [Space(5f)]

    [Header("Login References")]
    [SerializeField]
    private TextMeshPro loginEmail;
    [SerializeField]
    private TextMeshPro loginPassword;
    [SerializeField]
    private TMP_Text loginOutputText;
    [Space(5f)]

    [Header("Register References")]
    [SerializeField]
    private TextMeshPro registerUsername;
    [SerializeField]
    private TextMeshPro registerEmail;
    [SerializeField]
    private TextMeshPro registerPassword;
    [SerializeField]
    private TextMeshPro registerConfirmPassword;
    [SerializeField]
    private TMP_Text registerOutputText;

    private DatabaseReference reference;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }
    }

    private void Start()
    {
        reference = FirebaseDatabase.GetInstance("https://vrelaxapp-fabe6-default-rtdb.europe-west1.firebasedatabase.app/").RootReference;
        StartCoroutine(CheckAndFixDependancies());
        
    }

    private IEnumerator CheckAndFixDependancies()
    {
        var checkAndFixDEpendanciesTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(predicate: () => checkAndFixDEpendanciesTask.IsCompleted);

        var dependancyResult = checkAndFixDEpendanciesTask.Result;

        if(dependancyResult == DependencyStatus.Available)
        {
            initializeFirebase();
        }
        else
        {
            Debug.LogError($"Could not resolve all Firebase dependancies: {dependancyResult}");
        }
    }

    private void initializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        StartCoroutine(CheckAutoLogin());
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private IEnumerator CheckAutoLogin()
    {
        yield return new WaitForEndOfFrame();
        if(user!= null)
        {
            var reloadUSerTask = user.ReloadAsync();

            yield return new WaitUntil(predicate: () => reloadUSerTask.IsCompleted);

            AutoLogin();
        }
        else
        {
            AuthUIManager.instance.LoginScreen();
        }
    }

    private void AutoLogin()
    {
        if( user != null)
        {
            if(user.IsEmailVerified)
            {
                
                GameManager.instance.ChangeScene(2);
            }
            else
            {
                var emailTask = user.SendEmailVerificationAsync();
                //SendVerificationEmail();
                AuthUIManager.instance.LoginScreen();
            }
           
        }
        else
        {
            AuthUIManager.instance.LoginScreen();
        }
    }


    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if(auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if(!signedIn && user != null)
            {
                Debug.Log("Signed Out!");
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                if(!user.IsEmailVerified)
                    Debug.Log($"Signed In but E-mail not verified!: {user.DisplayName}");
                else
                    Debug.Log($"Signed In!: {user.DisplayName}");
            }
        }
    }

    public void ClearOutputs()
    {
        loginOutputText.text = "";
        registerOutputText.text = "";
    }

    public void LoginButton()
    {
        StartCoroutine(LoginLogic(loginEmail.text, loginPassword.text));
    }

    public void RegisterButton()
    {
        reference.Child("Users").OrderByChild("username").EqualTo(registerUsername.text).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                foreach (var child in snapshot.Children)
                {
                    registerOutputText.text = "Username is already in use!";
                    return;
                }
            }
        });

        StartCoroutine(RegisterLogic(registerUsername.text, registerEmail.text, registerPassword.text, registerConfirmPassword.text));
    }
    
    private IEnumerator LoginLogic(string _email, string _password)
    {
        Credential credential = EmailAuthProvider.GetCredential(_email, _password);

        var loginTask = auth.SignInWithCredentialAsync(credential);
        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);

        if(loginTask.Exception != null)
        {
            FirebaseException firebaseException = (FirebaseException)loginTask.Exception.GetBaseException();
            AuthError error = (AuthError)firebaseException.ErrorCode;
            string output = "Unknown error, please try again!";
            switch (error)
            {
                case AuthError.MissingEmail:
                    output = "Please enter your e-mail!";
                    break;
                case AuthError.MissingPassword:
                    output = "Please enter your password!";
                    break;
                case AuthError.InvalidEmail:
                    output = "Invalid e-mail!";
                    break;
                case AuthError.WrongPassword:
                    output = "Wrong password!";
                    break;
                case AuthError.UserNotFound:
                    output = "Account doesn't exist!";
                    break;
            }
            loginOutputText.text = output;
        }
        else
        {
            if (user.IsEmailVerified)
            {
                yield return new WaitForSeconds(1);
                GameManager.instance.ChangeScene(2);
            }

            else
            {
                loginOutputText.text = "Please verify your e-mail!";
            }        
        }
    }

    private IEnumerator RegisterLogic(string _username, string _email, string _password, string _confirmPassword)
    {
        if (_username == "") registerOutputText.text = "Please enter a username!";
        else if (_password != _confirmPassword) registerOutputText.text = "Passwords do not match!";
        else
        {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            yield return new WaitUntil(predicate: () => registerTask.IsCompleted);


            if (registerTask.Exception != null)
            {
                FirebaseException firebaseException = (FirebaseException)registerTask.Exception.GetBaseException();
                AuthError error = (AuthError)firebaseException.ErrorCode;
                string output = "Unknown error, please try again!";
                switch (error)
                {
                    case AuthError.InvalidEmail:
                        output = "Invalid e-mail!";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        output = "E-mail already in use!";
                        break;
                    case AuthError.WeakPassword:
                        output = "Weak password!";
                        break;
                    case AuthError.MissingEmail:
                        output = "Missing e-mail!";
                        break;
                    case AuthError.MissingPassword:
                        output = "Missing password!";
                        break;
                }
                registerOutputText.text = output;
            }
            else
            {
                UserProfile profile = new UserProfile
                {
                    DisplayName = _username,
                };

                var defaultUserTask = user.UpdateUserProfileAsync(profile);
                yield return new WaitUntil(predicate: () => defaultUserTask.IsCompleted);
                
                if(defaultUserTask.Exception != null)
                {
                    user.DeleteAsync();
                    FirebaseException firebaseException = (FirebaseException)defaultUserTask.Exception.GetBaseException();
                    AuthError error = (AuthError)firebaseException.ErrorCode;
                    string output = "Unknown error, please try again!";
                    switch (error)
                    {
                        case AuthError.Cancelled:
                            output = "Update user cancelled!";
                            break;
                        case AuthError.SessionExpired:
                            output = "Session expired!";
                            break;
                    }
                    registerOutputText.text = output;
                }
                    else
                    {
                        var emailTask = user.SendEmailVerificationAsync();
                        registerOutputText.text = "Verification e-mail sent!";
                        //SendVerificationEmail();
                        Debug.Log($"Firebase user created successfully {user.DisplayName}");
                        register_user(user.UserId, user.DisplayName, _email);
                }
            }
        }
    }

    private IEnumerator SendVerificationEmail()
    {
        if (user != null)
        {
            var emailTask = user.SendEmailVerificationAsync();

            yield return new WaitUntil(predicate: () => emailTask.IsCompleted);

            if(emailTask.Exception != null){
                FirebaseException firebaseException = (FirebaseException)emailTask.Exception.GetBaseException();
                AuthError error = (AuthError)firebaseException.ErrorCode;

                string output = "Unknown error, try again!";

                switch (error)
                {
                    case AuthError.Cancelled:
                        output = "Verification task was cancelled!";
                        break;
                    case AuthError.InvalidRecipientEmail:
                        output = "Invalid e-mail!";
                        break;
                    case AuthError.TooManyRequests:
                        output = "Too much request sended!";
                        break;
                }
                AuthUIManager.instance.AwaitVerification(false, user.Email, output);

            }
            else
            {
                AuthUIManager.instance.AwaitVerification(true, user.Email, null);
                Debug.Log("Email sent succesfully!");
            }
        }
    }

    //////// FIREBASE METHODS ///////
    public class User
    {
        public string username;
        public int type;
        public int duration;
        public string email;

        public User()
        {
        }

        public User(string username, int type, string email)
        {
            
            this.username = username;
            this.type = type;
            duration = 0;
            this.email = email;
        }
    }

    public class Appointment
    {
        public string user_id;
        public string date;

        public Appointment()
        {
        }

        public Appointment(string uid, string date)
        {
            user_id = uid;
            this.date = date;
        }
    }
    public int getUserType()
    {
        int type=-1;
        reference.GetValueAsync().ContinueWithOnMainThread(task => {
          if (task.IsFaulted)
          {
              // Handle the error...

          }
          else if (task.IsCompleted)
          {
                DataSnapshot snapshot = task.Result;
                JObject rss = JObject.Parse(snapshot.GetRawJsonValue());
                type = Int32.Parse(rss.SelectToken("Users." + user.UserId + ".type").ToString());
                Debug.Log("User type" + type);
          }
        });
        return type;
    }

    private void register_user(string uid, string name, string email) // just will be ussed in registering process
    {
        User user = new User(name, 0, email);
        string json = JsonUtility.ToJson(user);
        reference.Child("Users").Child(uid).SetRawJsonValueAsync(json);
    }

    public void saveAppointment(string patientName, string date)
    {
        string did = user.UserId;
        reference.GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Handle the error...

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                JObject rss = JObject.Parse(snapshot.GetRawJsonValue());
                string []parsed = rss.ToString().Split('"');
                for(int i = 0; i < parsed.Length; i++)
                {
                    if (parsed[i].Contains(patientName))
                    {
                        if (!parsed[i - 12].Equals("") && !parsed[i - 6].Equals(""))
                        {
                            Appointment apt = new Appointment(parsed[i - 12], date);
                            string json = JsonUtility.ToJson(apt);
                            if (reference != null)
                            {
                                try
                                {
                                    //save to db
                                    reference.Child("Appointments").Child(did).SetRawJsonValueAsync(json);
                                    //push notification
                                    string email = "vrelaxapp@gmail.com";
                                    string password = "H58UXdgQGK";

                                    var loginInfo = new NetworkCredential(email, password);
                                    var msg = new MailMessage();
                                    var smtpClient = new SmtpClient("smtp.gmail.com", 587);

                                    msg.From = new MailAddress(email);
                                    msg.To.Add(new MailAddress(parsed[i - 6]));
                                    msg.Subject = "Appointment Notification";
                                    msg.Body = "Hi, Your appointment has been saved. Date:" + date;
                                    msg.IsBodyHtml = true;

                                    smtpClient.EnableSsl = true;
                                    smtpClient.UseDefaultCredentials = false;
                                    smtpClient.Credentials = loginInfo;
                                    smtpClient.Send(msg);

                                }
                                catch (Exception e)
                                {
                                    Debug.LogError(e);
                                    Debug.LogError(e.GetBaseException());

                                }

                            }
                        }
                        break;
                    }
                    
                }    
            }
        });

       
    }
}