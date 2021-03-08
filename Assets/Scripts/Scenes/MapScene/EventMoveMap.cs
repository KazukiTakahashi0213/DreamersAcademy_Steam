using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMoveMap : ObjectMoveMap {
	[SerializeField] private EventMoveMapTrigger startTrigger_ = EventMoveMapTrigger.None;

	private EventMoveMapTriggerState triggerState_ = new EventMoveMapTriggerState(EventMoveMapTrigger.None);

	public int executeEventNum_ = 1;

	public enum MOVE_TYPE { NONE, RANDOM, }

	[SerializeField] MOVE_TYPE MoveType = MOVE_TYPE.NONE;//動く種類を設定

	[SerializeField] float _move_interval = 3.0f;//動く間隔

	private float _move_time = 0;

	public delegate void EventSetFunc(EventMoveMap eventMoveMap, MapManager mapManager);
	private List<EventSetFunc> eventSetFuncs_ = new List<EventSetFunc>();

	//EntryPoint
	void Start() {
		Init();
		EventInit();
	}

	void Update() {
		if (!TransMove()) StopAnim();//ここの記載順を変更すれば、話しかけたときに足踏み止めたりできると思う。

		if (!is_move) return;
		if (MoveType == MOVE_TYPE.NONE) return;

		_move_time += Time.deltaTime;
		if (_move_time > _move_interval) {
			_move_time = 0;
			var rand = Random.Range(0, 4);
			if (rand == 0) MoveUp();
			if (rand == 1) MoveDown();
			if (rand == 2) MoveRight();
			if (rand == 3) MoveLeft();
		}

	}

	public EventMoveMapTriggerState GetTriggerState() { return triggerState_; }
	public List<EventSetFunc> GetEventSetFuncs() { return eventSetFuncs_; }

	public void EventInit() {
		triggerState_.state_ = startTrigger_;
		eventSetFuncs_.Add(NoneEvent);
	}

	private static void NoneEvent(EventMoveMap eventMoveMap, MapManager mapManager) {
		AllEventManager allEventMgr = AllEventManager.GetInstance();

		//イベントの最後
		allEventMgr.EventFinishSet();
	}

	static public void NovelEvent(NovelWindowParts novelWindowParts, string context) {
		AllEventManager allEventMgr = AllEventManager.GetInstance();
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();

		//ウィンドウの表示
		allEventMgr.UpdateGameObjectSet(novelWindowParts.GetUpdateGameObject());
		allEventMgr.UpdateGameObjectsActiveSetExecute(true);

		List<string> contexts = t13.Utility.ContextSlice(context, "\r\n\r\n");

		for (int i = 0; i < contexts.Count; ++i) {
			//文字列の処理
			allEventMgr.EventTextSet(novelWindowParts.GetNovelWindowEventText(), contexts[i]);
			allEventMgr.EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
			allEventMgr.AllUpdateEventExecute(0.6f);

			//Blinkの開始
			allEventMgr.EventSpriteRendererSet(novelWindowParts.GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite());
			allEventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.BlinkStart);
			allEventMgr.AllUpdateEventExecute();

			//Enterの押下待ち
			allEventMgr.EventTriggerSet();

			//SE
			allEventMgr.SEAudioPlayOneShotEventSet(ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathNovelNext()));

			//Blinkの終了
			allEventMgr.EventSpriteRendererSet(novelWindowParts.GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite());
			allEventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.BlinkEnd);
			allEventMgr.AllUpdateEventExecute();
		}

		//ウィンドウの初期化
		allEventMgr.EventTextSet(novelWindowParts.GetNovelWindowEventText(), "");
		allEventMgr.EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
		allEventMgr.AllUpdateEventExecute();

		//ウィンドウの非表示
		allEventMgr.UpdateGameObjectSet(novelWindowParts.GetUpdateGameObject());
		allEventMgr.UpdateGameObjectsActiveSetExecute(false);
	}

	static public void ObjectMovePosYEvent(ObjectMoveMap objectMoveMap, int addValue, float timeRgulation) {
		AllEventManager allEventMgr = AllEventManager.GetInstance();
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();

		allEventMgr.UpdateGameObjectSet(objectMoveMap.GetUpdateGameObject(), new Vector3(objectMoveMap.gameObject.transform.position.x, objectMoveMap.gameObject.transform.position.y + addValue, objectMoveMap.gameObject.transform.position.z));
		allEventMgr.UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);
		allEventMgr.AllUpdateEventExecute(timeRgulation);
		if(addValue < 0) {
			objectMoveMap.MapMoveDown(System.Math.Abs(addValue));
		}
		else {
			objectMoveMap.MapMoveUp(System.Math.Abs(addValue));	
		}
	}
	static public void ObjectMovePosXEvent(ObjectMoveMap objectMoveMap, int addValue, float timeRgulation) {
		AllEventManager allEventMgr = AllEventManager.GetInstance();
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();

		allEventMgr.UpdateGameObjectSet(objectMoveMap.GetUpdateGameObject(), new Vector3(objectMoveMap.gameObject.transform.position.x + addValue, objectMoveMap.gameObject.transform.position.y, objectMoveMap.gameObject.transform.position.z));
		allEventMgr.UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);
		allEventMgr.AllUpdateEventExecute(timeRgulation);
		if (addValue < 0) {
			objectMoveMap.MapMoveLeft(System.Math.Abs(addValue));
		}
		else {
			objectMoveMap.MapMoveRight(System.Math.Abs(addValue));
		}
	}

	static public void BattleEvent() {
		AllEventManager allEventMgr = AllEventManager.GetInstance();
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();
		PlayerTrainerData playerTrainerData = PlayerTrainerData.GetInstance();
		EnemyTrainerData enemyTrainerData = EnemyTrainerData.GetInstance();
		PlayerBattleData playerBattleData = PlayerBattleData.GetInstance();
		EnemyBattleData enemyBattleData = EnemyBattleData.GetInstance();

		//白
		allEventMgr.EventSpriteRendererSet(
			allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(1, 1, 1, 0));
		allEventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		allEventMgr.AllUpdateEventExecute();

		//表示
		allEventMgr.EventSpriteRendererSet(
			allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(1, 1, 1, 1.0f));
		allEventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		allEventMgr.AllUpdateEventExecute(0.3f);

		//非表示
		allEventMgr.EventSpriteRendererSet(
			allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(1, 1, 1, 0));
		allEventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		allEventMgr.AllUpdateEventExecute(0.2f);

		//表示
		allEventMgr.EventSpriteRendererSet(
			allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(1, 1, 1, 1.0f));
		allEventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		allEventMgr.AllUpdateEventExecute(0.3f);

		//非表示
		allEventMgr.EventSpriteRendererSet(
			allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(1, 1, 1, 0));
		allEventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		allEventMgr.AllUpdateEventExecute(0.2f);

		//表示
		allEventMgr.EventSpriteRendererSet(
			allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(1, 1, 1, 1.0f));
		allEventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		allEventMgr.AllUpdateEventExecute(0.6f);

		//ウェイト
		allEventMgr.EventWaitSet(0.6f);

		//黒
		allEventMgr.EventSpriteRendererSet(
			allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(0, 0, 0, 1.0f));
		allEventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		allEventMgr.AllUpdateEventExecute();

		//プレイヤーのモンスター設定
		playerBattleData.monsterAdd(playerTrainerData.GetMonsterDatas(0));
		playerBattleData.monsterAdd(playerTrainerData.GetMonsterDatas(1));
		playerBattleData.monsterAdd(playerTrainerData.GetMonsterDatas(2));

		//シーンの切り替え
		allEventMgr.SceneChangeEventSet(SceneState.Battle, SceneChangeMode.Slide);
	}
}
