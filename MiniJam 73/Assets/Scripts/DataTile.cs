using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using CustomDataTypes;

[Serializable]
[CreateAssetMenu(fileName = "New Data Tile", menuName = "Tiles/Data Tile")]
public class DataTile : Tile
{
    public CustomTileData TileData;
}
