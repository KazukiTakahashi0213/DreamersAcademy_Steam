using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMenuSceneSkillActionCommandExecuteTrade : BMonsterMenuSceneSkillActionCommandExecuteState {
	public override void Execute(MonsterMenuManager monsterMenuManager) {
		AllEventManager eventMgr = AllEventManager.GetInstance();
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();

		sceneMgr.inputProvider_ = new InactiveInputProvider();

		monsterMenuManager.GetSkillActionCommandParts().gameObject.SetActive(false);

		MonsterMenuManager.skillTradeSelectMonsterNumber_ = monsterMenuManager.selectMonsterNumber_;

		//フェードアウト
		eventMgr.EventSpriteRendererSet(
			sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 255)
			);
		eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		eventMgr.AllUpdateEventExecute(0.4f);

		//シーンの切り替え
		BugMenuManager.SetProcessStateProvider(new BugMenuSceneMonsterMenuSkillTradeProcessStateProvider());
		eventMgr.SceneChangeEventSet(SceneState.BugMenu, SceneChangeMode.Slide);
	}
}
