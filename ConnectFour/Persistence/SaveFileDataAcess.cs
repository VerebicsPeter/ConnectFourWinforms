using System;
using System.IO;
using System.Threading.Tasks;

namespace ConnectFour.Persistence
{
    public class SaveFileDataAcess : IGameDataAccess
    {
        public async Task<GameTable> LoadAsync(string path)
        {
            try
            {
                using (StreamReader reader = new (path))
                {
                    string line = await reader.ReadLineAsync() ?? String.Empty;
                    
                    string[] parts = line.Split(';');

                    int x, y, t1, t2;
                    string currentPlayer;
                    x  = Int32.Parse(parts[0]);
                    y  = Int32.Parse(parts[1]);
                    t1 = Int32.Parse(parts[2]);
                    t2 = Int32.Parse(parts[3]);
                    currentPlayer = parts[4];

                    GameTable table = new(x, y, t1, t2, currentPlayer);

                    for (int i = 0; i < x; i++)
                    {
                        line = await reader.ReadLineAsync() ?? String.Empty;

                        table.Table[i] = line;
                    }
                    return table;
                }
            }
            catch
            {
                throw new IOException();
            }
        }

        public async Task SaveAsync(string path, GameTable game)
        {
            try
            {
                using (StreamWriter writer = new (path))
                {
                    writer.Write($"{game.X};{game.Y};{game.time1};{game.time2};{game.currPlayer}");
                    writer.WriteLine();
                    for (int i = 0; i < game.X; i++)
                    {
                        await writer.WriteLineAsync(game.Table[i]);
                    }
                }
            }
            catch
            {
                throw new IOException();
            }
        }
    }
}