using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

public class ButtonResetPassword : MonoBehaviour
{
    [SerializeField] private Button resetPasswordB;

    [SerializeField] private InputField emailField;

    private Coroutine resetPasswordRoutine;
    public GameObject resetPanel;

    private void Reset()
    {
        resetPasswordB = GetComponent<Button>();
        emailField = GameObject.Find("EmailField").GetComponent<InputField>();
    }

    public void ClosePanel()
    {
        resetPanel.SetActive(false);
    }

    void Start()
    {
        resetPasswordB.onClick.AddListener(HandleLoginButtonClicked);
    }

    private void HandleLoginButtonClicked()
    {
        if (resetPasswordRoutine == null)
        {
            resetPasswordRoutine = StartCoroutine(ResetPasswordCoroutine(emailField.text));
        }
    }

    private IEnumerator ResetPasswordCoroutine(string email)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var resetTask = auth.SendPasswordResetEmailAsync(email);

        yield return new WaitUntil(() => resetTask.IsCompleted);

        if (resetTask.Exception != null)
        {
            Debug.LogWarning($"Reset Failed with {resetTask.Exception}");
        }
        else
        {
            resetPanel.SetActive(true);
            Debug.Log("Password reset email sent successfully");
        }
    }
}