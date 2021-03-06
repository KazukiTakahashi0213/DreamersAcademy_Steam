using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDream : ICommandState {
	public ICommandState DownSelect(BattleManager mgr) {
		//SE
		mgr.GetInputSoundProvider().DownSelect();

		//どくのダメージ処理
		mgr.PoisonDamageProcess(PlayerBattleData.GetInstance(), mgr.GetPlayerStatusInfoParts(), mgr.GetPlayerMonsterParts());

		mgr.CommandDownCursorMove();
		return new CommandEscape();
	}
	public ICommandState LeftSelect(BattleManager mgr) {
		//SE
		mgr.GetInputSoundProvider().LeftSelect();

		//どくのダメージ処理
		mgr.PoisonDamageProcess(PlayerBattleData.GetInstance(), mgr.GetPlayerStatusInfoParts(), mgr.GetPlayerMonsterParts());

		mgr.CommandLeftCursorMove();
		return new CommandAttack();
	}
	public ICommandState RightSelect(BattleManager mgr) {
		return this;
	}
	public ICommandState UpSelect(BattleManager mgr) {
		return this;
	}

	public IProcessState Execute(BattleManager mgr) {
		//既にパワーアップしていたら
		if (PlayerBattleData.GetInstance().GetMonsterDatas(0).battleData_.HaveAbnormalType(AbnormalType.Hero)) return mgr.nowProcessState();

		//dpが100以上だったら
		if (PlayerBattleData.GetInstance().dreamPoint_ >= 100) {
			//SE
			mgr.GetInputSoundProvider().SelectEnter();

			if (PlayerBattleData.GetInstance().dreamSyncronize_ == false) {
				//ゆめの文字色の変更
				mgr.GetNovelWindowParts().GetCommandParts().GetCommandWindowTexts(1).color = new Color32(94, 120, 255, 255);

				//パワーアップするか否かのフラグの設定
				PlayerBattleData.GetInstance().dreamSyncronize_ = true;
			}
			else {
				//ゆめの文字色の変更
				mgr.GetNovelWindowParts().GetCommandParts().GetCommandWindowTexts(1).color = new Color32(50, 50, 50, 255);

				//パワーアップするか否かのフラグの設定
				PlayerBattleData.GetInstance().dreamSyncronize_ = false;
			}
		}

		return mgr.nowProcessState();
	}
}
