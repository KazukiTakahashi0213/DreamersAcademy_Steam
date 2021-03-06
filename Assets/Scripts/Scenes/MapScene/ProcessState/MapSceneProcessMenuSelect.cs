using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSceneProcessMenuSelect : BMapSceneProcessState {
	//選択肢制御
	private MapSceneMenuSelectCommandSelectProvider commandSelectProvider_ = new MapSceneMenuSelectCommandSelectProvider();
	public MapSceneMenuSelectCommandSelectProvider GetCommandSelectProvider() { return commandSelectProvider_; }

	public override MapSceneProcess Update(MapManager mapManager) {
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();
		AllEventManager eventMgr = AllEventManager.GetInstance();

		AllEventManager.GetInstance().EventUpdate();

		if (sceneMgr.inputProvider_.UpSelect()) {
			//選択肢が動かせたら
			if (mapManager.GetCommandParts().CommandSelect(-1, new Vector3(0, 0.55f, 0))) {
				//SE
				mapManager.GetInputSoundProvider().UpSelect();

				commandSelectProvider_.state_ = (MapSceneMenuSelectCommandSelect)mapManager.GetCommandParts().GetSelectNumber();
			}
		}
		else if (sceneMgr.inputProvider_.DownSelect()) {
			//選択肢が動かせたら
			if (mapManager.GetCommandParts().CommandSelect(1, new Vector3(0, -0.55f, 0))) {
				//SE
				mapManager.GetInputSoundProvider().DownSelect();

				commandSelectProvider_.state_ = (MapSceneMenuSelectCommandSelect)mapManager.GetCommandParts().GetSelectNumber();
			}
		}
		else if (sceneMgr.inputProvider_.RightSelect()) {
		}
		else if (sceneMgr.inputProvider_.LeftSelect()) {
		}
		else if (sceneMgr.inputProvider_.SelectEnter()) {
			//SE
			mapManager.GetInputSoundProvider().SelectEnter();

			commandSelectProvider_.SelectEnter(mapManager);

			commandSelectProvider_.state_ = MapSceneMenuSelectCommandSelect.None;
		}
		else if (sceneMgr.inputProvider_.SelectBack()) {
			//選択肢の初期化
			mapManager.GetCommandParts().SelectReset(new Vector3(-0.6f, 0.85f, -4));

			mapManager.GetPlayerMoveMap().is_move = true;
			mapManager.GetCommandParts().gameObject.SetActive(false);

			//操作の変更
			sceneMgr.inputProvider_ = new KeyBoardNormalInputProvider();

			commandSelectProvider_.state_ = MapSceneMenuSelectCommandSelect.None;

			return MapSceneProcess.PlayerMove;
		}
		else if (sceneMgr.inputProvider_.SelectNovelWindowActive()) {
		}
		else if (sceneMgr.inputProvider_.SelectMenu()) {
			//選択肢の初期化
			mapManager.GetCommandParts().SelectReset(new Vector3(-0.6f, 0.85f, -4));

			mapManager.GetPlayerMoveMap().is_move = true;
			mapManager.GetCommandParts().gameObject.SetActive(false);

			//操作の変更
			sceneMgr.inputProvider_ = new KeyBoardNormalInputProvider();

			commandSelectProvider_.state_ = MapSceneMenuSelectCommandSelect.None;

			return MapSceneProcess.PlayerMove;
		}

		return mapManager.GetProcessProvider().state_;
	}
}
