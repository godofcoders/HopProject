using UnityEngine;
using System.Collections.Generic;
using Tiles.Pool;

public class TileSpawner : MonoBehaviour
{
    [Header("Tile Spawn Settings")]
    public float minX = -3f;               // Minimum X position for tile spawning
    public float maxX = 3f;                // Maximum X position for tile spawning
    public float tileDistance = 2f;        // Distance between tiles
    public Transform player;               // Reference to the player (ball)
    public Camera mainCamera;              // Reference to the main camera

    private ITilePool tilePool;            // Tile pool interface
    private List<GameObject> activeTiles;  // List of currently active tiles
    private Vector3 lastTilePosition;      // Last tile's position
    private float despawnZLimit;           // Z-position to despawn tiles

    private bool isFirstTileSpawned = false; // Flag to ensure the first tile spawns at (0,0,0)

    void Start()
    {
        // Fetch the tile pool implementation
        tilePool = GetComponent<ITilePool>();
        if (tilePool == null)
        {
            Debug.LogError("TileSpawner requires a component implementing ITilePool.");
            return;
        }

        activeTiles = new List<GameObject>();
        lastTilePosition = Vector3.zero; // Start spawning at (0,0,0)

        // Spawn initial tiles based on camera frustum
        SpawnInitialTiles();
    }

    void Update()
    {
        // Continuously check for spawning and despawning tiles
        HandleTileSpawning();
        HandleTileDespawning();
    }

    void SpawnInitialTiles()
    {
        // Calculate how many tiles are visible in the camera's frustum
        float visibleRange = CalculateVisibleZRange();
        int tileCount = Mathf.CeilToInt(visibleRange / tileDistance);

        for (int i = 0; i < tileCount; i++)
        {
            SpawnTile();
        }
    }

    void HandleTileSpawning()
    {
        // Check if the player has moved past the last spawned tile
        if (player.position.z + tileDistance > lastTilePosition.z)
        {
            SpawnTile();
        }
    }

    void SpawnTile()
    {
        GameObject tile = tilePool.GetTile();
        if (tile == null) return;

        Vector3 nextTilePosition;

        if (!isFirstTileSpawned)
        {
            // First tile always at (0,0,0)
            nextTilePosition = Vector3.zero;
            isFirstTileSpawned = true;
        }
        else
        {
            // Randomize X-axis position and calculate Z-axis for subsequent tiles
            float nextX = Random.Range(minX, maxX);
            nextTilePosition = new Vector3(
                nextX,
                lastTilePosition.y,
                lastTilePosition.z + tileDistance
            );
        }

        // Set tile position and activate it
        tile.transform.position = nextTilePosition;
        tile.SetActive(true);
        activeTiles.Add(tile);

        lastTilePosition = nextTilePosition;
    }

    void HandleTileDespawning()
    {
        // Calculate the Z-position behind which tiles should be despawned
        despawnZLimit = mainCamera.transform.position.z - CalculateVisibleZRange();

        // Check and despawn tiles behind the despawn limit
        if (activeTiles.Count > 0)
        {
            GameObject firstTile = activeTiles[0];
            if (firstTile.transform.position.z < despawnZLimit)
            {
                DespawnTile(firstTile);
            }
        }
    }

    void DespawnTile(GameObject tile)
    {
        tilePool.ReturnTile(tile);
        activeTiles.RemoveAt(0);
    }
    public void ResetTiles()
    {
        // Despawn all active tiles and return them to the pool
        foreach (var tile in activeTiles)
        {
            tilePool.ReturnTile(tile);
            tile.SetActive(false);
        }

        activeTiles.Clear();

        // Reset tile spawning parameters
        lastTilePosition = Vector3.zero;
        isFirstTileSpawned = false;

        // Spawn initial tiles again
        SpawnInitialTiles();
    }


    float CalculateVisibleZRange()
    {
        // Calculate how far the camera can see in the Z-direction
        float halfHeight = mainCamera.orthographicSize;
        float visibleZ = halfHeight * 2f;
        return visibleZ;
    }
}
