using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomDataTypes;

public class CharacterController : ObjectMover
{
    public static event Action OnPlayerMove = delegate { };

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
            CharacterController.OnPlayerMove();
            MovementSequence = MoveObject(GetMovementVectorFromDirection(ObjectDirection) + Vector3Int.FloorToInt(MoveableObject.position));
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
