using System;

namespace ConnectFour.Model
{
    internal class GameWonEventArgs : EventArgs
    {
        public GameState State { get; private set; }

        public GameWonEventArgs(GameState state)
        {
            State = state;
        }
    }
}
