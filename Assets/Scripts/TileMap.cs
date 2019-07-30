using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileMap : MonoBehaviour
{
    public Tile tilePrefab; // Skapar en tile
    public int mapSize; // Storleken på mapen
    [Range(0, 1)]
    public float bombChance;

    Tile currentHoverTile = null;
    Dictionary<Vector2Int, Tile> tileMap = new Dictionary<Vector2Int, Tile>();

    int totalNumBombs;
    int totalNumFlags;
    int totalNumFlaggedBombs;
    bool updateTiles = true;

    void Start()
    {
        GameController.OnGameOver += RevealBombsAndDisable;
        GameController.OnWin += RevealBombsAndDisable;

        // TODO: Poisson disc sampling? with a random size per disc ofc
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

    private void OnDestroy()
    {
        GameController.OnGameOver -= RevealBombsAndDisable;
        GameController.OnWin -= RevealBombsAndDisable;
    }

    void RevealBombsAndDisable()
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
        if (Input.GetMouseButtonDown(1) && currentHoverTile != null && (totalNumFlags < totalNumBombs || currentHoverTile.hasFlag))
        {
            currentHoverTile.ToggleFlagState();
            UpdateFlagParams();
        }
    }

    void UpdateFlagParams()
    {
        totalNumFlags = TilesWhere(t => t.hasFlag).Count();
        totalNumFlaggedBombs = TilesWhere(t => t.hasFlag && t.isBomb).Count();
        if (totalNumBombs == totalNumFlaggedBombs && totalNumBombs == totalNumFlags) GameController.Instance.PlayerWinYeet();
    }

    private IEnumerable<Tile> TilesWhere(System.Func<Tile, bool> pred) => tileMap.Select(t => t.Value).Where(pred);

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
        if (Random.value < bombChance)
        {
            totalNumBombs++;
            newTile.isBomb = true;
        }
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
