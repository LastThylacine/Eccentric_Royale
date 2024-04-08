using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
    #region SingletonOneScene
    public static MapInfo Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
    #endregion

    [SerializeField] private List<Transform> _enemyTowers = new List<Transform>();
    [SerializeField] private List<Transform> _playerTowers = new List<Transform>();

    public Vector3 GetNearestTowerPosition(in Vector3 currentPosition, bool enemy)
    {
        List<Transform> towers = enemy ? _enemyTowers : _playerTowers;

        Vector3 nearestTowerPosition = towers[0].position;
        float distance = Vector3.Distance(currentPosition, towers[0].position);

        for (int i = 1; i < towers.Count; i++)
        {
            float tempDistance = Vector3.Distance(currentPosition, towers[i].position);
            if (tempDistance > distance) continue;

            nearestTowerPosition = towers[i].position;
            distance = tempDistance;                                                                                                        
        }

        return nearestTowerPosition;
    }
}
