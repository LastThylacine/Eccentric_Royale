using UnityEngine;

public class Spawner : MonoBehaviour
{

    public void Spawn(string id, Vector3 spawnPoint, bool isEnemy)
    {
        Unit unitPrefab;
        Quaternion rotation = Quaternion.identity;

        if (isEnemy)
        {
            unitPrefab = CardsInGame.Instance.EnemyDeck[id].Unit;
            rotation = Quaternion.Euler(0, 180, 0);
        }
        else
            unitPrefab = CardsInGame.Instance.PlayerDeck[id].Unit;

        Unit unit = Instantiate(unitPrefab, spawnPoint, rotation);

        unit.Init(isEnemy);
        MapInfo.Instance.AddUnit(unit);
    }
}
