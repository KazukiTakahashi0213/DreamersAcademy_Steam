using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommandParts : MonoBehaviour {
	[SerializeField] CommandParts commandParts_ = null;
	[SerializeField] CommandWindowParts skillInfoParts_ = null;

	public CommandParts GetCommandParts() { return commandParts_; }
	public CommandWindowParts GetSkillInfoParts() { return skillInfoParts_; }

	public void MonsterDataReflect(IMonsterData monsterData, IMonsterData enemyMonsterData) {
		//技をTextに反映
		for (int i = 0; i < commandParts_.GetCommandWindowTextsCount(); ++i) {
			commandParts_.GetCommandWindowTexts(i).text = "　" + t13.Utility.StringFullSpaceBackTamp(monsterData.GetSkillDatas(i).skillName_, 7);
		}

		//文字の色の変更
		//for (int i = 0; i < commandParts_.GetCommandWindowTextsCount(); ++i) {
		//	int simillarResult = enemyMonsterData.ElementSimillarCheckerForValue(monsterData.GetSkillDatas(i).elementType_);
		//
		//	if (simillarResult == 0) commandParts_.GetCommandWindowTexts(i).color = new Color32(195, 195, 195, 255);
		//	else if (simillarResult == 1) commandParts_.GetCommandWindowTexts(i).color = new Color32(52, 130, 207, 255);
		//	else if (simillarResult == 2) commandParts_.GetCommandWindowTexts(i).color = new Color32(50, 50, 50, 255);
		//	else if (simillarResult == 3) commandParts_.GetCommandWindowTexts(i).color = new Color32(207, 52, 112, 255);
		//}
		for (int i = 0; i < commandParts_.GetCommandWindowTextsCount(); ++i) {
			commandParts_.GetCommandWindowTexts(i).color = monsterData.GetSkillDatas(i).elementType_.GetColor();
		}
	}
}
