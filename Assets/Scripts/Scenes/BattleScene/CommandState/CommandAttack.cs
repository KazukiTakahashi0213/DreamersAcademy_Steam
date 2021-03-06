using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandAttack : ICommandState {
	public ICommandState DownSelect(BattleManager mgr) {
		//SE
		mgr.GetInputSoundProvider().DownSelect();

		//どくのダメージ処理
		mgr.PoisonDamageProcess(PlayerBattleData.GetInstance(), mgr.GetPlayerStatusInfoParts(), mgr.GetPlayerMonsterParts());

		mgr.CommandDownCursorMove();
		return new CommandMonsterTrade();
	}
	public ICommandState LeftSelect(BattleManager mgr) {
		return this;
	}
	public ICommandState RightSelect(BattleManager mgr) {
		//SE
		mgr.GetInputSoundProvider().RightSelect();

		//どくのダメージ処理
		mgr.PoisonDamageProcess(PlayerBattleData.GetInstance(), mgr.GetPlayerStatusInfoParts(), mgr.GetPlayerMonsterParts());

		mgr.CommandRightCursorMove();
		return new CommandDream();
	}
	public ICommandState UpSelect(BattleManager mgr) {
		return this;
	}

	public IProcessState Execute(BattleManager mgr) {
		//SE
		mgr.GetInputSoundProvider().SelectEnter();

		//こんらんの処理の初期化
		mgr.ConfusionProcessStart();

		mgr.ChangeUiAttackCommand();

		return mgr.nowProcessState().NextProcess();
	}
}
