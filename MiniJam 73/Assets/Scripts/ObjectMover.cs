using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomDataTypes;

public class ObjectMover : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] protected GameObject Object;
    [SerializeField] protected Direction ObjectDirection;

    [Header("Object Data")]
    [SerializeField] protected const float MovementDuration = 0.5f;

    protected Vector3Int CurrentPosition;
    protected IEnumerator MovementSequence;


    protected virtual IEnumerator MoveObject(Vector3Int newPosition)
    {
        yield return null;
    }

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
}
