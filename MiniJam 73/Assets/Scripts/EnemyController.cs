using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomDataTypes;

public class EnemyController : ObjectMover
{
    [Header("Enemy Data")]
    [SerializeField] private int CurrentPositionIndex;
    [SerializeField] private Transform[] Locations;

    private void OnEnable()
    {
        CharacterController.OnPlayerMove += HandlePlayerMoved;

        MoveableObject.position = Locations[CurrentPositionIndex].position;
        CurrentPosition = Vector3Int.FloorToInt(Locations[CurrentPositionIndex].position);
    }

    private void OnDisable()
    {
        CharacterController.OnPlayerMove -= HandlePlayerMoved;
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
            MovementSequence = MoveObject (CurrentPosition -(GetNormalizedMovementVector(CurrentPosition, Locations[CurrentPositionIndex].position)));
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
            // Vector3 PixelPerfectPosition = new Vector3(Utilities.RoundToTheNearestPixel(DesiredPosition.x), Utilities.RoundToTheNearestPixel(DesiredPosition.y), DesiredPosition.z);
            MoveableObject.position = DesiredPosition;

            yield return new WaitForEndOfFrame();
        }

        CurrentPosition = newPosition;
        ObjectDirection = Direction.None;
        MovementSequence = null;
    }
}
