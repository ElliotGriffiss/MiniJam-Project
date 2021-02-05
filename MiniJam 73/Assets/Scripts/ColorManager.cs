using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomDataTypes;

public class ColorManager : MonoBehaviour
{
    [Header("Renderers")]
    [SerializeField] private Renderer[] RedRenderers;
    [SerializeField] private Renderer[] BlueRenderers;
    [SerializeField] private Renderer[] GreenRenderers;

    [Header("Color Data")]
    [SerializeField] private int ColorIndex;

    private void Start()
    {
        CharacterController.OnPlayerMove += HandlePlayerMoved;
        CharacterController.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDestroy()
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
        foreach (Renderer ren in RedRenderers)
        {
            ren.enabled = ColorIndex == (byte)Colors.Red;
        }

        foreach (Renderer ren in BlueRenderers)
        {
            ren.enabled = ColorIndex == (byte)Colors.Green;
        }

        foreach (Renderer ren in GreenRenderers)
        {
            ren.enabled = ColorIndex == (byte)Colors.Blue;
        }
    }

    private void HandlePlayerDeath()
    {
        ColorIndex = -1;
        TurnOnAllColors();
    }

    private void TurnOnAllColors()
    {
        foreach (Renderer ren in RedRenderers)
        {
            ren.enabled = true;
        }

        foreach (Renderer ren in BlueRenderers)
        {
            ren.enabled = true;
        }

        foreach (Renderer ren in GreenRenderers)
        {
            ren.enabled = true;
        }
    }
}
