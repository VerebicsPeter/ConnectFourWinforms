using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour.Persistence
{
    public class SaveFileDataAcess : IGameDataAccess
    {
        public async Task<GameTable> LoadAsync(string path)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync(string path, GameTable game)
        {
            throw new NotImplementedException();
        }
    }
}
