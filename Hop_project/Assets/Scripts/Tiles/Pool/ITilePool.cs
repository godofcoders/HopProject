
using UnityEngine;

namespace Tiles.Pool
{
    public interface ITilePool
    {
        GameObject GetTile();
        void ReturnTile(GameObject tile);
    }   
}
