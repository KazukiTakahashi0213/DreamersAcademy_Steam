using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTradeSelectCommandSelectProcess : IProcessState {
	private BattleSceneMonsterTradeSelectCommandExecuteProvider executeProvider_ = new BattleSceneMonsterTradeSelectCommandExecuteProvider(BattleSceneMonsterTradeSelectCommandExecuteState.Yes);

	public IProcessState BackProcess() {
		return new StartCommandSelectProcess();
	}

	public IProcessState NextProcess() {
		return new CommandSelectProcess();
	}

	public IProcessState Update(BattleManager mgr) {
		AllEventManager eventMgr = AllEventManager.GetInstance();
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();

		//敵の思考時間の処理
		EnemyBattleData.GetInstance().ThinkingTimeCounter();

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
		int commandSelectNumber = mgr.GetMonsterTradeSelectCommandParts().CommandSelectForNumber(new Vector3(4.16f, 0, 0), new Vector3());
		if (commandSelectNumber > -1) {
			//SE
			mgr.GetInputSoundProvider().UpSelect();

			executeProvider_.state_ = (BattleSceneMonsterTradeSelectCommandExecuteState)mgr.GetMonsterTradeSelectCommandParts().SelectNumber() + 1;

			//どくのダメージ処理
			mgr.PoisonDamageProcess(PlayerBattleData.GetInstance(), mgr.GetPlayerStatusInfoParts(), mgr.GetPlayerMonsterParts());

		}
		else if (sceneMgr.inputProvider_.UpSelect()) {

		}
		else if (sceneMgr.inputProvider_.DownSelect()) {

		}
		else if (sceneMgr.inputProvider_.RightSelect()) {
			//選択肢が動かせたら
			if (mgr.GetMonsterTradeSelectCommandParts().CommandSelectRight(new Vector3(4.16f, 0, 0))) {
				//SE
				mgr.GetInputSoundProvider().RightSelect();

				executeProvider_.state_ = (BattleSceneMonsterTradeSelectCommandExecuteState)mgr.GetMonsterTradeSelectCommandParts().SelectNumber() + 1;

				//どくのダメージ処理
				mgr.PoisonDamageProcess(PlayerBattleData.GetInstance(), mgr.GetPlayerStatusInfoParts(), mgr.GetPlayerMonsterParts());
			}
		}
		else if (sceneMgr.inputProvider_.LeftSelect()) {
			//選択肢が動かせたら
			if (mgr.GetMonsterTradeSelectCommandParts().CommandSelectLeft(new Vector3(-4.16f, 0, 0))) {
				//SE
				mgr.GetInputSoundProvider().LeftSelect();

				executeProvider_.state_ = (BattleSceneMonsterTradeSelectCommandExecuteState)mgr.GetMonsterTradeSelectCommandParts().SelectNumber() + 1;

				//どくのダメージ処理
				mgr.PoisonDamageProcess(PlayerBattleData.GetInstance(), mgr.GetPlayerStatusInfoParts(), mgr.GetPlayerMonsterParts());
			}
		}
		else if (sceneMgr.inputProvider_.SelectEnter()
			|| mgr.GetMonsterTradeSelectCommandParts().MouseLeftButtonTriggerActive()) {
			return executeProvider_.Execute(mgr);
		}
		else if (sceneMgr.inputProvider_.SelectBack()
			|| sceneMgr.inputProvider_.SelectMouseRightTrigger()) {
			mgr.InactiveUiMonsterTradeSelectCommand();
			mgr.ActiveUiStartCommand();

			return mgr.nowProcessState().BackProcess();
		}

		//どくで倒れたら
		if (mgr.PoisonDamageDown()) return new CommandEventExecuteProcess();

		return this;
	}
}
