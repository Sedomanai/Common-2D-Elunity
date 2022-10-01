using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Elang
{
    [System.Serializable]
    public class TopTile : PillarTile
    {
        [SerializeField]
        Tilemap _map;

        public override Sprite GetSprite(Vector3Int location, ITilemap map, TileQuirks quirks) {
            ElangTile tile = map.GetTile<ElangTile>(location - new Vector3Int(0, -1, 0));
            if (tile) {
                return base.GetSprite(location, map, quirks);
            } else return null;
        }
    }
}