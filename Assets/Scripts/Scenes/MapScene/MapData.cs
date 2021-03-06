using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapData : MonoBehaviour
{
    private int[,] _map;//二次元配列

    public enum MAP_STATUS { 
        FLOOR,//0：床
        WALL,//1：壁
        PLAYER,//2：プレイヤー
        EVENT_FLOOR,//3：イベント当たり判定あり
        EVENT_WALL,//4：イベント当たり判定なし
    }

    [SerializeField]Tilemap _tilemap = null;//二次元配列を取得したいtilemapをアタッチ

    MAP_STATUS nextTileMapStatus_ = MAP_STATUS.PLAYER;
    MAP_STATUS nowTileMapStatus_ = MAP_STATUS.FLOOR;

    void Start()
    {
        MapDataReset();
    }

    public void SetMapStatus(Vector3 obj_pos, MAP_STATUS state) {
        _map[_map.GetLength(0) - (int)obj_pos.y, (int)obj_pos.x - 1] = (int)state;
    }

    public void MemoryNextTileMapStatus(Vector3 obj_pos) { nextTileMapStatus_ = (MAP_STATUS)_map[_map.GetLength(0) - (int)obj_pos.y, (int)obj_pos.x - 1]; }
    public MAP_STATUS GetNextTileMapStatus() {
        MAP_STATUS retMapStatus = nowTileMapStatus_;

        nowTileMapStatus_ = nextTileMapStatus_;

        return retMapStatus; 
    }

    public bool MoveCheck(Vector3 obj_pos, MAP_STATUS state) {
        //イベントとイベントを重ねない処理
        if (state == MAP_STATUS.EVENT_WALL || state == MAP_STATUS.EVENT_FLOOR) {
            if (_map[_map.GetLength(0) - (int)obj_pos.y, (int)obj_pos.x - 1] == (int)MAP_STATUS.EVENT_WALL
                || _map[_map.GetLength(0) - (int)obj_pos.y, (int)obj_pos.x - 1] == (int)MAP_STATUS.EVENT_FLOOR) return false;
		}
        if (_map[_map.GetLength(0) - (int)obj_pos.y, (int)obj_pos.x - 1] == (int)MAP_STATUS.WALL
            || _map[_map.GetLength(0) - (int)obj_pos.y, (int)obj_pos.x - 1] == (int)MAP_STATUS.PLAYER
            || _map[_map.GetLength(0) - (int)obj_pos.y, (int)obj_pos.x - 1] == (int)MAP_STATUS.EVENT_WALL) {
            return false;
        }

        return true;
    }

    //データのリセット
    public void MapDataReset() {
        _map = new int[_tilemap.size.y, _tilemap.size.x];

        for (int y = 0; y < _map.GetLength(0); y++) {
            for (int x = 0; x < _map.GetLength(1); x++) {
                if (_tilemap.GetTile(new Vector3Int(x, y, 0))) _map[_map.GetLength(0) - y - 1, x] = (int)MAP_STATUS.WALL;
            }
        }

        SetMapStatus(GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position, MAP_STATUS.PLAYER);

        foreach (var human in GameObject.FindGameObjectsWithTag("Human")) {
            SetMapStatus(human.GetComponent<Transform>().position, MAP_STATUS.EVENT_WALL);
        }

        foreach (var human in GameObject.FindGameObjectsWithTag("Event")) {
            SetMapStatus(human.GetComponent<Transform>().position, MAP_STATUS.EVENT_FLOOR);
        }

        DebugLogDataString();
    }

    //デバッグで二次元配列を表示
    public void DebugLogDataString() {
        string map_str = "\n";
        for (int y = 0; y < _map.GetLength(0); y++)
        {
            for (int x = 0; x < _map.GetLength(1); x++)
            {
                if (_map[y, x] == (int)MAP_STATUS.FLOOR)  map_str += "行,";
                if (_map[y, x] == (int)MAP_STATUS.WALL)   map_str += "壁,";
                if (_map[y, x] == (int)MAP_STATUS.PLAYER) map_str += "俺,";
                if (_map[y, x] == (int)MAP_STATUS.EVENT_WALL)  map_str += "人,";
                if (_map[y, x] == (int)MAP_STATUS.EVENT_FLOOR)  map_str += "動,";
            }
            map_str += "\n";
        }
        Debug.Log(map_str);
    }
}