using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSceneProcessTradeMonsterSelect : BMapSceneProcessState {
	public override MapSceneProcess Update(MapManager mapManager) {
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();
		AllEventManager allEventMgr = AllEventManager.GetInstance();
		PlayerTrainerData playerData = PlayerTrainerData.GetInstance();

		AllEventManager.GetInstance().EventUpdate();

		if (allSceneMgr.inputProvider_.UpSelect()) {
			//選択肢が動かせたら
			if (mapManager.GetTradeMonsterSelectCommandParts().CommandSelectUp(new Vector3(0, 0.55f, 0))) {

			}
		}
		else if (allSceneMgr.inputProvider_.DownSelect()) {
			//選択肢が動かせたら
			if (mapManager.GetTradeMonsterSelectCommandParts().CommandSelectDown(new Vector3(0, -0.55f, 0))) {

			}
		}
		else if (allSceneMgr.inputProvider_.RightSelect()) {
		}
		else if (allSceneMgr.inputProvider_.LeftSelect()) {
		}
		else if (allSceneMgr.inputProvider_.SelectEnter()) {
			if (mapManager.GetTradeMonsterSelectCommandParts().SelectNumber() != mapManager.GetTradeMonsterSelectCommandParts().GetCommandWindowTextsCount() - 1) {
				//追加するモンスターのデータ
				IMonsterData addMonster = EnemyTrainerData.GetInstance().GetMonsterDatas(mapManager.GetTradeMonsterSelectCommandParts().SelectNumber());

				//モンスターの取得
				PlayerTrainerData.GetInstance().MonsterAdd(addMonster);

				//イベントの実行
				mapManager.nowEventMoveMap_.GetEventSetFuncs()[5](mapManager.nowEventMoveMap_, mapManager);

				//ウェイト
				allEventMgr.EventWaitSet(allSceneMgr.GetEventWaitTime());

				//ノベル処理
				{
					string context = addMonster.tribesData_.monsterName_ + "を受け取った！";
					EventMoveMap.NovelEvent(mapManager.GetNovelWindowParts(), context);
				}
			}

			//エネミーのデータの初期化
			EnemyTrainerData.ReleaseInstance();

			//選択肢の初期化
			mapManager.GetTradeMonsterSelectCommandParts().SelectReset(new Vector3(-0.6f, 0.85f, -4));

			//選択肢の非表示
			mapManager.GetTradeMonsterSelectCommandParts().gameObject.SetActive(false);

			//ウェイト
			allEventMgr.EventWaitSet(allSceneMgr.GetEventWaitTime());

			//イベントの実行
			mapManager.nowEventMoveMap_.GetEventSetFuncs()[6](mapManager.nowEventMoveMap_, mapManager);

			return MapSceneProcess.EventExecute;
		}
		else if (allSceneMgr.inputProvider_.SelectBack()) {
		}
		else if (allSceneMgr.inputProvider_.SelectNovelWindowActive()) {
		}
		else if (allSceneMgr.inputProvider_.SelectMenu()) {
		}

		return mapManager.GetProcessProvider().state_;
	}
}
