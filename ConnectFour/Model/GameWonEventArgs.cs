using System;

namespace ConnectFour.Model
{
    internal class GameWonEventArgs : EventArgs
    {
        public GameState State { get; private set; }

        public List<Point> WinningCoordList { get; private set; }

        public GameWonEventArgs(GameState state, Stack<Point> winningCoords)
        {
            State = state;
            WinningCoordList = winningCoords.ToList();
        }
    }
}
