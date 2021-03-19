using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCommandSelectProcess : IProcessState {
	private BattleSceneStartCommandExecuteProvider executeProvider_ = new BattleSceneStartCommandExecuteProvider(BattleSceneStartCommandExecuteState.Attack);

	public IProcessState BackProcess() {
		return this;
	}

	public IProcessState NextProcess() {
		return new MonsterTradeSelectCommandSelectProcess();
	}

	public IProcessState Update(BattleManager mgr) {
		AllEventManager eventMgr = AllEventManager.GetInstance();
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();

		//敵の思考時間の処理
		EnemyBattleData.GetInstance().ThinkingTimeCounter();

		//モンスターが交換されていたら
		if (PlayerBattleData.GetInstance().changeMonsterActive_) {
			executeProvider_.state_ = BattleSceneStartCommandExecuteState.Attack;

			mgr.ActiveUiStartCommand();
			mgr.InactiveUiStartCommand();

			//フェードイン
			eventMgr.EventSpriteRendererSet(
				sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
				, null
				, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 0)
				);
			eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
			eventMgr.AllUpdateEventExecute(0.4f);

			eventMgr.UpdateGameObjectSet(mgr.GetStartCommandParts().GetEventGameObject());
			eventMgr.UpdateGameObjectsActiveSetExecute(true);

			//dpが100以上だったら
			if (PlayerBattleData.GetInstance().GetDreamPoint() >= 100) {
				eventMgr.EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText()
					, "ゆめたちが　\n"
					+ "きょうめいしている・・・");
				eventMgr.EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				eventMgr.AllUpdateEventExecute();
			}
			else {
				eventMgr.EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), PlayerBattleData.GetInstance().GetMonsterDatas(0).uniqueName_ + "は　どうする？");
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

		//やけどのダメージ処理
		if (mgr.BurnsDamageProcess(PlayerBattleData.GetInstance(), mgr.GetPlayerStatusInfoParts(), mgr.GetPlayerMonsterParts())) {
			return new CommandEventExecuteProcess();
		}

		if (EnemyBattleData.GetInstance().GetThinkingEnd() == false) {
			//やけどのダメージ処理
			if (mgr.BurnsDamageProcess(EnemyBattleData.GetInstance(), mgr.GetEnemyStatusInfoParts(), mgr.GetEnemyMonsterParts())) {
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

		//カーソルが動いていたら
		int commandSelectNumber = mgr.GetStartCommandParts().CommandSelectForNumber(new Vector3(), new Vector3(0, 1.32f, 0));
		if (commandSelectNumber > -1) {
			//SE
			mgr.GetInputSoundProvider().UpSelect();

			executeProvider_.state_ = (BattleSceneStartCommandExecuteState)mgr.GetStartCommandParts().SelectNumber() + 1;

			//どくのダメージ処理
			mgr.PoisonDamageProcess(PlayerBattleData.GetInstance(), mgr.GetPlayerStatusInfoParts(), mgr.GetPlayerMonsterParts());

		}
		else if (sceneMgr.inputProvider_.UpSelect()) {
			//選択肢が動かせたら
			if (mgr.GetStartCommandParts().CommandSelectUp(new Vector3(0, 1.33f, 0))) {
				//SE
				mgr.GetInputSoundProvider().UpSelect();

				executeProvider_.state_ = (BattleSceneStartCommandExecuteState)mgr.GetStartCommandParts().SelectNumber() + 1;

				//どくのダメージ処理
				mgr.PoisonDamageProcess(PlayerBattleData.GetInstance(), mgr.GetPlayerStatusInfoParts(), mgr.GetPlayerMonsterParts());
			}
		}
		else if (sceneMgr.inputProvider_.DownSelect()) {
			//選択肢が動かせたら
			if (mgr.GetStartCommandParts().CommandSelectDown(new Vector3(0, -1.33f, 0))) {
				//SE
				mgr.GetInputSoundProvider().DownSelect();

				executeProvider_.state_ = (BattleSceneStartCommandExecuteState)mgr.GetStartCommandParts().SelectNumber() + 1;

				//どくのダメージ処理
				mgr.PoisonDamageProcess(PlayerBattleData.GetInstance(), mgr.GetPlayerStatusInfoParts(), mgr.GetPlayerMonsterParts());
			}
		}
		else if (sceneMgr.inputProvider_.RightSelect()) {

		}
		else if (sceneMgr.inputProvider_.LeftSelect()) {

		}
		else if (sceneMgr.inputProvider_.SelectEnter()
			|| mgr.GetStartCommandParts().MouseLeftButtonTriggerActive()) {
			return executeProvider_.Execute(mgr);
		}

		//どくで倒れたら
		if (mgr.PoisonDamageDown()) return new CommandEventExecuteProcess();

		return this;
	}
}
