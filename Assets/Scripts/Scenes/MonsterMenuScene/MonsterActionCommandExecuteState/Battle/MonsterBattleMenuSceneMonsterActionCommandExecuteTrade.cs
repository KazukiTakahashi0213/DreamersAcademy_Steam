using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBattleMenuSceneMonsterActionCommandExecuteTrade : BMonsterBattleMenuSceneMonsterActionCommandExecuteState {
	public override void Execute(MonsterMenuManager monsterMenuManager) {
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();
		AllEventManager eventMgr = AllEventManager.GetInstance();

		if (PlayerBattleData.GetInstance().GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).battleActive_
			&& PlayerBattleData.GetInstance().GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).tribesData_.monsterNumber_ != 0) {
			sceneMgr.inputProvider_ = new InactiveInputProvider();

			PlayerBattleData.GetInstance().changeMonsterNumber_ = monsterMenuManager.selectMonsterNumber_;
			PlayerBattleData.GetInstance().changeMonsterActive_ = true;

			//フェードアウト
			eventMgr.EventSpriteRendererSet(
				sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
				, null
				, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 255)
				);
			eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
			eventMgr.AllUpdateEventExecute(0.4f);

			//シーンの切り替え
			eventMgr.SceneChangeEventSet(SceneState.Battle, SceneChangeMode.Continue);
		}
	}
}
