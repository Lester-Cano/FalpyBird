using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonLogin : MonoBehaviour
{
    [SerializeField] private Button loginB;

    [SerializeField] private InputField emailField;
    [SerializeField] private InputField passwordField;

    private Coroutine loginRoutine;

    public event Action<FirebaseUser> OnLoginSucceded;
    public event Action<string> OnLoginFailed;


    private void Reset()
    {
        loginB = GetComponent<Button>();
    }

    void Start()
    {
        loginB.onClick.AddListener(HandleLoginButtonClicked);
    }

    private void HandleLoginButtonClicked()
    {
        if (loginRoutine == null)
        {
            emailField = GameObject.Find("EmailField").GetComponent<InputField>();
            passwordField = GameObject.Find("PasswordField").GetComponent<InputField>();
            loginRoutine = StartCoroutine(LoginCoroutine(emailField.text, passwordField.text));
        }
    }

    private IEnumerator LoginCoroutine(string email, string password)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogWarning($"Login Failed with {loginTask.Exception}");
            OnLoginFailed?.Invoke($"Login Failed with {loginTask.Exception}");
        }
        else
        {
            Debug.Log($"Login succeeded with {loginTask.Result}");
            OnLoginSucceded?.Invoke(loginTask.Result);
            SceneManager.LoadScene("SampleScene");
        }
    }


}
