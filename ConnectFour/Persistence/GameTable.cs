using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour.Persistence
{
    public class GameTable
    {
        // represents the game tabel given a valid game state (loaded from a file)

        public int X, Y;         // size
        public int time1, time2; // time1 = x's time, time2 = o's time elapsed
        public string currPlayer;// current player
        public string[] Table;   // board

        public GameTable(int x, int y, int t1, int t2, string curr)
        {
            X = x;
            Y = y;
            time1 = t1;
            time2 = t2;
            currPlayer = curr;
            Table = new string[X];
        }
    }
}
