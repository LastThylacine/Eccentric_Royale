using System;
using UnityEngine;
using UnityEngine.UI;

public class AuthorizationUI : MonoBehaviour
{
    [SerializeField] private Authorization _authorization;
    [SerializeField] private InputField _loginInputField;
    [SerializeField] private InputField _passwordInputField;
    [SerializeField] private Button _signInButton;
    [SerializeField] private Button _signUpButton;
    [SerializeField] private GameObject _authorizationCanvas;
    [SerializeField] private GameObject _registrationCanvas;

    private void Awake()
    {
        _loginInputField.onEndEdit.AddListener(_authorization.SetLogin);
        _passwordInputField.onEndEdit.AddListener(_authorization.SetPassword);

        _signInButton.onClick.AddListener(SignInClick);
        _signUpButton.onClick.AddListener(SignUpClick);

        _authorization.Error += () =>
        {
            _signInButton.gameObject.SetActive(true);
            _signUpButton.gameObject.SetActive(true);
        };
    }

    private void SignUpClick()
    {
        _authorizationCanvas.SetActive(false);
        _registrationCanvas.SetActive(true);
    }

    private void SignInClick()
    {
        _signInButton.gameObject.SetActive(false);
        _signUpButton.gameObject.SetActive(false);
        _authorization.SignIn();
    }
}
