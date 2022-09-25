using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using System;
using Firebase.Database;
using UnityEngine.SceneManagement;

public class ButtonRegister : MonoBehaviour
{
    [SerializeField] private Button registerB;
    private Coroutine registerRoutine;

    public event Action<FirebaseUser> OnUserRegistered;
    public event Action<string> OnUserRegistrationFailed;
    public event Action<string> OnUserRegistrationSucceeded;

    private void Reset()
    {
        registerB = GetComponent<Button>();
    }

    void Start()
    {
        registerB.onClick.AddListener(RegisterButtonClick);
    }


    private void RegisterButtonClick()
    {
        string email = GameObject.Find("EmailField").GetComponent<InputField>().text;
        string password = GameObject.Find("PasswordField").GetComponent<InputField>().text;
        registerRoutine = StartCoroutine(RegisterUser(email, password));
    }



    private IEnumerator RegisterUser(string email, string password)
    {
        Debug.Log(email);
        Debug.Log(password);
        var auth = FirebaseAuth.DefaultInstance;
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            Debug.LogWarning($"Failed to register task {registerTask.Exception}");
            OnUserRegistrationFailed?.Invoke($"Failed to register task {registerTask.Exception}");
        }
        else
        {
            Debug.Log($"Succesfully registered user {registerTask.Result.Email}");
            SceneManager.LoadScene("SampleScene");

            UserData data = new UserData();

            data.username = GameObject.Find("UserField").GetComponent<InputField>().text;
            string json = JsonUtility.ToJson(data);

            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(registerTask.Result.UserId).SetRawJsonValueAsync(json);


            OnUserRegistered?.Invoke(registerTask.Result);
        }

        registerRoutine = null;
    }

    public class UserData
    {
        public string username;
        public int score;
    }


}