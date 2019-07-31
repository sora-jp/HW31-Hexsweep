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
    public Transform flagMarker;
    
    public Color hoverColor, revealColor, revealBombColor;    // Färgen när musen är över oss
    
    public bool isBomb;  // Är vi en bomb?
    public bool hasFlag; // Har spelaren satt en flagga på oss
    public bool isRevealed; // Har vi klickat på tilen

    public int neighbourBombCount; // Hur många bomber det finns runt oss

    Color baseColor;    // Egentliga färg

    void Start()
    {
        outlineImage.transform.localScale = Vector3.one * 0.85f;   // Ingen outline när spelet startar
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
    public void SetHoverState(bool hovering, bool animateBase = true)
    {
        outlineImage?.transform?.AnimateScale(hovering ? Vector3.one * 2f : Vector3.one * 0.85f, hovering ? 0.15f : 0.1f);
        if (isRevealed) return;
        if (animateBase) baseImage.AnimateColor(hovering ? hoverColor : baseColor, 0.1f);
    }

    public void ToggleFlagState()
    {
        if (isRevealed) return;
        hasFlag = !hasFlag;
        float dur = 0.25f;
        if (hasFlag)
        {
            flagMarker.AnimateScale(Vector3.one * 0.45f, dur);
        }
        else
        {
            flagMarker.AnimateScale(Vector3.zero, dur);
        }
    }

    public void SetTileRevealed()
    {
        //SetHoverState(false, false);
        //outlineImage.enabled = false;
        baseImage.AnimateColor(isBomb ? revealBombColor : revealColor, isBomb ? 0.5f : 0.15f);
        isRevealed = true;
    }

    /// <summary>
    /// Revealar våran tile
    /// </summary>
    public void RevealTile(bool recurse = false, bool triggeredByClick = false)
    {
        if (isRevealed || (hasFlag && triggeredByClick)) return;
        if (isBomb && triggeredByClick)
        {
            GameController.Instance.PlayerClickBombYeet();
            return;
        }
        SetTileRevealed();

        if (recurse && neighbourBombCount == 0)
        {
            StartCoroutine(_RevealNeighbours());
        }
    }

    IEnumerator _RevealNeighbours()
    {
        foreach (var tile in map.GetNeighbours(tileCoords))
        {
            yield return new WaitForSeconds(0.035f);
            tile.RevealTile(true, true);
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
