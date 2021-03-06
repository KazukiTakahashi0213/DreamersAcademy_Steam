using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSceneMenuSelectCommandSelectMonster : BMapSceneMenuSelectCommandSelectState {
	public override void SelectEnter(MapManager mapManager) {
		AllEventManager eventMgr = AllEventManager.GetInstance();
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();

		//操作の変更
		sceneMgr.inputProvider_ = new InactiveInputProvider();

		//選択肢の非表示
		mapManager.GetCommandParts().gameObject.SetActive(false);

		//選択肢の初期化
		mapManager.GetCommandParts().SelectReset(new Vector3(-0.6f, 0.85f, -4));

		//フェードアウト
		eventMgr.EventSpriteRendererSet(
			sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 255)
			);
		eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		eventMgr.AllUpdateEventExecute(0.4f);

		//シーンの切り替え
		MonsterMenuManager.SetProcessStateProvider(new MonsterMenuSceneNormalProcessStateProvider());
		eventMgr.SceneChangeEventSet(SceneState.MonsterMenu, SceneChangeMode.Slide);
	}
}
