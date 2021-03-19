using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BEffectValueType {
	public virtual bool EffectValueEventSet(BattleManager mgr, BTrainerBattleData attackTrainerBattleData, BTrainerBattleData defenseTrainerBattleData, EffectParts attackEffectParts, EffectParts defenseEffectParts, MonsterParts defenseMonsterParts, StatusInfoParts attackStatusInfoParts, StatusInfoParts defenseStatusInfoParts, DreamPointInfoParts attackDreamPointInfoParts, DreamPointInfoParts defenseDreamPointInfoParts, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData) {
		return false;
	}
}
