using System;
using System.Threading.Tasks;

namespace ConnectFour.Persistence
{
    public interface IGameDataAccess
    {
        Task<GameTable> LoadAsync(string path);

        Task SaveAsync(string path, GameTable game);
    }
}
