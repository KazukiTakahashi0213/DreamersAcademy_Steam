using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMenuSceneBattleProcessStateProvider : BMonsterMenuSceneProcessStateProvider {
	public MonsterMenuSceneBattleProcessStateProvider() {
		states_.Add(new MonsterMenuSceneBattleProcessNone());
		states_.Add(new MonsterMenuSceneBattleProcessMonsterSelect());
		states_.Add(new MonsterMenuSceneBattleProcessMonsterActionSelect());
		states_.Add(new MonsterMenuSceneBattleProcessSkillSelect());
		states_.Add(new MonsterMenuSceneBattleProcessSkillActionSelect());
	}

	public override void init(MonsterMenuManager monsterMenuManager) {
		PlayerBattleData playerData = PlayerBattleData.GetInstance();

		monsterMenuManager.GetMonsterActionCommandParts().GetCommandWindowTexts(0).text = "　こうかん";

		//StatusInfosPartsのモンスター情報の変更
		for (int i = 0; i < playerData.GetMonsterDatasLength() / 2; ++i) {
			if (i == 0) {
				monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() / 2).MonsterStatusInfoSet(playerData.GetMonsterDatas(i));
			}
			else {
				monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(i + monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() / 2).MonsterStatusInfoSet(playerData.GetMonsterDatas(i));
				monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(-i + monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() / 2).MonsterStatusInfoSet(playerData.GetMonsterDatas(playerData.GetMonsterDatasLength() - i));
			}
		}

		//MagazinePartsのSDの画像の変更
		for (int i = 0; i < monsterMenuManager.GetMagazineParts().GetMonsterSDsPartsCount(); ++i) {
			monsterMenuManager.GetMagazineParts().GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().sprite = playerData.GetMonsterDatas(i).tribesData_.SDTex_;
		}

		//モンスターのパラメーターの反映
		monsterMenuManager.GetParameterInfoFrameParts().MonsterDataReflect(playerData.GetMonsterDatas(0));

		//モンスターの技の名前の反映
		for (int i = 0; i < monsterMenuManager.GetSkillCommandParts().GetCommandWindowTextsCount(); ++i) {
			monsterMenuManager.GetSkillCommandParts().GetCommandWindowTexts(i).text = "　" + playerData.GetMonsterDatas(0).GetSkillDatas(i).skillName_;
		}
	}
}
