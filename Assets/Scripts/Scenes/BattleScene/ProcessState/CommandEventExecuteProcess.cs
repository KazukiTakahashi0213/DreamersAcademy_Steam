using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandEventExecuteProcess : IProcessState {
	public IProcessState BackProcess() {
		return this;
	}

	public IProcessState NextProcess() {
		return new StartCommandSelectProcess();
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
			//ウィンドウの非表示
			mgr.GetNovelWindowParts().gameObject.SetActive(false);

			mgr.GetPlayerStatusInfoParts().ProcessIdleStart();
			mgr.GetPlayerMonsterParts().ProcessIdleStart();
			mgr.ActiveUiStartCommand();
			sceneMgr.inputProvider_ = new KeyBoardNormalTriggerInputProvider();

			//攻撃技の反映
			mgr.GetAttackCommandParts().MonsterDataReflect(PlayerBattleData.GetInstance().GetMonsterDatas(0), EnemyBattleData.GetInstance().GetMonsterDatas(0));

			return mgr.nowProcessState().NextProcess();
		}

		return this;
	}
}
