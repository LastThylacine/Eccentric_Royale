using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistrationUI : MonoBehaviour
{
    [SerializeField] private Registration _registration;
    [SerializeField] private InputField _loginInputField;
    [SerializeField] private InputField _passwordInputField;
    [SerializeField] private InputField _confirmPasswordInputField;
    [SerializeField] private Button _applyButton;
    [SerializeField] private Button _signInButton;
    [SerializeField] private GameObject _authorizationCanvas;
    [SerializeField] private GameObject _registrationCanvas;

    private void Awake()
    {
        _loginInputField.onEndEdit.AddListener(_registration.SetLogin);
        _passwordInputField.onEndEdit.AddListener(_registration.SetPassword);
        _confirmPasswordInputField.onEndEdit.AddListener(_registration.SetConfirmPassword);

        _applyButton.onClick.AddListener(SignUpClick);
        _signInButton.onClick.AddListener(SignInClick);

        _registration.Error += () =>
        {
            _applyButton.gameObject.SetActive(true);
            _signInButton.gameObject.SetActive(true);
        };

        _registration.Success += () =>
        {
            _signInButton.gameObject.SetActive(true);
        };
    }

    private void SignUpClick()
    {
        _applyButton.gameObject.SetActive(false);
        _signInButton.gameObject.SetActive(false);
        _registration.SignUp();
    }
    private void SignInClick()
    {
        _registrationCanvas.SetActive(false);
        _authorizationCanvas.SetActive(true);
    }
}
