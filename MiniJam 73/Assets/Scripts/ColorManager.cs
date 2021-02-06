using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomDataTypes;

public class ColorManager : MonoBehaviour
{
    [Header("Renderers")]
    [SerializeField] private SpriteRenderer[] BackgroundSprites;

    [SerializeField] private Renderer[] NeturalRenderers;
    [SerializeField] private Renderer[] RedRenderers;
    [SerializeField] private Renderer[] GreenRenderers;
    [SerializeField] private Renderer[] BlueRenderers;

    [Header("Color Data")]
    [SerializeField] private Color BackgroundRed;
    [SerializeField] private Color BackgroundGreen;
    [SerializeField] private Color BackgroundBlue;

    [SerializeField] private int ColorIndex;

    private void OnEnable()
    {
        CharacterController.OnPlayerMove += HandlePlayerMoved;
        CharacterController.OnPlayerDeath += HandlePlayerDeath;

        TurnOnNeturalColor();
    }

    private void OnDisable()
    {
        CharacterController.OnPlayerMove -= HandlePlayerMoved;
        CharacterController.OnPlayerDeath -= HandlePlayerDeath;
    }

    private void HandlePlayerMoved()
    {
        ColorIndex++;

        if (ColorIndex > 2)
        {
            ColorIndex = 0;
        }

        UpdateActiveColor();
    }

    private void UpdateActiveColor()
    {
        ChangeBackgroundColor(Color.black);

        foreach (Renderer ren in NeturalRenderers)
        {
            ren.enabled = false;
        }

        foreach (Renderer ren in RedRenderers)
        {
            ren.enabled = ColorIndex == (byte)Colors.Red;

            if (ColorIndex == (byte)Colors.Red)
                ChangeBackgroundColor(BackgroundRed);
        }

        foreach (Renderer ren in BlueRenderers)
        {
            ren.enabled = ColorIndex == (byte)Colors.Blue;

            if (ColorIndex == (byte)Colors.Blue)
            ChangeBackgroundColor(BackgroundBlue);
        }

        foreach (Renderer ren in GreenRenderers)
        {
            ren.enabled = ColorIndex == (byte)Colors.Green;

            if (ColorIndex == (byte)Colors.Green)
                ChangeBackgroundColor(BackgroundGreen);
        }
    }

    private void HandlePlayerDeath()
    {
        ColorIndex = -1;
        TurnOnNeturalColor();
    }

    private void TurnOnNeturalColor()
    {
        ChangeBackgroundColor(Color.white);

        foreach (Renderer ren in RedRenderers)
        {
            ren.enabled = false;
        }

        foreach (Renderer ren in BlueRenderers)
        {
            ren.enabled = false;
        }

        foreach (Renderer ren in GreenRenderers)
        {
            ren.enabled = false;
        }

        foreach (Renderer ren in NeturalRenderers)
        {
            ren.enabled = true;
        }
    }

    private void ChangeBackgroundColor(Color newColor)
    {
        foreach (SpriteRenderer ren in BackgroundSprites)
        {
            ren.color = newColor;
        }
    }
}
