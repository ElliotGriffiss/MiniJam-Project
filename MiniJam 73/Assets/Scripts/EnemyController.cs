using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomDataTypes;

public class EnemyController : ObjectMover
{
    [Header("Enemy Reference")]
    [SerializeField] private SpriteRenderer Sprite;

    [Header("Enemy Data")]
    [SerializeField] private int CurrentPositionIndex;
    [SerializeField] private Transform[] Locations;

    private void OnEnable()
    {
        CharacterController.OnPlayerMove += HandlePlayerMoved;
        CharacterController.OnPlayerDeath += HandlePlayerDeath;

        SpawnEnemy();
    }

    private void OnDisable()
    {
        CharacterController.OnPlayerMove -= HandlePlayerMoved;
        CharacterController.OnPlayerDeath -= HandlePlayerDeath;
    }

    private void SpawnEnemy()
    {
        if (MovementSequence != null)
        {
            StopCoroutine(MovementSequence);
            MovementSequence = null;
        }

        Sprite.flipX = false;
        CurrentPositionIndex = 0;
        CurrentPosition = Vector3Int.FloorToInt(Locations[CurrentPositionIndex].position);
        MoveableObject.position = CurrentPosition;
    }

    private void HandlePlayerMoved()
    {
        if (CurrentPosition == Locations[CurrentPositionIndex].position)
        {
            CurrentPositionIndex++;

            // ensures the enemy loops around all locations
            if (CurrentPositionIndex > Locations.Length-1)
            {
                CurrentPositionIndex = 0;
            }
        }

        if (MovementSequence == null)
        {
            Vector3Int normalizedMovementVector = GetNormalizedMovementVector(CurrentPosition, Locations[CurrentPositionIndex].position);

            if (normalizedMovementVector == Vector3Int.right)
            {
                Sprite.flipX = false;
            }
            else if (normalizedMovementVector == Vector3Int.left)
            {
                Sprite.flipX = true;
            }

            MovementSequence = MoveObject (CurrentPosition - normalizedMovementVector);
            StartCoroutine(MovementSequence);
        }
    }

    protected override IEnumerator MoveObject(Vector3Int newPosition)
    {
        float normalizedTime = 0f;

        while (normalizedTime < 1)
        {
            normalizedTime += Time.deltaTime / MovementDuration;

            Vector3 DesiredPosition = Vector3.Lerp(CurrentPosition, newPosition, normalizedTime);
            MoveableObject.position = DesiredPosition;

            yield return new WaitForEndOfFrame();
        }

        CurrentPosition = newPosition;
        ObjectDirection = Direction.None;
        MovementSequence = null;
    }

    private void HandlePlayerDeath()
    {
        SpawnEnemy();
    }
}
