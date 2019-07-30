using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public Tile tilePrefab; // Skapar en tile
    public int mapSize; // Storleken på mapen

    Tile currentHoverTile = null;
    Dictionary<Vector2Int, Tile> tileMap = new Dictionary<Vector2Int, Tile>();

    void Start()
    {
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
    }

    void Update()
    {
        /*
         var q = (sqrt(3)/3 * point.x  -  1./3 * point.y) / size
         var r = (                        2./3 * point.y) / size
        */
        Vector2 mouseCoords = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var x = Mathf.Sqrt(3) / 3 * mouseCoords.x - 1.0f / 3 * mouseCoords.y;
        var y = 2.0f / 3 * mouseCoords.y;
    }

    /// <summary>
    /// Skapar en tile med en viss position
    /// </summary>
    /// <param name="q">Q-värde</param>
    /// <param name="r">R-värde</param>
    void SpawnTile(int q, int r)
    {
        Tile newTile = Instantiate(tilePrefab);
        newTile.SetPosition(q, r);
    }
}
