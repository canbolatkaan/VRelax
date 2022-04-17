using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using TMPro;
using UnityEngine;

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
        reference = FirebaseDatabase.GetInstance("https://vrelaxapp-fabe6-default-rtdb.europe-west1.firebasedatabase.app/").RootReference;//DefaultInstance.RootReference;
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
            if (user.IsEmailVerified)
            {
                GameManager.instance.ChangeScene(2);    
            }
            else
            {
                StartCoroutine(SendVerificationEmail());
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
                Debug.Log($"Signed In: {user.DisplayName}");
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
            string output = "Unkonown error, please try again!";
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
                StartCoroutine(SendVerificationEmail());
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
            //instance.getReference;
            //private DatabaseReference db;
            

            if (registerTask.Exception != null)
            {
                FirebaseException firebaseException = (FirebaseException)registerTask.Exception.GetBaseException();
                AuthError error = (AuthError)firebaseException.ErrorCode;
                string output = "Unkonown error, please try again!";
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
                    string output = "Unkonown error, please try again!";
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
                        registerOutputText.text = "Verification e-mail sent!";
                        Debug.Log($"Firebase user created successfully {user.DisplayName} ({user.UserId})");
                        register_user(user.UserId, user.DisplayName);
                    //TODO: send verification e-mail
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
        public int duration; // in MINUTES

        public User()
        {
        }

        public User(string username, int type)
        {
            
            this.username = username;
            this.type = type;
            duration = 0;
        }
    }
    private void register_user(string uid, string name) // just will be ussed in registering process
    {
        User user = new User(name, 0);
        string json = JsonUtility.ToJson(user);
            
        reference.Child("Users").Child(uid).SetRawJsonValueAsync(json);
    }
}