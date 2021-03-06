using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSelectProcess : IProcessState {
	public IProcessState BackProcess() {
		return this;
	}

	public IProcessState NextProcess() {
		return new AttackCommandSelectProcess();
	}

	public IProcessState Update(BattleManager mgr) {
		AllEventManager eventMgr = AllEventManager.GetInstance();
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();

		//敵の思考時間の処理
		EnemyBattleData.GetInstance().ThinkingTimeCounter();

		//モンスターが交換されていたら
		if (PlayerBattleData.GetInstance().changeMonsterActive_) {
			//モンスターが変更されていたら
			if (PlayerBattleData.GetInstance().changeMonsterNumber_ > 0) {
				//アイドル状態の停止
				mgr.GetPlayerStatusInfoParts().ProcessIdleEnd();
				mgr.GetPlayerMonsterParts().ProcessIdleEnd();

				//フェードイン
				eventMgr.EventSpriteRendererSet(
					sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
					, null
					, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 0)
					);
				eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
				eventMgr.AllUpdateEventExecute(0.4f);

				//イベントの最後
				eventMgr.EventFinishSet();

				return new EnemyCommandSelectProcess();
			}
			else {
				mgr.ActiveUiCommand();
				mgr.InactiveUiCommand();

				//フェードイン
				eventMgr.EventSpriteRendererSet(
					sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
					, null
					, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 0)
					);
				eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
				eventMgr.AllUpdateEventExecute(0.4f);

				eventMgr.UpdateGameObjectSet(mgr.GetCursorParts().GetEventGameObject());
				eventMgr.UpdateGameObjectSet(mgr.GetNovelWindowParts().GetCommandParts().GetEventGameObject());
				eventMgr.UpdateGameObjectsActiveSetExecute(true);

				//dpが100以上だったら
				if (PlayerBattleData.GetInstance().dreamPoint_ >= 100) {
					eventMgr.EventTextSet(mgr.GetNovelWindowParts().GetEventText()
						, "ゆめたちが　\n"
						+ "きょうめいしている・・・");
					eventMgr.EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
					eventMgr.AllUpdateEventExecute();
				}
				else {
					eventMgr.EventTextSet(mgr.GetNovelWindowParts().GetEventText(), PlayerBattleData.GetInstance().GetMonsterDatas(0).uniqueName_ + "は　どうする？");
					eventMgr.EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
					eventMgr.AllUpdateEventExecute();
				}

				eventMgr.EventStatusInfoPartsSet(mgr.GetPlayerStatusInfoParts(), new Color32(0, 0, 0, 0));
				eventMgr.StatusInfoPartsUpdateExecuteSet(StatusInfoPartsEventManagerExecute.IdleMoveStart);
				eventMgr.AllUpdateEventExecute();

				//イベントの最後
				eventMgr.EventFinishSet();

				PlayerBattleData.GetInstance().changeMonsterActive_ = false;
			}
		}

		//やけどのダメージ処理
		if(mgr.BurnsDamageProcess(PlayerBattleData.GetInstance(), mgr.GetPlayerStatusInfoParts(), mgr.GetPlayerMonsterParts())) {
			return new CommandEventExecuteProcess();
		}

		if (EnemyBattleData.GetInstance().GetThinkingEnd() == false) {
			//やけどのダメージ処理
			if(mgr.BurnsDamageProcess(EnemyBattleData.GetInstance(), mgr.GetEnemyStatusInfoParts(), mgr.GetEnemyMonsterParts())) {
				return new CommandEventExecuteProcess();
			}

			if (EnemyBattleData.GetInstance().PoinsonCounter()) {
				//どくのダメージ処理
				mgr.PoisonDamageProcess(EnemyBattleData.GetInstance(), mgr.GetEnemyStatusInfoParts(), mgr.GetEnemyMonsterParts());
				if (mgr.PoisonDamageDown()) return new CommandEventExecuteProcess();
			}
		}

		if (AllEventManager.GetInstance().EventUpdate()) {
			sceneMgr.inputProvider_ = new KeyBoardNormalTriggerInputProvider();
		}

		if (sceneMgr.inputProvider_.UpSelect()) {
			mgr.nowCommandState_ = mgr.nowCommandState_.UpSelect(mgr);
			if (mgr.PoisonDamageDown()) return new CommandEventExecuteProcess();
		}
		else if (sceneMgr.inputProvider_.DownSelect()) {
			mgr.nowCommandState_ = mgr.nowCommandState_.DownSelect(mgr);
			if (mgr.PoisonDamageDown()) return new CommandEventExecuteProcess();
		}
		else if (sceneMgr.inputProvider_.RightSelect()) {
			mgr.nowCommandState_ = mgr.nowCommandState_.RightSelect(mgr);
			if (mgr.PoisonDamageDown()) return new CommandEventExecuteProcess();
		}
		else if (sceneMgr.inputProvider_.LeftSelect()) {
			mgr.nowCommandState_ = mgr.nowCommandState_.LeftSelect(mgr);
			if (mgr.PoisonDamageDown()) return new CommandEventExecuteProcess();
		}
		else if (sceneMgr.inputProvider_.SelectEnter()) {
			return mgr.nowCommandState_.Execute(mgr);
		}
		else if (sceneMgr.inputProvider_.SelectNovelWindowActive()) {
			mgr.GetNovelWindowPartsActiveState().state_ = mgr.GetNovelWindowPartsActiveState().Next(mgr);
		}

		return this;
	}
}
