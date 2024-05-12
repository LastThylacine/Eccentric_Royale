using UnityEngine;

public class Spawner : MonoBehaviour
{
    private CardsInGame _cardsInGame;

    private void Start()
    {
        _cardsInGame = CardsInGame.Instance;
    }


    public void Spawn(string id, Vector3 spawnPoint, bool isEnemy)
    {
        Unit unitPrefab;
        Quaternion rotation = Quaternion.identity;

        if (isEnemy)
        {
            unitPrefab = _cardsInGame.EnemyDeck[id].Unit;
            spawnPoint *= -1;
            rotation = Quaternion.Euler(0, 180, 0);
        }
        else
            unitPrefab = _cardsInGame.PlayerDeck[id].Unit;

        Unit unit = Instantiate(unitPrefab, spawnPoint, rotation);

        unit.Init(isEnemy);
        MapInfo.Instance.AddUnit(unit);
    }
}
