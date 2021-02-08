using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CustomDataTypes
{
    [Serializable]
    public struct LevelData
    {
        public GameObject LevelParent;
        public Vector3Int SpawnPoint;
        [Space]
        public Tilemap WallMap;
        public Tilemap SpikeMap;
        public Tilemap NeutralMap;
        [Space]
        public Transform[] Enemies;
    }

    public enum CustomTileData : byte
    {
        Block_Movement,
        Kill_Player,
        Set_Neutral,
        Level_End,
        Display_White_Text,
    }

    public enum Direction : byte
    {
        None,
        Left,
        Right,
        Up,
        Down,
    }

    public enum Colors : int
    {
        Red,
        Green,
        Blue,
    }
}
