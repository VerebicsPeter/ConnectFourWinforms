using System;

namespace ConnectFour.Model
{
    internal class TileChangedEventArgs : EventArgs
    {
        public Player PlayerOnTile { get; private set; }

        public int X { get; private set; }

        public int Y { get; private set; }

        public TileChangedEventArgs(int x, int y, Player player)
        {
            PlayerOnTile = player;
            X = x;
            Y = y;
        }
    }
}
