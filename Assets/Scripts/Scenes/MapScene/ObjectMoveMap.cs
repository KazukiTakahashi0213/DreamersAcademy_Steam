using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoveMap : MonoBehaviour
{
    [SerializeField] UpdateGameObject updateGameObject_ = null;
    [SerializeField] EventSpriteRenderer eventSpriteRenderer_ = null;

    public DIRECTION_STATUS direction = DIRECTION_STATUS.UP;//初期のキャラの向き

    public enum DIRECTION_STATUS
    {
        UP,
        DOWN,
        RIGHT,
        LEFT,
    }

    public float speed = 1.0f;//移動スピード

    public MapData.MAP_STATUS ObjectType = MapData.MAP_STATUS.PLAYER;//オブジェクトの種類

    protected MapData _map = null;

    protected Vector2 _now_pos = Vector2.zero;
    protected Vector3 _start_pos = Vector3.zero;
    protected Vector2 _next = Vector2.zero;
    public Vector2 GetNext() { return _next; }
    public Vector3 GetStartPos() { return _start_pos; }
    public void SetStartPos(Vector3 value) { _start_pos = value; }
    public void ResetNowPos() { 
        _now_pos = _start_pos;
        transform.position = _start_pos;
    }

    public bool is_move { get; set; } = true;//falseは動けない(バトル中、話しかけてる最中など)

    public UpdateGameObject GetUpdateGameObject() { return updateGameObject_; }
    public EventSpriteRenderer GetEventSpriteRenderer() { return eventSpriteRenderer_; }

    //アニメーション関連
    [SerializeField] float _walk_interval = 0.2f;
    [SerializeField] float _stop_interval = 0.4f;
    [SerializeField] public bool animeSpritesActive_ = false;
    SpriteRenderer _sprite_renderer = null;
    Sprite[] sprites = null;
    int[] dir_states = new int[4] { 10, 1, 7, 4 };

    protected void Init()//継承したクラスのスタート関数で必ず呼び出すこと
    {
        _map = GameObject.FindGameObjectWithTag("Map").GetComponent<MapData>();
        _now_pos = transform.position;
        _start_pos = transform.position;

        _sprite_renderer = GetComponent<SpriteRenderer>();
        if (animeSpritesActive_
            && _sprite_renderer.sprite) {
            var name = _sprite_renderer.sprite.name;
            name = name.Substring(0, name.IndexOf("_"));
            sprites = ResourcesGraphicsLoader.GetInstance().GetGraphicsAll("CharaChip/" + name);
        }
    }

    protected void StopAnim()
    {
        if (sprites == null) return;

        if (_stop_interval != 0) ChangeSprite(dir_states[(int)direction], _stop_interval);
        else _sprite_renderer.sprite = sprites[dir_states[(int)direction]];
    }

    protected virtual void MoveUp()
    {
        direction = DIRECTION_STATUS.UP;
        if (_next != Vector2.zero) return;
        var next_pos = new Vector2(_now_pos.x, _now_pos.y + 1);
        if (_map.MoveCheck(next_pos, ObjectType))
        {
            _next.y = 1;
            _map.MemoryNextTileMapStatus(next_pos);
            _map.SetMapStatus(next_pos, ObjectType);
        }
    }
    protected virtual void MoveDown()
    {
        direction = DIRECTION_STATUS.DOWN;
        if (_next != Vector2.zero) return;
        var next_pos = new Vector2(_now_pos.x, _now_pos.y - 1);
        if (_map.MoveCheck(next_pos, ObjectType))
        {
            _next.y = -1;
            _map.MemoryNextTileMapStatus(next_pos);
            _map.SetMapStatus(next_pos, ObjectType);
        }
    }
    protected virtual void MoveRight()
    {
        direction = DIRECTION_STATUS.RIGHT;
        if (_next != Vector2.zero) return;
        var next_pos = new Vector2(_now_pos.x + 1, _now_pos.y);
        if (_map.MoveCheck(next_pos, ObjectType))
        {
            _next.x = 1;
            _map.MemoryNextTileMapStatus(next_pos);
            _map.SetMapStatus(next_pos, ObjectType);
        }
    }

    protected virtual void MoveLeft()
    {
        direction = DIRECTION_STATUS.LEFT;
        if (_next != Vector2.zero) return;
        var next_pos = new Vector2(_now_pos.x - 1, _now_pos.y);
        if (_map.MoveCheck(next_pos, ObjectType))
        {
            _next.x = -1;
            _map.MemoryNextTileMapStatus(next_pos);
            _map.SetMapStatus(next_pos, ObjectType);
        }
    }

    protected bool TransMove()
    {
        if (_next == Vector2.zero) return false;

        //上
        if (_next.y == 1)
        {
            transform.Translate(_next * speed * Time.deltaTime);
            if (_now_pos.y + _next.y <= transform.position.y)
            {
                transform.position = new Vector3(_now_pos.x, _now_pos.y + _next.y, _start_pos.z);
                _map.SetMapStatus(_now_pos, _map.GetNextTileMapStatus());
                _now_pos = transform.position;
                _next = Vector2.zero;
            }
        }

        //下
        if (_next.y == -1)
        {
            transform.Translate(_next * speed * Time.deltaTime);
            if (_now_pos.y + _next.y >= transform.position.y)
            {
                transform.position = new Vector3(_now_pos.x, _now_pos.y + _next.y, _start_pos.z);
                _map.SetMapStatus(_now_pos, _map.GetNextTileMapStatus());
                _now_pos = transform.position;
                _next = Vector2.zero;
            }
        }

        //右
        if (_next.x == 1)
        {
            transform.Translate(_next * speed * Time.deltaTime);
            if (_now_pos.x + _next.x <= transform.position.x)
            {
                transform.position = new Vector3(_now_pos.x + _next.x, _now_pos.y, _start_pos.z);
                _map.SetMapStatus(_now_pos, _map.GetNextTileMapStatus());
                _now_pos = transform.position;
                _next = Vector2.zero;
            }
        }

        //左
        if (_next.x == -1)
        {
            transform.Translate(_next * speed * Time.deltaTime);
            if (_now_pos.x + _next.x >= transform.position.x)
            {
                transform.position = new Vector3(_now_pos.x + _next.x, _now_pos.y, _start_pos.z);
                _map.SetMapStatus(_now_pos, _map.GetNextTileMapStatus());
                _now_pos = transform.position;
                _next = Vector2.zero;
            }
        }

        ChangeSprite(dir_states[(int)direction], _walk_interval);

        return true;
    }

    float sum_time = 0;
    int[] walk_state = new int[4] { 1, 0, -1, 0 };
    void ChangeSprite(int e, float _interval)
    {
        var interval = _interval;
        sum_time += Time.deltaTime;
        for (int i = 0; i < walk_state.Length; i++)
        {
            if (interval * i <= sum_time && sum_time < interval * i + 1)
                _sprite_renderer.sprite = sprites[e + walk_state[i]];
        }
        if (interval * walk_state.Length <= sum_time) sum_time = 0;
    }


    public void MapMoveUp(int addValue) {
        for (int i = 0; i < addValue; ++i) {
            var next_pos = new Vector3(_now_pos.x, _now_pos.y + 1, 0);

            _next.y = 1;
            _map.MemoryNextTileMapStatus(next_pos);
            _map.SetMapStatus(next_pos, ObjectType);

            _map.SetMapStatus(_now_pos, _map.GetNextTileMapStatus());
            _next = Vector2.zero;

            _now_pos = next_pos;
        }

        _map.DebugLogDataString();
    }
    public void MapMoveDown(int addValue) {
        for (int i = 0; i < addValue; ++i) {
            var next_pos = new Vector3(_now_pos.x, _now_pos.y - 1, 0);

            _next.y = -1;
            _map.MemoryNextTileMapStatus(next_pos);
            _map.SetMapStatus(next_pos, ObjectType);

            _map.SetMapStatus(_now_pos, _map.GetNextTileMapStatus());
            _next = Vector2.zero;

            _now_pos = next_pos;
        }

        _map.DebugLogDataString();
    }
    public void MapMoveRight(int addValue) {
        for (int i = 0; i < addValue; ++i) {
            var next_pos = new Vector3(_now_pos.x + 1, _now_pos.y, 0);

            _next.x = 1;
            _map.MemoryNextTileMapStatus(next_pos);
            _map.SetMapStatus(next_pos, ObjectType);

            _map.SetMapStatus(_now_pos, _map.GetNextTileMapStatus());
            _next = Vector2.zero;

            _now_pos = next_pos;
        }

        _map.DebugLogDataString();
    }
    public void MapMoveLeft(int addValue) {
        for (int i = 0; i < addValue; ++i) {
            var next_pos = new Vector3(_now_pos.x - 1, _now_pos.y, 0);

            _next.x = -1;
            _map.MemoryNextTileMapStatus(next_pos);
            _map.SetMapStatus(next_pos, ObjectType);

            _map.SetMapStatus(_now_pos, _map.GetNextTileMapStatus());
            _next = Vector2.zero;

            _now_pos = next_pos;
        }

        _map.DebugLogDataString();
    }
}
