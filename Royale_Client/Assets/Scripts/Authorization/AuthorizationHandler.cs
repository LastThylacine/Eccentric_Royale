using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthorizationHandler : MonoBehaviour
{
    [Scene, SerializeField] private string _menuSceneName;
    [SerializeField] private Authorization _authorization;

    private void Start()
    {
        _authorization.Success += Success;
    }

    private void OnDestroy()
    {
        _authorization.Success -= Success;
    }

    private void Success()
    {
        SceneManager.LoadScene(_menuSceneName);
    }
}
