using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectValueTypeState {
	None
	, Support
	, HitPointDamage
	, HitPointHeal
	, DreamPointDrain
}

public class EffectValueTypeProvider {
	public EffectValueTypeProvider() {
		states_.Add(new EffectValueTypeNone());
		states_.Add(new EffectValueTypeSupport());
		states_.Add(new EffectValueTypeHitPointDamage());
		states_.Add(new EffectValueTypeHitPointHeal());
		states_.Add(new EffectValueTypeDreamPointDrain());
	}

	public EffectValueTypeState state_ = EffectValueTypeState.None;

	private List<BEffectValueType> states_ = new List<BEffectValueType>();

	public bool EffectValueEventSet(BattleManager mgr, BTrainerBattleData attackTrainerBattleData, BTrainerBattleData defenseTrainerBattleData, EffectParts attackEffectParts, EffectParts defenseEffectParts, MonsterParts defenseMonsterParts, StatusInfoParts attackStatusInfoParts, StatusInfoParts defenseStatusInfoParts, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData) { return states_[(int)state_].EffectValueEventSet(mgr, attackTrainerBattleData, defenseTrainerBattleData, attackEffectParts, defenseEffectParts, defenseMonsterParts, attackStatusInfoParts, defenseStatusInfoParts, attackMonsterData, attackSkillData, defenseMonsterData); }
}
