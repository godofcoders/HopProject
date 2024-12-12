using UnityEngine;
using System.Collections.Generic;
using Tiles.Pool;

public class TilePool : MonoBehaviour, ITilePool
{
    [Header("Tile Pool Settings")]
    public GameObject tilePrefab;
    public int poolSize = 20;

    private Queue<GameObject> tileQueue;

    void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        tileQueue = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject tile = Instantiate(tilePrefab);
            tile.SetActive(false);
            tileQueue.Enqueue(tile);
        }
    }

    public GameObject GetTile()
    {
        if (tileQueue.Count > 0)
        {
            GameObject tile = tileQueue.Dequeue();
            tile.SetActive(true);
            return tile;
        }
        else
        {
            Debug.LogWarning("Tile Pool exhausted, consider increasing pool size.");
            return null;
        }
    }

    public void ReturnTile(GameObject tile)
    {
        tile.SetActive(false);
        tileQueue.Enqueue(tile);
    }
}