using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UserInfo : MonoBehaviour
{
    #region Singleton
    public static UserInfo Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public int ID { get; private set; } = 1;
    
    public void SetID(int id)
    {
        ID = id;
    }
}
