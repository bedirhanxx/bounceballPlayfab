using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public int numberToSpawn;
    public List<GameObject> spawnPool;
    public Transform CoinHolder;
    void Start()
    {
        spawnObjects();
    }
    
    public void spawnObjects()
    {
        int randomItem = 0;
        GameObject toSpawn;
        float screenX, screenY;
        Vector2 pos;
        for (int i = 0; i < numberToSpawn; i++)
        {
            randomItem = Random.Range(0,spawnPool.Count);
            toSpawn = spawnPool[randomItem];

            screenX = Random.Range(-1f,1f);
            screenY = Random.Range(15f ,30f);

            pos = new Vector2(screenX, screenY);

            GameObject Coins = Instantiate(toSpawn, pos, toSpawn.transform.rotation);
            Coins.transform.position = new Vector3(Coins.transform.position.x, Coins.transform.position.y, -2f);
            Coins.transform.SetParent(CoinHolder.transform);

        }
    }
}
