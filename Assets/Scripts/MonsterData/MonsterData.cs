using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterData : IMonsterData {
	//EntryPoint
	public MonsterData(MonsterTribesData monsterTribes, int exp, int level) {
		tribesData_ = monsterTribes;
		battleData_ = new MonsterBattleData();

		exp_ = exp;
		level_ = level;
		uniqueName_ = monsterTribes.monsterName_;
		nowHitPoint_ = RealHitPoint();
		battleActive_ = true;
	}

	public int exp_ { get; }
	public int level_ { get; }
	public string uniqueName_ { get; set; }
	public int nowHitPoint_ { get; set; }
	public bool battleActive_ { get; set; }

	//技
	private const int SKILL_MAX_SIZE = 4;
	private int skillSize_ = 0;
	private ISkillData[] skillDatas_ = new ISkillData[SKILL_MAX_SIZE] {
		new SkillData(0),
		new SkillData(0),
		new SkillData(0),
		new SkillData(0),
	};
	public ISkillData GetSkillDatas(int number) { return skillDatas_[number]; }
	public int GetSkillSize() { return skillSize_; }

	//種族値、番号、デフォルトの名前、属性、画像
	public IMonsterTribesData tribesData_ { get; }

	//努力値
	private int uniqueHitPoint_ = 0;
	private int uniqueAttack_ = 0;
	private int uniqueDefense_ = 0;
	private int uniqueSpeed_ = 0;

	//バトルで使用するデータ
	public IMonsterBattleData battleData_ { get; set; }

	//実数値
	public int RealHitPoint() { return ((tribesData_.tribesHitPoint_ * 2 + (uniqueHitPoint_ / 4)) * level_ / 100) + level_ + 10; }
	public int RealAttack() { return (int)((tribesData_.tribesAttack_ + (uniqueAttack_ / 4)) * PowerUpAttack()); }
	public int RealDefense() { return (int)((tribesData_.tribesDefense_ + (uniqueDefense_ / 4)) * PowerUpDefense()); }
	public int RealSpeed() { return (int)((tribesData_.tribesSpeed_ + (uniqueSpeed_ / 4)) * PowerUpSpeed()); }
	private float PowerUpAttack() {
		//もしヒーロー状態だったら
		if (battleData_.firstAbnormalState_.state_ == AbnormalType.Hero) {
			return tribesData_.tribesDreamAttack_;
		}

		return 1;
	}
	private float PowerUpDefense() {
		//もしヒーロー状態だったら
		if (battleData_.firstAbnormalState_.state_ == AbnormalType.Hero) {
			return tribesData_.tribesDreamDefense_;
		}

		return 1;
	}
	private float PowerUpSpeed() {
		//もしヒーロー状態だったら
		if (battleData_.firstAbnormalState_.state_ == AbnormalType.Hero) {
			return tribesData_.tribesDreamSpeed_;
		}

		return 1;
	}

	public void SkillAdd(SkillData addSkill) {
		if (skillSize_ == SKILL_MAX_SIZE) return;

		skillDatas_[skillSize_] = addSkill;
		skillSize_ += 1;
	}
	public void SkillSet(SkillData setSkill, int number) {
		if (skillSize_ <= number) return;

		skillDatas_[number] = setSkill;
	}
	public void SkillSwap(int changeNumber, int baseNumber) {
		ISkillData temp = skillDatas_[baseNumber];

		skillDatas_[baseNumber] = skillDatas_[changeNumber];
		skillDatas_[changeNumber] = temp;
	}

	public void BattleDataReset() {
		battleData_ = new MonsterBattleData();
	}

	//タイプ相性
	static private float[,] elementSimillar_ = new float[(int)ElementType.Max, (int)ElementType.Max] {
		  { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f }//None
		, { 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 1.0f }//Fire
		, { 1.0f, 0.5f, 1.0f, 2.0f, 1.0f, 1.0f }//Water
		, { 1.0f, 2.0f, 0.5f, 1.0f, 1.0f, 1.0f }//Tree
		, { 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 2.0f }//Holy
		, { 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f }//Dark
	};
	public float ElementSimillarChecker(ElementTypeState checkElementType) {
		return elementSimillar_[(int)tribesData_.firstElement_.state_, (int)checkElementType.state_] * elementSimillar_[(int)tribesData_.secondElement_.state_, (int)checkElementType.state_];
	}
	/// <summary>
	/// 3:効果抜群 2:効果普通 1:効果今一つ 0:効果無し
	/// </summary>
	public int ElementSimillarCheckerForValue(ElementTypeState checkElementType) {
		float checkResult = ElementSimillarChecker(checkElementType);

		if (checkResult > 1.0f) {
			return 3;
		}
		else if (checkResult < 1.0f
			&& checkResult > 0) {
			return 1;
		}
		else if (checkResult < 0.1f) {
			return 0;
		}

		return 2;
	}

	//技の習得関係
	public bool SkillTradeCheck(ElementType skillElementType) {
		if (tribesData_.firstElement_.SkillTradeCheck(skillElementType)) return true;
		if (tribesData_.secondElement_.SkillTradeCheck(skillElementType)) return true;

		return false;
	}

	/// <summary>
	///メイン計算式
	/// </summary>
	static public int BattleDamageCalculate(IMonsterData attackMonster, IMonsterData defenseMonster, ISkillData attackSkill) {
		//乱数の値
		float[] randomValue = new float[16] {
			0.85f, 0.86f, 0.87f, 0.88f, 0.89f,
			0.90f, 0.91f, 0.92f, 0.93f, 0.94f, 0.95f, 0.96f, 0.97f, 0.98f, 0.99f,
			1.00f,
			};

		//乱数
		float randomResult = randomValue[AllSceneManager.GetInstance().GetRandom().Next(0, 16)];

		//モンスターのタイプと技のタイプの一致か否か
		float typeMatch = 1;
		if (attackMonster.tribesData_.firstElement_.state_ == attackSkill.elementType_.state_
			|| attackMonster.tribesData_.secondElement_.state_ == attackSkill.elementType_.state_) {
			typeMatch = 1.5f;
		}

		//敵モンスターと技のタイプ相性
		float typeSimillar = defenseMonster.ElementSimillarChecker(attackSkill.elementType_);

		//補正値の初期値 4096
		//技の最終威力
		int realSkillPower = (int)(attackSkill.effectValue_ * (4096 / 4096));

		//物理か特殊か
		//EffectAttackTypeState ea = attackSkill.effectValueType_.GetEffectAttackTypeState();

		//モンスターの最終攻撃力
		int realMonsterAttack = 0;
		{
			//if (ea.state_ == EffectAttackType.Normal) {
			//	realMonsterAttack = (int)(attackMonster.RealAttack() * attackMonster.battleData_.RealAttackParameterRank() * (4096 / 4096));
			//}
			realMonsterAttack = (int)(attackMonster.RealAttack() * attackMonster.battleData_.RealAttackParameterRank() * (4096 / 4096));
		}

		//敵モンスターの最終防御力
		int realMonsterDefense = 0;
		{
			//if (ea.state_ == EffectAttackType.Normal) {
			//	realMonsterDefense = (int)(defenseMonster.RealDefense() * defenseMonster.battleData_.RealDefenseParameterRank() * (4096 / 4096));
			//}
			realMonsterDefense = (int)(defenseMonster.RealDefense() * defenseMonster.battleData_.RealDefenseParameterRank() * (4096 / 4096));
		}

		//(((レベル×2/5+2)×威力×A/D)/50+2)
		//最終ダメージ
		int realDamage = (int)((((attackMonster.level_ * 2 / 5 + 2) * realSkillPower * realMonsterAttack / realMonsterDefense) / 50)
					   * randomResult * typeMatch * typeSimillar);

		return realDamage;
	}
	/// <summary>
	///乱数平均、急所なし、計算式
	/// </summary>
	static public int TestDamageCalculate(IMonsterData attackMonster, IMonsterData defenseMonster, ISkillData attackSkill) {
		//乱数
		float randomResult = 9.5f;

		//モンスターのタイプと技のタイプの一致か否か
		float typeMatch = 1;
		if (attackMonster.tribesData_.firstElement_.state_ == attackSkill.elementType_.state_
			|| attackMonster.tribesData_.secondElement_.state_ == attackSkill.elementType_.state_) {
			typeMatch = 1.5f;
		}

		//敵モンスターと技のタイプ相性
		float typeSimillar = defenseMonster.ElementSimillarChecker(attackSkill.elementType_);

		//補正値の初期値 4096
		//技の最終威力
		int realSkillPower = (int)(attackSkill.effectValue_ * (4096 / 4096));

		//物理か特殊か
		//EffectAttackTypeState ea = attackSkill.effectValueType_.GetEffectAttackTypeState();

		//モンスターの最終攻撃力
		int realMonsterAttack = 0;
		{
			//if (ea.state_ == EffectAttackType.Normal) {
			//	realMonsterAttack = (int)(attackMonster.RealAttack() * attackMonster.battleData_.RealAttackParameterRank() * (4096 / 4096));
			//}
			realMonsterAttack = (int)(attackMonster.RealAttack() * attackMonster.battleData_.RealAttackParameterRank() * (4096 / 4096));
		}

		//敵モンスターの最終防御力
		int realMonsterDefense = 0;
		{
			//if (ea.state_ == EffectAttackType.Normal) {
			//	realMonsterDefense = (int)(defenseMonster.RealDefense() * defenseMonster.battleData_.RealDefenseParameterRank() * (4096 / 4096));
			//}
			realMonsterDefense = (int)(defenseMonster.RealDefense() * defenseMonster.battleData_.RealDefenseParameterRank() * (4096 / 4096));
		}

		//(((レベル×2/5+2)×威力×A/D)/50+2)
		//最終ダメージ
		int realDamage = (int)((((attackMonster.level_ * 2 / 5 + 2) * realSkillPower * realMonsterAttack / realMonsterDefense) / 50)
					   * randomResult * typeMatch * typeSimillar);

		return realDamage;
	}
}
