using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using CustomDataTypes;

public class CharacterController : ObjectMover
{
    public static event Action OnPlayerMove = delegate { };
    public static event Action OnPlayerDeath = delegate { };
    public static event Action OnNeutralTriggerActivated = delegate { };
    public static event Action OnPlayerCompleteLevel = delegate { };

    [Header("Player References")]
    [SerializeField] private Animator PlayerAnimator;

    private LevelData LevelData;
    private IEnumerator DeathSequence;
    private bool PlayerHasControl = false;

    public void UpdateCurrentLevel(LevelData levelData)
    {
        LevelData = levelData;

        CurrentPosition = LevelData.SpawnPoint;
        MoveableObject.position = CurrentPosition;
        PlayerAnimator.SetBool("PlayerDead", false);
        PlayerHasControl = true;

        // This just makes sure everything is reset properly when a level loads.
        CharacterController.OnPlayerDeath();
    }

    private void Update()
    {
        if (MovementSequence == null && PlayerHasControl)
        {
            CheckForPlayerInput();
        }
    }

    private void CheckForPlayerInput()
    {
        if (Input.GetAxis("Horizontal") < 0)
        {
            ObjectDirection = Direction.Left;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            ObjectDirection = Direction.Right;
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            ObjectDirection = Direction.Up;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            ObjectDirection = Direction.Down;
        }

        if (ObjectDirection != Direction.None)
        {
            Vector3Int newPosition = GetMovementVectorFromDirection(ObjectDirection) + CurrentPosition;

            if (CheckForCollision(newPosition))
            {
                ObjectDirection = Direction.None;
            }
            else
            {
                MovementSequence = MoveObject(newPosition);
                StartCoroutine(MovementSequence);
                CharacterController.OnPlayerMove();
            }
        }
    }

    /// <summary>
    /// Checks to see if the player is goiing to collide with a wall.
    /// </summary>
    /// <param name="newPosition"></param>
    /// <returns></returns>
    private bool CheckForCollision(Vector3Int newPosition)
    {
        TileBase baseTile = LevelData.WallMap.GetTile(newPosition);
        DataTile customDataTile = baseTile as DataTile;

        if (customDataTile != null)
        {
            if (customDataTile.TileData == CustomTileData.Block_Movement)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks for death against Enemy Locations.
    /// </summary>
    /// <param name="newPosition"></param>
    /// <returns></returns>
    private void CheckForDeathAgainstEnemies()
    {
        // For enemie detection.
        foreach (Transform transform in LevelData.Enemies)
        {
            if (Vector3.Distance(transform.position, MoveableObject.position) < 0.1f)
            {
                TriggerPlayerDeath();
            }
        }
    }

    /// <summary>
    /// Checks for death against Tilemap Data.
    /// </summary>
    /// <param name="newPosition"></param>
    /// <returns></returns>
    private bool CheckForCollionAgainstTileData(Vector3Int newPosition)
    {
        TileBase baseTile = LevelData.NeutralMap.GetTile(newPosition);
        DataTile customDataTile = baseTile as DataTile;

        if (customDataTile != null)
        {
            if (customDataTile.TileData == CustomTileData.Kill_Player)
            {
                TriggerPlayerDeath();
                return true;
            }
            else if (customDataTile.TileData == CustomTileData.Set_Neutral)
            {
                CharacterController.OnNeutralTriggerActivated();
            }
            else if (customDataTile.TileData == CustomTileData.Level_End)
            {
                CharacterController.OnPlayerCompleteLevel();
            }
            else if (customDataTile.TileData == CustomTileData.Display_White_Text)
            {
                PlayerHasControl = false;
                StartCoroutine(DelayNextLevel());
            }
        }

        return false;
    }

    private void TriggerPlayerDeath()
    {
        if (MovementSequence != null)
        {
            StopCoroutine(MovementSequence);
            MovementSequence = null;
        }

        if (DeathSequence == null)
        {
            DeathSequence = RunDeathSequence();
            StartCoroutine(DeathSequence);
        }
    }

    private IEnumerator RunDeathSequence()
    {
        PlayerHasControl = false;
        ObjectDirection = Direction.None;
        PlayerAnimator.SetBool("PlayerDead", true);
        yield return new WaitForSeconds(1f);

        CharacterController.OnPlayerDeath();
        CurrentPosition = LevelData.SpawnPoint;
        MoveableObject.position = CurrentPosition;
        PlayerAnimator.SetBool("PlayerDead", false);
        DeathSequence = null;
        PlayerHasControl = true;
    }

    private IEnumerator DelayNextLevel()
    {
        Debug.Log("Delay");
        CharacterController.OnNeutralTriggerActivated();
        yield return new WaitForSeconds(2f);
        CharacterController.OnPlayerCompleteLevel();

    }

    protected override IEnumerator MoveObject(Vector3Int newPosition)
    {
        float normalizedTime = 0f;

        while (normalizedTime < 1)
        {
            normalizedTime += Time.deltaTime / MovementDuration;

            Vector3 DesiredPosition = Vector3.Lerp(CurrentPosition, newPosition, normalizedTime);
            MoveableObject.position = DesiredPosition;
            CheckForDeathAgainstEnemies();

            yield return new WaitForEndOfFrame();
        }

        CurrentPosition = newPosition;
        ObjectDirection = Direction.None;

        CheckForCollionAgainstTileData(newPosition);
        MovementSequence = null;
    }
}
