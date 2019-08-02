using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizeSetter : MonoBehaviour
{
    public TileMap tileMap;

    void Start()
    {
        GetComponent<Camera>().orthographicSize = tileMap.mapSize * 1.5f / 2;
    }
}
