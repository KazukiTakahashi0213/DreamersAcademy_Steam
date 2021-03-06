using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSceneMenuSelectCommandSelectBack : BMapSceneMenuSelectCommandSelectState {
	public override void SelectEnter(MapManager mapManager) {
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();
		AllEventManager eventMgr = AllEventManager.GetInstance();

		//選択肢の初期化
		mapManager.GetCommandParts().SelectReset(new Vector3(-0.6f, 0.85f, -4));

		mapManager.GetPlayerMoveMap().is_move = true;
		mapManager.GetCommandParts().gameObject.SetActive(false);

		//操作の変更
		sceneMgr.inputProvider_ = new KeyBoardNormalInputProvider();

		mapManager.GetProcessProvider().state_ = MapSceneProcess.PlayerMove;
	}
}
