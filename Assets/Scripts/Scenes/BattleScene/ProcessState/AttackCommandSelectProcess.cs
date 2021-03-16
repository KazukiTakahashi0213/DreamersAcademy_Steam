using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommandSelectProcess : IProcessState {
	public IProcessState BackProcess() {
		return new CommandSelectProcess();
	}

	public IProcessState NextProcess() {
		return new EnemyCommandSelectProcess();
	}

	public IProcessState Update(BattleManager mgr) {
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();

		//敵の思考時間の処理
		EnemyBattleData.GetInstance().ThinkingTimeCounter();

		//モンスターが交換されていたら
		if (PlayerBattleData.GetInstance().changeMonsterActive_ == true) {
			if (PlayerBattleData.GetInstance().changeMonsterNumber_ > 0) {
				//アイドル状態の停止
				mgr.GetPlayerStatusInfoParts().ProcessIdleEnd();
				mgr.GetPlayerMonsterParts().ProcessIdleEnd();

				//イベントの最後
				AllEventManager.GetInstance().EventFinishSet();

				return new EnemyCommandSelectProcess();
			}
			else {
				allSceneMgr.inputProvider_ = new InactiveInputProvider();

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

				AllEventManager.GetInstance().UpdateGameObjectSet(mgr.GetCommandCommandParts().GetEventGameObject());
				AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(true);

				//dpが100以上だったら
				if (PlayerBattleData.GetInstance().GetDreamPoint() >= 100) {
					AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText()
						, "ゆめたちが　\n"
						+ "きょうめいしている・・・");
					AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
					AllEventManager.GetInstance().AllUpdateEventExecute();
				}
				else {
					AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), PlayerBattleData.GetInstance().GetMonsterDatas(0).uniqueName_ + "は　どうする？");
					AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
					AllEventManager.GetInstance().AllUpdateEventExecute();
				}

				AllEventManager.GetInstance().EventStatusInfoPartsSet(mgr.GetPlayerStatusInfoParts(), new Color32(0, 0, 0, 0));
				AllEventManager.GetInstance().StatusInfoPartsUpdateExecuteSet(StatusInfoPartsEventManagerExecute.IdleMoveStart);
				AllEventManager.GetInstance().AllUpdateEventExecute();

				AllEventManager.GetInstance().EventFinishSet();

				PlayerBattleData.GetInstance().changeMonsterActive_ = false;
			}
		}

		//やけどのダメージ処理
		if(mgr.BurnsDamageProcess(PlayerBattleData.GetInstance(), mgr.GetPlayerStatusInfoParts(), mgr.GetPlayerMonsterParts())){
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

		//こんらんの処理
		mgr.ConfusionProcess();

		if (AllEventManager.GetInstance().EventUpdate()) {
			allSceneMgr.inputProvider_ = new KeyBoardNormalTriggerInputProvider();
		}

		if (allSceneMgr.inputProvider_.UpSelect()) {
			//選択肢が動かせたら
			if (mgr.GetAttackCommandParts().GetCommandParts().CommandSelectUp(new Vector3(0, 0.83f, 0))) {
				//SE
				mgr.GetInputSoundProvider().UpSelect();

				mgr.AttackCommandSkillInfoTextSet(mgr.GetAttackCommandParts().GetCommandParts().SelectNumber());

				//どくのダメージ処理
				mgr.PoisonDamageProcess(PlayerBattleData.GetInstance(), mgr.GetPlayerStatusInfoParts(), mgr.GetPlayerMonsterParts());
			}
		}
		else if (allSceneMgr.inputProvider_.DownSelect()) {
			//選択肢が動かせたら
			if (mgr.GetAttackCommandParts().GetCommandParts().CommandSelectDown(new Vector3(0, -0.83f, 0))) {
				//SE
				mgr.GetInputSoundProvider().DownSelect();

				mgr.AttackCommandSkillInfoTextSet(mgr.GetAttackCommandParts().GetCommandParts().SelectNumber());

				//どくのダメージ処理
				mgr.PoisonDamageProcess(PlayerBattleData.GetInstance(), mgr.GetPlayerStatusInfoParts(), mgr.GetPlayerMonsterParts());
			}
		}
		else if (allSceneMgr.inputProvider_.RightSelect()) {
			//選択肢が動かせたら
			if (mgr.GetAttackCommandParts().GetCommandParts().CommandSelectRight(new Vector3(5.56f, 0, 0))) {
				//SE
				mgr.GetInputSoundProvider().RightSelect();

				mgr.AttackCommandSkillInfoTextSet(mgr.GetAttackCommandParts().GetCommandParts().SelectNumber());

				//どくのダメージ処理
				mgr.PoisonDamageProcess(PlayerBattleData.GetInstance(), mgr.GetPlayerStatusInfoParts(), mgr.GetPlayerMonsterParts());
			}
		}
		else if (allSceneMgr.inputProvider_.LeftSelect()) {
			//選択肢が動かせたら
			if (mgr.GetAttackCommandParts().GetCommandParts().CommandSelectLeft(new Vector3(-5.56f, 0, 0))) {
				//SE
				mgr.GetInputSoundProvider().LeftSelect();

				mgr.AttackCommandSkillInfoTextSet(mgr.GetAttackCommandParts().GetCommandParts().SelectNumber());

				//どくのダメージ処理
				mgr.PoisonDamageProcess(PlayerBattleData.GetInstance(), mgr.GetPlayerStatusInfoParts(), mgr.GetPlayerMonsterParts());
			}
		}
		else if (allSceneMgr.inputProvider_.SelectEnter()) {
			mgr.playerSelectSkillNumber_ = mgr.GetAttackCommandParts().GetCommandParts().SelectNumber();

			ISkillData playerSkillData = PlayerBattleData.GetInstance().GetMonsterDatas(0).GetSkillDatas(mgr.playerSelectSkillNumber_);

			//スキルがNoneでなかったら
			if (//playerSkillData.nowPlayPoint_ > 0
				playerSkillData.skillNumber_ != (int)SkillDataNumber.None) {
				//SE
				mgr.GetInputSoundProvider().SelectEnter();

				mgr.GetPlayerStatusInfoParts().ProcessIdleEnd();
				mgr.GetPlayerMonsterParts().ProcessIdleEnd();

				allSceneMgr.inputProvider_ = new InactiveInputProvider();

				//コマンドUIの非表示
				mgr.InactiveUiAttackCommand();

				//ppの消費
				//playerSkillData.nowPlayPoint_ -= 1;

				//イベントの最後
				AllEventManager.GetInstance().EventFinishSet();

				return mgr.nowProcessState().NextProcess();
			}
		}
		else if (allSceneMgr.inputProvider_.SelectBack()) {
			//こんらん状態なら
			if (PlayerBattleData.GetInstance().GetMonsterDatas(0).battleData_.firstAbnormalState_.state_ == AbnormalType.Confusion
				|| PlayerBattleData.GetInstance().GetMonsterDatas(0).battleData_.secondAbnormalState_.state_ == AbnormalType.Confusion) {
				allSceneMgr.inputProvider_ = new KeyBoardNormalTriggerInputProvider();
			}

			mgr.InactiveUiAttackCommand();
			mgr.ActiveUiCommand();

			return mgr.nowProcessState().BackProcess();
		}
		else if (allSceneMgr.inputProvider_.SelectNovelWindowActive()) {

		}

		//どくで倒れたら
		if (mgr.PoisonDamageDown()) return new CommandEventExecuteProcess();

		return this;
	}
}
