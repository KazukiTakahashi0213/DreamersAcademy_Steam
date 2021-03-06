using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSceneMenuSelectCommandSelectTitle : BMapSceneMenuSelectCommandSelectState {
	public override void SelectEnter(MapManager mapManager) {
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();
		AllEventManager eventMgr = AllEventManager.GetInstance();

		//操作の変更
		sceneMgr.inputProvider_ = new InactiveInputProvider();

		//プレイヤーデータの初期化
		PlayerTrainerData.ReleaseInstance();

		//選択肢の初期化
		mapManager.GetCommandParts().gameObject.SetActive(false);
		mapManager.GetCommandParts().SelectReset(new Vector3(-0.6f, 0.85f, -4));

		//フェードアウト
		eventMgr.EventSpriteRendererSet(
			sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 255)
			);
		eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		eventMgr.AllUpdateEventExecute(sceneMgr.GetEventWaitTime());

		//イベントの最後
		//シーンの切り替え
		eventMgr.SceneChangeEventSet(SceneState.Title, SceneChangeMode.Change);
	}
}
