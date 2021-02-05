using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomDataTypes;

public class ObjectMover : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] protected Transform MoveableObject;
    [SerializeField] protected Direction ObjectDirection;

    [Header("Object Data")]
    [SerializeField] protected const float MovementDuration = 0.5f;

    protected Vector3Int CurrentPosition;
    protected IEnumerator MovementSequence;


    protected virtual IEnumerator MoveObject(Vector3Int newPosition)
    {
        // I created this but forgot inheriting iEnumerators is awakward, oh well.
        yield return null;
    }

    /// <summary>
    /// Returns a movement vector based on an enum state.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    protected Vector3Int GetMovementVectorFromDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                return Vector3Int.left;

            case Direction.Right:
                return Vector3Int.right;

            case Direction.Up:
                return Vector3Int.up;

            case Direction.Down:
                return Vector3Int.down;

            default:
                Debug.LogError("This should never be a case");
                return Vector3Int.zero;
        }
    }

    /// <summary>
    /// Works out the normalized movement vector from the current position and target location.
    /// It's basically used to ensure the enemy moves only one tile each move, instead of directly to the target location
    /// </summary>
    /// <param name="currentPosition"></param>
    /// <param name="targetLocation"></param>
    /// <returns></returns>
    protected Vector3Int GetNormalizedMovementVector(Vector3Int currentPosition, Vector3 targetLocation)
    {
        return Vector3Int.FloorToInt((currentPosition + targetLocation).normalized);
    }
}
