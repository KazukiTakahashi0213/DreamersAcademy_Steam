using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterData {
	IMonsterTribesData tribesData_ { get; }
	IMonsterBattleData battleData_ { get; set; }
	int exp_ { get; }
	int level_ { get; }
	string uniqueName_ { get; set; }
	int nowHitPoint_ { get; set; }
	bool battleActive_ { get; set; }

	ISkillData GetSkillDatas(int number);
	int GetSkillSize();

	int RealHitPoint();
	int RealAttack();
	int RealDefense();
	int RealSpeed();

	void SkillAdd(SkillData addSkill);
	void SkillSet(SkillData setSkill, int number);
	void SkillSwap(int changeNumber, int baseNumber);

	void BattleDataReset();

	float ElementSimillarChecker(ElementTypeState checkElementType);
	int ElementSimillarCheckerForValue(ElementTypeState checkElementType);

	bool SkillTradeCheck(ElementType skillElementType);
}
