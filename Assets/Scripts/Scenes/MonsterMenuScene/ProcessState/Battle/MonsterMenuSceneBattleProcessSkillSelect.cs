using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMenuSceneBattleProcessSkillSelect : BMonsterMenuSceneProcessState {
    public override MonsterMenuSceneProcess Update(MonsterMenuManager monsterMenuManager) {
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();
		AllEventManager eventMgr = AllEventManager.GetInstance();
		PlayerBattleData playerData = PlayerBattleData.GetInstance();

		eventMgr.EventUpdate();

		if (sceneMgr.inputProvider_.UpSelect()) {
			//選択肢が動かせたら
			if (monsterMenuManager.GetSkillCommandParts().CommandSelect(-2, new Vector3(0, 1.72f, 0))) {
				//SE
				monsterMenuManager.GetInputSoundProvider().UpSelect();

				//技の情報の反映
				monsterMenuManager.GetSkillInfoFrameParts().SkillInfoReflect(playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).GetSkillDatas(monsterMenuManager.GetSkillCommandParts().GetSelectNumber()));
			}
		}
		else if (sceneMgr.inputProvider_.DownSelect()) {
			//選択肢が動かせたら
			if (monsterMenuManager.GetSkillCommandParts().CommandSelect(2, new Vector3(0, -1.72f, 0))) {
				//SE
				monsterMenuManager.GetInputSoundProvider().DownSelect();

				//技の情報の反映
				monsterMenuManager.GetSkillInfoFrameParts().SkillInfoReflect(playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).GetSkillDatas(monsterMenuManager.GetSkillCommandParts().GetSelectNumber()));
			}
		}
		else if (sceneMgr.inputProvider_.RightSelect()) {
			//選択肢が動かせたら
			if (monsterMenuManager.GetSkillCommandParts().CommandSelect(1, new Vector3(6.08f, 0, 0))) {
				//SE
				monsterMenuManager.GetInputSoundProvider().RightSelect();

				//技の情報の反映
				monsterMenuManager.GetSkillInfoFrameParts().SkillInfoReflect(playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).GetSkillDatas(monsterMenuManager.GetSkillCommandParts().GetSelectNumber()));
			}
		}
		else if (sceneMgr.inputProvider_.LeftSelect()) {
			//選択肢が動かせたら
			if (monsterMenuManager.GetSkillCommandParts().CommandSelect(-1, new Vector3(-6.08f, 0, 0))) {
				//SE
				monsterMenuManager.GetInputSoundProvider().LeftSelect();

				//技の情報の反映
				monsterMenuManager.GetSkillInfoFrameParts().SkillInfoReflect(playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).GetSkillDatas(monsterMenuManager.GetSkillCommandParts().GetSelectNumber()));
			}
		}
		else if (sceneMgr.inputProvider_.SelectEnter()) {
		}
		else if (sceneMgr.inputProvider_.SelectBack()) {
			monsterMenuManager.GetSkillCommandParts().GetCursorParts().gameObject.SetActive(false);

			monsterMenuManager.GetParameterInfoFrameParts().gameObject.SetActive(true);
			monsterMenuManager.GetSkillInfoFrameParts().gameObject.SetActive(false);

			//操作の変更
			AllSceneManager.GetInstance().inputProvider_ = new KeyBoardNormalInputProvider();

			//技の選択肢の初期化
			monsterMenuManager.GetSkillCommandParts().SelectReset(new Vector3(-5.29f, 0.82f, 2));

			return MonsterMenuSceneProcess.MonsterSelect;
		}

		return monsterMenuManager.GetNowProcessState().state_;
	}
}
