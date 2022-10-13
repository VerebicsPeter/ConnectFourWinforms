namespace ConnectFour.Model
{
    internal enum TileValue 
    {
        EMPTY, X, O
    }

    internal class Tile
    {
        public TileValue Value { get; set; }

        public Tile()
        {
            Value = TileValue.EMPTY;
        }
        public Tile(TileValue value)
        {
            Value = value;
        }

        public bool IsEmpty () { return Value == TileValue.EMPTY; }
        public bool IsRed () { return Value == TileValue.X; }
        public bool IsYellow () { return Value == TileValue.O; }
    }
}