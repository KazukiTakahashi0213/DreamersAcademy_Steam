using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour, ISceneManager {
	private MapSceneProcessProvider processProvider_ = new MapSceneProcessProvider();
	public MapSceneProcess eventBackProcess_ = MapSceneProcess.None;
	private MapSceneInputSoundProvider inputSoundProvider_ = new MapSceneInputSoundProvider();

	private bool firstStart_ = true;

	public EventMoveMap nowEventMoveMap_ = null;

	public bool monsterTradeSelectActive_ = false;

	[SerializeField] private MapData mapData_ = null;
	[SerializeField] private Camera mainCamera_ = null;
	[SerializeField] private PlayerMoveMap playerMoveMap_ = null;
	[SerializeField] private NovelWindowParts novelWindowParts_ = null;
	[SerializeField] private CommandParts commandParts_ = null;
	[SerializeField] private List<FloorObjectsParts> floorObjects_ = null;
	[SerializeField] private CommandParts tradeMonsterSelectCommandParts_ = null;

	public MapSceneProcessProvider GetProcessProvider() { return processProvider_; }
	public MapSceneInputSoundProvider GetInputSoundProvider() { return inputSoundProvider_; }

	public MapData GetMapData() { return mapData_; }
	public Camera GetMainCamera() { return mainCamera_; }
	public NovelWindowParts GetNovelWindowParts() { return novelWindowParts_; }
	public PlayerMoveMap GetPlayerMoveMap() { return playerMoveMap_; }
	public CommandParts GetCommandParts() { return commandParts_; }
	public FloorObjectsParts GetFloorObjects() { return floorObjects_[PlayerTrainerData.GetInstance().nowMapFloor_]; }
	public CommandParts GetTradeMonsterSelectCommandParts() { return tradeMonsterSelectCommandParts_; }

	public void SceneStart() {
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();
		AllEventManager allEventMgr = AllEventManager.GetInstance();
		PlayerTrainerData playerData = PlayerTrainerData.GetInstance();

		//初めての処理だったら
		if (firstStart_) {
			firstStart_ = false;

			//オブジェクトの配置
			for (int i = 0; i < floorObjects_.Count; ++i) {
				floorObjects_[i].gameObject.SetActive(true);
			}

			//依存性注入
			processProvider_.state_ = MapSceneProcess.PlayerMove;

			//ウェイト
			allEventMgr.EventWaitSet(0.5f);

			//シーンの切り替え
			allEventMgr.SceneChangeEventSet(SceneState.Map, SceneChangeMode.Change);
		}
		else {
			//依存性注入
			processProvider_.state_ = MapSceneProcess.PlayerMove;
			inputSoundProvider_.state_ = MapSceneInputSoundState.Normal;

			//主人公の移動の変更
			playerMoveMap_.is_move = true;

			//BGMの再生
			AllSceneManager.GetInstance().GetPublicAudioParts().GetBGMAudioSource().clip = ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathDreamers_Map());
			AllSceneManager.GetInstance().GetPublicAudioParts().GetBGMAudioSource().Play();

			//オブジェクトの配置
			for (int i = 0; i < floorObjects_.Count; ++i) {
				floorObjects_[i].gameObject.SetActive(false);
			}
			floorObjects_[playerData.nowMapFloor_].gameObject.SetActive(true);

			//マップデータに反映
			mapData_.MapDataReset();

			//各階層の扉の画像の反映
			{
				Sprite[] sprites = ResourcesGraphicsLoader.GetInstance().GetGraphicsAll("MapScene/institute01");
				for (int i = floorObjects_.Count - 1; i >= playerData.clearMapFloor_; --i) {
					floorObjects_[i].GetEventMoveMaps(1).GetEventSpriteRenderer().GetSpriteRenderer().sprite = sprites[44];
					floorObjects_[i].GetEventMoveMaps(2).GetEventSpriteRenderer().GetSpriteRenderer().sprite = sprites[43];
					floorObjects_[i].GetEventMoveMaps(3).GetEventSpriteRenderer().GetSpriteRenderer().sprite = sprites[51];
					floorObjects_[i].GetEventMoveMaps(4).GetEventSpriteRenderer().GetSpriteRenderer().sprite = sprites[52];
				}
				for (int i = 0; i < playerData.clearMapFloor_; ++i) {
					floorObjects_[i].GetEventMoveMaps(1).GetEventSpriteRenderer().GetSpriteRenderer().sprite = null;
					floorObjects_[i].GetEventMoveMaps(2).GetEventSpriteRenderer().GetSpriteRenderer().sprite = null;
					floorObjects_[i].GetEventMoveMaps(3).GetEventSpriteRenderer().GetSpriteRenderer().sprite = null;
					floorObjects_[i].GetEventMoveMaps(4).GetEventSpriteRenderer().GetSpriteRenderer().sprite = null;
				}
			}
			//各階層の階層イベントの反映
			for (int i = floorObjects_.Count - 1; i >= playerData.clearMapFloor_; --i) {
				floorObjects_[i].GetEventMoveMaps(0).executeEventNum_ = 1;
				floorObjects_[i].GetEventMoveMaps(1).executeEventNum_ = 0;
				floorObjects_[i].GetEventMoveMaps(2).executeEventNum_ = 0;
			}
			for (int i = 0; i < playerData.clearMapFloor_; ++i) {
				floorObjects_[i].GetEventMoveMaps(0).executeEventNum_ = 4;
				floorObjects_[i].GetEventMoveMaps(1).executeEventNum_ = 1;
				floorObjects_[i].GetEventMoveMaps(2).executeEventNum_ = 1;
			}

			//フェードイン
			allEventMgr.EventSpriteRendererSet(
				allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
				, null
				, new Color(allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 0)
				);
			allEventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
			allEventMgr.AllUpdateEventExecute(0.4f);

			//イベントの最後
			//制御の変更
			allEventMgr.InputProviderChangeEventSet(new KeyBoardNormalInputProvider());
		}
	}

	public void SceneUpdate() {
		processProvider_.state_ = processProvider_.Update(this);
	}

	public void SceneEnd() {
		//主人公の向きの変更
		if (playerMoveMap_.GetStartPos().y < 9.2f) {
			playerMoveMap_.direction = ObjectMoveMap.DIRECTION_STATUS.UP;
		}

		//オブジェクトの向きの初期化
		for (int i = 0; i < floorObjects_[PlayerTrainerData.GetInstance().nowMapFloor_].GetEventMoveMapsCount(); ++i) {
			floorObjects_[PlayerTrainerData.GetInstance().nowMapFloor_].GetEventMoveMaps(i).direction = ObjectMoveMap.DIRECTION_STATUS.DOWN;
		}

		//オブジェクトの座標の初期化
		for (int i = 0; i < floorObjects_[PlayerTrainerData.GetInstance().nowMapFloor_].GetEventMoveMapsCount(); ++i) {
			floorObjects_[PlayerTrainerData.GetInstance().nowMapFloor_].GetEventMoveMaps(i).ResetNowPos();
		}
		playerMoveMap_.ResetNowPos();

		playerMoveMap_.SetStartPos(new Vector3(9, 2.2f, -1));
	}

	public GameObject GetGameObject() { return gameObject; }
}
