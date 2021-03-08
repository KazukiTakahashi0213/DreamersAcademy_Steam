using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandEventExecuteProcess : IProcessState {
	public IProcessState BackProcess() {
		return this;
	}

	public IProcessState NextProcess() {
		return new CommandSelectProcess();
	}

	public IProcessState Update(BattleManager mgr) {
		AllEventManager eventMgr = AllEventManager.GetInstance();
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();

		//交換されていたら
		if (PlayerBattleData.GetInstance().changeMonsterActive_) {
			//フェードイン
			eventMgr.EventSpriteRendererSet(
				sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
				, null
				, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 0)
				);
			eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
			eventMgr.AllUpdateEventExecute(0.4f);

			//交換イベント
			PlayerBattleData.GetInstance().MonsterChangeEventSet(mgr);

			//イベントの最後
			eventMgr.EventFinishSet();

			PlayerBattleData.GetInstance().changeMonsterActive_ = false;
		}

		if (AllEventManager.GetInstance().EventUpdate()
			&& PlayerBattleData.GetInstance().GetMonsterDatas(0).battleActive_) {
			mgr.GetPlayerStatusInfoParts().ProcessIdleStart();
			mgr.GetPlayerMonsterParts().ProcessIdleStart();
			mgr.ActiveUiCommand();
			sceneMgr.inputProvider_ = new KeyBoardNormalTriggerInputProvider();

			//文字の色の変更
			IMonsterData md = PlayerBattleData.GetInstance().GetMonsterDatas(0);
			for (int i = 0; i < mgr.GetAttackCommandParts().GetCommandParts().GetCommandWindowTextsCount(); ++i) {
				int simillarResult = EnemyBattleData.GetInstance().GetMonsterDatas(0).ElementSimillarCheckerForValue(md.GetSkillDatas(i).elementType_);

				if (simillarResult == 0) mgr.GetAttackCommandParts().GetCommandParts().GetCommandWindowTexts(i).color = new Color32(195, 195, 195, 255);
				else if (simillarResult == 1) mgr.GetAttackCommandParts().GetCommandParts().GetCommandWindowTexts(i).color = new Color32(52, 130, 207, 255);
				else if (simillarResult == 2) mgr.GetAttackCommandParts().GetCommandParts().GetCommandWindowTexts(i).color = new Color32(50, 50, 50, 255);
				else if (simillarResult == 3) mgr.GetAttackCommandParts().GetCommandParts().GetCommandWindowTexts(i).color = new Color32(207, 52, 112, 255);
			}
			return mgr.nowProcessState().NextProcess();
		}

		return this;
	}
}
