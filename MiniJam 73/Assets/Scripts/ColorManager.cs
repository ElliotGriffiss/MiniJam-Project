using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomDataTypes;

public class ColorManager : MonoBehaviour
{
    [Header("Renderers")]
    [SerializeField] private Camera MainCamera;

    [SerializeField] private Renderer[] NeturalRenderers;
    [SerializeField] private Renderer[] RedRenderers;
    [SerializeField] private Renderer[] GreenRenderers;
    [SerializeField] private Renderer[] BlueRenderers;

    [Header("Color Data")]
    [SerializeField] private Color CameraBackgroundRed;
    [SerializeField] private Color CameraBackgroundGreen;
    [SerializeField] private Color CameraBackgroundBlue;

    [SerializeField] private int ColorIndex;

    private void Start()
    {
        CharacterController.OnPlayerMove += HandlePlayerMoved;
        CharacterController.OnPlayerDeath += HandlePlayerDeath;

        TurnoNeturalColor();
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
        MainCamera.backgroundColor = Color.black;

        foreach (Renderer ren in NeturalRenderers)
        {
            ren.enabled = false;
        }

        foreach (Renderer ren in RedRenderers)
        {
            ren.enabled = ColorIndex == (byte)Colors.Red;

            if (ColorIndex == (byte)Colors.Red)
                MainCamera.backgroundColor = CameraBackgroundRed;
        }

        foreach (Renderer ren in BlueRenderers)
        {
            ren.enabled = ColorIndex == (byte)Colors.Blue;

            if (ColorIndex == (byte)Colors.Blue)
             MainCamera.backgroundColor = CameraBackgroundBlue;
        }

        foreach (Renderer ren in GreenRenderers)
        {
            ren.enabled = ColorIndex == (byte)Colors.Green;

            if (ColorIndex == (byte)Colors.Green)
                MainCamera.backgroundColor = CameraBackgroundGreen;
        }
    }

    private void HandlePlayerDeath()
    {
        ColorIndex = -1;
        TurnoNeturalColor();
    }

    private void TurnoNeturalColor()
    {
        MainCamera.backgroundColor = Color.white;

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
}
