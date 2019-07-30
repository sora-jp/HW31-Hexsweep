using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public Tile tilePrefab; // Skapar en tile
    public int mapSize; // Storleken på mapen
    [Range(0, 1)]
    public float bombChance;

    Tile currentHoverTile = null;
    Dictionary<Vector2Int, Tile> tileMap = new Dictionary<Vector2Int, Tile>();

    int totalNumBombs;
    bool updateTiles = true;

    void Start()
    {
        GameController.OnGameOver += OnGameOver;

        for (int i = -mapSize + 1; i < mapSize; i++)
        {
            for (int j = -mapSize + 1; j < mapSize; j++)
            {
                // Variabler
                int q = i, r = j, s = -(q + r);

                // Om den största koordinaten är störren än mapSize, skippa tilen
                if (Mathf.Max(
                        Mathf.Abs(q), 
                        Mathf.Abs(r), 
                        Mathf.Abs(s)
                    ) > mapSize - 1) continue;

                // Skapa tilen
                SpawnTile(i, j);
            }
        }

        foreach (var kvp in tileMap)
        {
            var tile = kvp.Value;
            tile.neighbourBombCount = GetNeighbours(kvp.Key).Where(t => t.isBomb).Count();
        }
    }

    void OnGameOver()
    {
        updateTiles = false;
        foreach (var tile in tileMap.Select(kvp => kvp.Value))
        {
            tile.SetHoverState(false);
            if (tile.isBomb)
                tile.RevealTile();
        }
    }

    void Update()
    {
        if (!updateTiles) return;
        Vector2 mouseCoords = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var x = Mathf.RoundToInt((Mathf.Sqrt(3) / 3 * mouseCoords.x - 1.0f / 3 * mouseCoords.y) * 2);
        var y = Mathf.RoundToInt((2.0f / 3 * mouseCoords.y) * 2);
        if (currentHoverTile?.tileCoords != new Vector2Int(x, y))
        {
            currentHoverTile?.SetHoverState(false);
            currentHoverTile = GetTile(x, y);
            currentHoverTile?.SetHoverState(true);
        }
        if (Input.GetMouseButtonDown(0) && currentHoverTile != null)
        {
            currentHoverTile.RevealTile(true, true);
        }
    }

    public IEnumerable<Tile> GetNeighbours(Vector2Int coord)
    {
        var neighbours = new List<Tile>();
        neighbours.Add(GetTile(coord + new Vector2Int(1, 0)));
        neighbours.Add(GetTile(coord + new Vector2Int(-1, 0)));
        neighbours.Add(GetTile(coord + new Vector2Int(1, -1)));
        neighbours.Add(GetTile(coord + new Vector2Int(0, 1)));
        neighbours.Add(GetTile(coord + new Vector2Int(0, -1)));
        neighbours.Add(GetTile(coord + new Vector2Int(-1, 1)));
        return neighbours.Where(n => n != null);
    }

    /// <summary>
    /// Skapar en tile med en viss position
    /// </summary>
    /// <param name="q">Q-värde</param>
    /// <param name="r">R-värde</param>
    void SpawnTile(int q, int r)
    {
        Tile newTile = Instantiate(tilePrefab);
        if (Random.value < bombChance) newTile.isBomb = true;
        newTile.map = this;
        newTile.SetPosition(q, r);
        tileMap[newTile.tileCoords] = newTile;
    }

    Tile GetTile(Vector2Int coords) => GetTile(coords.x, coords.y);

    /// <summary>
    /// Hämta en tile
    /// </summary>
    /// <param name="q">Q-värde</param>
    /// <param name="r">R-värde</param>
    /// <returns>Tile med positionen (q, r), eller null om platsen är tom</returns>
    Tile GetTile(int q, int r) =>
        tileMap.ContainsKey(new Vector2Int(q, r)) ? tileMap[new Vector2Int(q, r)] : null;
}
