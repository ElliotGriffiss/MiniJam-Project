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

    [Header("Player Data")]
    [SerializeField] private Tilemap Tilemap;
    private Vector3Int SpawnPoint;

    private void OnEnable()
    {
        SpawnPoint = Vector3Int.FloorToInt(MoveableObject.position);
        CurrentPosition = SpawnPoint;
    }

    private void Update()
    {
        if (MovementSequence == null)
        {
            CheckForPlayerInput();
        }
    }

    private void CheckForPlayerInput()
    {
        // check against input axis and direction to prevent 180 degree turns
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
        TileBase baseTile = Tilemap.GetTile(newPosition);
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

    private bool CheckForDeath(Vector3Int newPosition)
    {
        TileBase baseTile = Tilemap.GetTile(newPosition);
        DataTile customDataTile = baseTile as DataTile;

        if (customDataTile != null)
        {
            if (customDataTile.TileData == CustomTileData.Kill_Player)
            {
                CharacterController.OnPlayerDeath();
                MoveableObject.position = SpawnPoint;
                CurrentPosition = SpawnPoint;
                return true;
            }
        }

        return false;
    }

    protected override IEnumerator MoveObject(Vector3Int newPosition)
    {
        float normalizedTime = 0f;

        while (normalizedTime < 1)
        {
            normalizedTime += Time.deltaTime / MovementDuration;

            Vector3 DesiredPosition = Vector3.Lerp(CurrentPosition, newPosition, normalizedTime);
            // Vector3 PixelPerfectPosition = new Vector3(Utilities.RoundToTheNearestPixel(DesiredPosition.x), Utilities.RoundToTheNearestPixel(DesiredPosition.y), DesiredPosition.z);
            MoveableObject.position = DesiredPosition;

            yield return new WaitForEndOfFrame();
        }

        CurrentPosition = newPosition;
        ObjectDirection = Direction.None;
        MovementSequence = null;

        CheckForDeath(newPosition);
    }
}
