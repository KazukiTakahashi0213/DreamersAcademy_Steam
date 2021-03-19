using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneStartCommandExecuteMonster : BBattleSceneStartCommandExecute {
	public override IProcessState Execute(BattleManager battleManager) {
		AllEventManager eventMgr = AllEventManager.GetInstance();
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();

		//SE
		battleManager.GetInputSoundProvider().SelectEnter();

		battleManager.InactiveUiStartCommand();

		sceneMgr.inputProvider_ = new InactiveInputProvider();

		//フェードアウト
		eventMgr.EventSpriteRendererSet(
			sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 255)
			);
		eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		eventMgr.AllUpdateEventExecute(0.4f);

		//シーンの切り替え
		MonsterMenuManager.SetProcessStateProvider(new MonsterMenuSceneSimpleProcessStateProvider());
		eventMgr.SceneChangeEventSet(SceneState.MonsterMenu, SceneChangeMode.Slide);

		return battleManager.nowProcessState();
	}
}
