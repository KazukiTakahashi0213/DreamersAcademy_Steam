using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneCommandExecuteDream : BBattleSceneCommandExecute {
	public override IProcessState Execute(BattleManager battleManager) {
		//既にパワーアップしていたら
		if (PlayerBattleData.GetInstance().GetMonsterDatas(0).battleData_.HaveAbnormalType(AbnormalType.Hero)) return battleManager.nowProcessState();

		//dpが100以上だったら
		if (PlayerBattleData.GetInstance().GetDreamPoint() >= 100) {
			//SE
			battleManager.GetInputSoundProvider().SelectEnter();

			if (PlayerBattleData.GetInstance().dreamSyncronize_ == false) {
				//ゆめの文字色の変更
				battleManager.GetCommandCommandParts().GetCommandWindowTexts(1).color = new Color32(94, 120, 255, 255);

				//パワーアップするか否かのフラグの設定
				PlayerBattleData.GetInstance().dreamSyncronize_ = true;
			}
			else {
				//ゆめの文字色の変更
				battleManager.GetCommandCommandParts().GetCommandWindowTexts(1).color = new Color32(50, 50, 50, 255);

				//パワーアップするか否かのフラグの設定
				PlayerBattleData.GetInstance().dreamSyncronize_ = false;
			}
		}

		return battleManager.nowProcessState();
	}
}
