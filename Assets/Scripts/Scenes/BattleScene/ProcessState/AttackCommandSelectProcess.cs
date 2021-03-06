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

				AllEventManager.GetInstance().UpdateGameObjectSet(mgr.GetCursorParts().GetEventGameObject());
				AllEventManager.GetInstance().UpdateGameObjectSet(mgr.GetNovelWindowParts().GetCommandParts().GetEventGameObject());
				AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(true);

				//dpが100以上だったら
				if (PlayerBattleData.GetInstance().dreamPoint_ >= 100) {
					AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetEventText()
						, "ゆめたちが　\n"
						+ "きょうめいしている・・・");
					AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
					AllEventManager.GetInstance().AllUpdateEventExecute();
				}
				else {
					AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetEventText(), PlayerBattleData.GetInstance().GetMonsterDatas(0).uniqueName_ + "は　どうする？");
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
			mgr.nowAttackCommandState_ = mgr.nowAttackCommandState_.UpSelect(mgr);
			if (mgr.PoisonDamageDown()) return new CommandEventExecuteProcess();
		}
		else if (allSceneMgr.inputProvider_.DownSelect()) {
			mgr.nowAttackCommandState_ = mgr.nowAttackCommandState_.DownSelect(mgr);
			if (mgr.PoisonDamageDown()) return new CommandEventExecuteProcess();
		}
		else if (allSceneMgr.inputProvider_.RightSelect()) {
			mgr.nowAttackCommandState_ = mgr.nowAttackCommandState_.RightSelect(mgr);
			if (mgr.PoisonDamageDown()) return new CommandEventExecuteProcess();
		}
		else if (allSceneMgr.inputProvider_.LeftSelect()) {
			mgr.nowAttackCommandState_ = mgr.nowAttackCommandState_.LeftSelect(mgr);
			if (mgr.PoisonDamageDown()) return new CommandEventExecuteProcess();
		}
		else if (allSceneMgr.inputProvider_.SelectEnter()) {
			ISkillData playerSkillData = PlayerBattleData.GetInstance().GetMonsterDatas(0).GetSkillDatas(mgr.playerSelectSkillNumber_);

			if (playerSkillData.nowPlayPoint_ > 0
				&& playerSkillData.skillNumber_ != (int)SkillDataNumber.None) {
				//SE
				mgr.GetInputSoundProvider().SelectEnter();

				mgr.GetPlayerStatusInfoParts().ProcessIdleEnd();
				mgr.GetPlayerMonsterParts().ProcessIdleEnd();

				allSceneMgr.inputProvider_ = new InactiveInputProvider();

				//コマンドUIの非表示
				mgr.InactiveUiAttackCommand();

				//ppの消費
				playerSkillData.nowPlayPoint_ -= 1;

				//dpが100以下だったら
				if (PlayerBattleData.GetInstance().dreamPoint_ <= 100) {
					//dpの変動
					PlayerBattleData.GetInstance().dreamPoint_ += playerSkillData.upDpValue_;
				}

				//イベントの最後
				AllEventManager.GetInstance().EventFinishSet();

				return mgr.nowAttackCommandState_.Execute(mgr);
			}
		}
		else if (allSceneMgr.inputProvider_.SelectBack()) {
			//こんらん状態なら
			if (PlayerBattleData.GetInstance().GetMonsterDatas(0).battleData_.firstAbnormalState_.state_ == AbnormalType.Confusion
				|| PlayerBattleData.GetInstance().GetMonsterDatas(0).battleData_.secondAbnormalState_.state_ == AbnormalType.Confusion) {
				allSceneMgr.inputProvider_ = new KeyBoardNormalTriggerInputProvider();
			}

			mgr.ChangeUiCommand();

			return mgr.nowProcessState().BackProcess();
		}
		else if (allSceneMgr.inputProvider_.SelectNovelWindowActive()) {
			mgr.GetNovelWindowPartsActiveState().state_ = mgr.GetNovelWindowPartsActiveState().Next(mgr);
		}

		return this;
	}
}
