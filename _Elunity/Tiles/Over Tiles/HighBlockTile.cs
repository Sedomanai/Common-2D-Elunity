using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Elang
{
#if UNITY_EDITOR
    [CreateAssetMenu(fileName = "High Block Tile", menuName = "Elang Tile/High Block Tile", order = 14)]
#endif
    public class HighBlockTile : ElangTile
    {
        [SerializeField]
        Tilemap _map;

        public ElangTile baseTile;
        public ElangTile pipeTile;

        public override Sprite GetSprite(Vector3Int location, ITilemap map, TileQuirks quirks) {
            var up = new Vector3Int(0, 1, 0);
            var upTile = map.GetTile(location + up);

            if (!upTile) {
                _map.SetTile(up, pipeTile);
            };



            var baseSprite = baseTile.GetSprite(location, map, quirks);

            return baseSprite;
        }
    }

}