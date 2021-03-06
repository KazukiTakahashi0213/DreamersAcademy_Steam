using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSceneProcessPlayerMove : BMapSceneProcessState {
	public override MapSceneProcess Update(MapManager mapManager) {
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();
		AllEventManager allEventMgr = AllEventManager.GetInstance();
		PlayerTrainerData playerData = PlayerTrainerData.GetInstance();

		AllEventManager.GetInstance().EventUpdate();

		if (playerData.battleEnd_) {
			playerData.battleEnd_ = false;

			if (playerData.battleResult_) {
				mapManager.nowEventMoveMap_.executeEventNum_ = 2;
			}
			else {
				mapManager.nowEventMoveMap_.executeEventNum_ = 3;
			}

			//操作の変更
			allSceneMgr.inputProvider_ = new KeyBoardSelectInactiveTriggerInputProvider();

			mapManager.GetPlayerMoveMap().is_move = false;

			//フェードイン
			allEventMgr.EventSpriteRendererSet(
				allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
				, null
				, new Color(allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 0)
				);
			allEventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
			allEventMgr.AllUpdateEventExecute(0.4f);

			//戦闘結果イベントの実行
			mapManager.nowEventMoveMap_.GetEventSetFuncs()[mapManager.nowEventMoveMap_.executeEventNum_](mapManager.nowEventMoveMap_, mapManager);

			mapManager.eventBackProcess_ = mapManager.GetProcessProvider().state_;
			return MapSceneProcess.EventExecute;
		}

		//イベントがエントリーゾーンにあったら
		if (mapManager.GetPlayerMoveMap().GetEntryZone()._collision_object) {
			EventMoveMap eventObject = mapManager.GetPlayerMoveMap().GetEntryZone()._collision_object;

			if (eventObject.GetTriggerState().EventTrigger(mapManager.GetPlayerMoveMap().GetEntryZone(), mapManager.GetPlayerMoveMap())) {
				eventObject.GetEventSetFuncs()[eventObject.executeEventNum_](eventObject, mapManager);

				mapManager.GetPlayerMoveMap().is_move = false;

				//操作の変更
				allSceneMgr.inputProvider_ = new KeyBoardSelectInactiveTriggerInputProvider();

				mapManager.nowEventMoveMap_ = eventObject;

				mapManager.eventBackProcess_ = mapManager.GetProcessProvider().state_;
				return MapSceneProcess.EventExecute;
			}
		}

		if (allSceneMgr.inputProvider_.UpSelect()) {
		}
		else if (allSceneMgr.inputProvider_.DownSelect()) {
		}
		else if (allSceneMgr.inputProvider_.RightSelect()) {
		}
		else if (allSceneMgr.inputProvider_.LeftSelect()) {
		}
		else if (allSceneMgr.inputProvider_.SelectEnter()) {
			allEventMgr.EventTriggerNext();
		}
		else if (allSceneMgr.inputProvider_.SelectBack()) {
		}
		else if (allSceneMgr.inputProvider_.SelectNovelWindowActive()) {
		}
		else if (allSceneMgr.inputProvider_.SelectMenu()) {
			//SE
			mapManager.GetInputSoundProvider().SelectMenu();

			mapManager.GetPlayerMoveMap().is_move = false;
			mapManager.GetCommandParts().gameObject.SetActive(true);

			//操作の変更
			allSceneMgr.inputProvider_ = new KeyBoardNormalTriggerInputProvider();

			return MapSceneProcess.MenuSelect;
		}

		return mapManager.GetProcessProvider().state_;
	}
}
