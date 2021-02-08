using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomDataTypes;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CharacterController Character;
    [SerializeField] private GameObject EndingSlide;

    [SerializeField] private byte LevelIndex = 0;
    [SerializeField] private LevelData[] LevelData;

    private void Start()
    {
        CharacterController.OnPlayerCompleteLevel += HandleLevelCompleted;
        ChangeLevel();
    }

    private void HandleLevelCompleted()
    {
        LevelData[LevelIndex].LevelParent.SetActive(false);
        LevelIndex++;
        ChangeLevel();
    }

    private void ChangeLevel()
    {
        if (LevelIndex > LevelData.Length - 1)
        {
            Debug.Log("Game Completed");
            EndingSlide.SetActive(true);
        }
        else
        {
            LevelData[LevelIndex].LevelParent.SetActive(true);
            Character.UpdateCurrentLevel(LevelData[LevelIndex]);
        }
    }

    private void OnDestroy()
    {
        CharacterController.OnPlayerCompleteLevel -= HandleLevelCompleted;
    }
}
