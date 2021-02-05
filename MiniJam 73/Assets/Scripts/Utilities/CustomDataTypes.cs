namespace CustomDataTypes
{
    public enum CustomTileData : byte
    {
        Block_Movement,
        Kill_Player,
        Level_End,
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
