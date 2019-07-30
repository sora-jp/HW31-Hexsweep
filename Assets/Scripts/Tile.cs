using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Vector2Int tileCoords; // Position på spelbrädet i Q-R koordinater

    public Image baseImage;     // Fyllningen
    public Image outlineImage;  // Outlinen

    public Color hoverColor;    // Färgen när musen är över oss

    public bool isBomb;  // Är vi en bomb?
    public bool hasFlag; // Har spelaren satt en flagga på oss

    Color baseColor;    // Egentliga färg

    void Start()
    {
        outlineImage.enabled = false;   // Ingen outline när spelet startar
        baseColor = baseImage.color;    // Spara våran egentliga färg så vi kan ta tbx den senare
    }

    /// <summary>
    /// Körs när spelarens mus kommer över oss eller lämnar oss
    /// </summary>
    /// <param name="hovering">Är spelarens mus över oss just nu?</param>
    public void SetHoverState(bool hovering)
    {
        outlineImage.enabled = hovering;
        baseImage.color = hovering ? hoverColor : baseColor;
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
    }
}
