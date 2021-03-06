using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterBattleData {
	AbnormalTypeState firstAbnormalState_ { get; set; }
	AbnormalTypeState secondAbnormalState_ { get; set; }

	void RankReset();

	void AttackParameterRankAdd(int value);
	void DefenseParameterRankAdd(int value);
	void SpecialAttackParameterRankAdd(int value);
	void SpecialDefenseParameterRankAdd(int value);
	void SpeedParameterRankAdd(int value);

	void AvoidRateParameterRankAdd(int value);
	void HitRateParameterRankAdd(int value);

	float RealAttackParameterRank();
	float RealDefenseParameterRank();
	float RealSpecialAttackParameterRank();
	float RealSpecialDefenseParameterRank();
	float RealSpeedParameterRank();

	int GetAvoidRateParameterRank();
	int GetHitRateParameterRank();

	void AbnormalSetStatusInfoParts(StatusInfoParts statusInfoParts);
	void AbnormalSetStatusInfoPartsEventSet(StatusInfoParts statusInfoParts);
	void RefreshAbnormalType(AbnormalType refreshAbnormalType);
	bool HaveAbnormalType(AbnormalType haveAbnormalType);

	bool BurnsCounter();

	void SleepTurnSeedCreate();
	bool UseSleepTurn();

	void ConfusionTurnSeedCreate();
	bool UseConfusionTurn();
}
