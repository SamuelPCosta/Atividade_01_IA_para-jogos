using UnityEngine;
using System.Collections.Generic;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject lifePrefab;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private int numBullets;
    [SerializeField] private int numLifes;
    [SerializeField] private int numEnemies;

    [SerializeField] private float maxX;
    [SerializeField] private float maxY;

    private List<Vector2> usedPositions = new List<Vector2>();
    private float minDistance = 0.5f;

    void Start()
    {
        numBullets = Random.Range(1, numBullets + 1);
        numLifes = Random.Range(1, numLifes + 1);
        numEnemies = Random.Range(1, numEnemies + 1);

        SpawnObjects(bulletPrefab, numBullets);
        SpawnObjects(lifePrefab, numLifes);
        SpawnObjects(enemyPrefab, numEnemies);
    }

    void SpawnObjects(GameObject prefab, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            float x = RandomEvenPosition(maxX);
            float y = RandomEvenPosition(maxY);
            Vector2 localPos = new Vector2(x, y);

            if (!PositionOccupied(localPos))
            {
                GameObject obj = Instantiate(prefab, transform);
                obj.transform.localPosition = localPos;
                usedPositions.Add(localPos);
            }
        }
    }

    float RandomEvenPosition(float max)
    {
        float val = Random.Range(-max, max);
        int rounded = Mathf.RoundToInt(val);
        if (rounded % 2 != 0) rounded += (rounded < max ? 1 : -1);
        return rounded;
    }

    bool PositionOccupied(Vector2 localPos)
    {
        foreach (var used in usedPositions)
        {
            if (Vector2.Distance(localPos, used) < minDistance) return true;
        }
        return false;
    }
}
