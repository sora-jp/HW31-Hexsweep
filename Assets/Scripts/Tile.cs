using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileMap map;

    public Vector2Int tileCoords; // Position på spelbrädet i Q-R koordinater

    public TextMeshPro text;       // Texten på tilen
    public SpriteRenderer baseImage;     // Fyllningen
    public SpriteRenderer outlineImage;  // Outlinen
    public SpriteRenderer flagMarker;    // Markör om vi har en flagga på oss
    
    public Color hoverColor;    // Färgen när musen är över oss
    
    public bool isBomb;  // Är vi en bomb?
    public bool hasFlag; // Har spelaren satt en flagga på oss
    public bool isRevealed; // Har vi klickat på tilen

    public int neighbourBombCount; // Hur många bomber det finns runt oss

    Color baseColor;    // Egentliga färg

    void Start()
    {
        outlineImage.enabled = false;   // Ingen outline när spelet startar
        baseColor = baseImage.color;    // Spara våran egentliga färg så vi kan ta tbx den senare
    }

    void Update()
    {
        text.enabled = !isBomb && isRevealed && neighbourBombCount > 0;
        text.SetText(isBomb ? "B" : neighbourBombCount.ToString());
    }

    /// <summary>
    /// Körs när spelarens mus kommer över oss eller lämnar oss
    /// </summary>
    /// <param name="hovering">Är spelarens mus över oss just nu?</param>
    public void SetHoverState(bool hovering)
    {
        if (isRevealed) return;
        outlineImage.enabled = hovering && !isRevealed;
        baseImage.color = hovering || isRevealed ? hoverColor : baseColor;
    }

    public void SetTileRevealed()
    {
        SetHoverState(false);
        outlineImage.enabled = false;
        baseImage.color = isBomb ? Color.red : hoverColor;
        isRevealed = true;
    }

    /// <summary>
    /// Revealar våran tile
    /// </summary>
    public void RevealTile(bool recurse = false, bool triggeredByClick = false)
    {
        if (isRevealed) return;
        if (hasFlag && triggeredByClick) return;
        if (isBomb && triggeredByClick)
        {
            GameController.Instance.PlayerClickBombYeet();
            return;
        }
        SetTileRevealed();
        if (recurse && neighbourBombCount == 0)
        {
            foreach (var tile in map.GetNeighbours(tileCoords))
            {
                tile.RevealTile(true, triggeredByClick);
            }
        }
    }

    public void ToggleFlagState()
    {
        if (isRevealed) return;
        hasFlag = !hasFlag;

        if (hasFlag)
        {
            flagMarker.enabled = true;
        }
        else
        {
            flagMarker.enabled = false;
        }
    }

    /// <summary>
    /// Sätter våran världsposition givet kubcoordinater
    /// </summary>
    /// <param name="q">Q-värde</param>
    /// <param name="r">R-värde</param>
    public void SetPosition(int q, int r)
    {
        // Källa: Redblob games, Hex coordinates
        var x = Mathf.Sqrt(3) * q + (Mathf.Sqrt(3) / 2) * r;
        var y = 1.5f * r;
        transform.position = new Vector2(x, y) * 0.5f;
        tileCoords = new Vector2Int(q, r);
    }
}
