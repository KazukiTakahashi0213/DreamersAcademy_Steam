using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSceneMenuSelectCommandSelectReport : BMapSceneMenuSelectCommandSelectState {
	public override void SelectEnter(MapManager mapManager) {
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();
		AllEventManager eventMgr = AllEventManager.GetInstance();

		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().blinkTimeRegulation_ = 0.5f;
		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().GetBlinkState().state_ = UpdateSpriteRendererProcessBlink.In;

		//選択肢の初期化
		mapManager.GetCommandParts().SelectReset(new Vector3(-0.6f, 0.85f, -4));

		mapManager.GetPlayerMoveMap().is_move = false;
		mapManager.GetCommandParts().gameObject.SetActive(false);

		//操作の変更
		sceneMgr.inputProvider_ = new KeyBoardSelectInactiveTriggerInputProvider();

		mapManager.GetProcessProvider().state_ = MapSceneProcess.EventExecute;
		mapManager.eventBackProcess_ = MapSceneProcess.PlayerMove;

		//データのセーブ
		SaveDataTrasfer.GetInstance().DataSave();

		//SE
		eventMgr.SEAudioPlayOneShotEventSet(ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathSelect2()));

		//ノベル処理
		EventMoveMap.NovelEvent(mapManager.GetNovelWindowParts(), "レポートに　しっかりかきのこした！");

		//イベントの最後
		eventMgr.EventFinishSet();
	}
}
