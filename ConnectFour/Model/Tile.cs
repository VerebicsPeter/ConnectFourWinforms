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

        public static Tile MakeTile(char value)
        {
            if (value == 'x')
            {
                return new Tile(TileValue.X);
            }
            if (value == 'o')
            {
                return new Tile(TileValue.O);
            }
            return new Tile(TileValue.EMPTY);
        }

        public bool IsEmpty () { return Value == TileValue.EMPTY; }
        public bool IsX () { return Value == TileValue.X; }
        public bool ISO () { return Value == TileValue.O; }
    }
}