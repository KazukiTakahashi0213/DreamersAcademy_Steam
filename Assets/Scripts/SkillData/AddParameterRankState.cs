using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum AddParameterRank {
	 None
	,Attack
	,Defense
	,SpecialAttack
	,SpecialDefense
	,Speed
	,AvoidRate
	,HitRate
	,Max
}

public class AddParameterRankState {
	public AddParameterRankState(AddParameterRank setState, int addParameterRank) {
		state_ = setState;
		addParameterRank_ = addParameterRank;
	}

	public AddParameterRank state_;
	private int addParameterRank_ = 0;

	//None
	static private string NoneAddParameterExecute(AddParameterRankState mine, IMonsterData monsterData) {
		string rankString = "";

		rankString = "ーー";

		return rankString;
	}

	//Attack
	static private string AttackAddParameterExecute(AddParameterRankState mine, IMonsterData monsterData) {
		monsterData.battleData_.AttackParameterRankAdd(mine.addParameterRank_);

		return monsterData.uniqueName_ + "の\n"
			+ "こうげきが　" + RankStringGenerate(mine.addParameterRank_);
	}

	//Defense
	static private string DefenseAddParameterExecute(AddParameterRankState mine, IMonsterData monsterData) {
		monsterData.battleData_.DefenseParameterRankAdd(mine.addParameterRank_);

		return monsterData.uniqueName_ + "の\n"
			+ "ぼうぎょが　" + RankStringGenerate(mine.addParameterRank_);
	}

	//SpecialAttack
	static private string SpecialAttackAddParameterExecute(AddParameterRankState mine, IMonsterData monsterData) {
		monsterData.battleData_.SpecialAttackParameterRankAdd(mine.addParameterRank_);

		return monsterData.uniqueName_ + "の\n"
			+ "とくこうが　" + RankStringGenerate(mine.addParameterRank_);
	}

	//SpecialDefense
	static private string SpecialDefenseAddParameterExecute(AddParameterRankState mine, IMonsterData monsterData) {
		monsterData.battleData_.SpecialDefenseParameterRankAdd(mine.addParameterRank_);

		return monsterData.uniqueName_ + "の\n"
			+ "とくぼうが　" + RankStringGenerate(mine.addParameterRank_);
	}

	//Speed
	static private string SpeedAddParameterExecute(AddParameterRankState mine, IMonsterData monsterData) {
		monsterData.battleData_.SpeedParameterRankAdd(mine.addParameterRank_);

		return monsterData.uniqueName_ + "の\n"
			+ "すばやさが　" + RankStringGenerate(mine.addParameterRank_);
	}

	//AvoidRate
	static private string AvoidRateAddParameterExecute(AddParameterRankState mine, IMonsterData monsterData) {
		monsterData.battleData_.AvoidRateParameterRankAdd(mine.addParameterRank_);

		return monsterData.uniqueName_ + "の\n"
			+ "かいひりつが　" + RankStringGenerate(mine.addParameterRank_);
	}

	//HitRate
	static private string HitRateAddParameterExecute(AddParameterRankState mine, IMonsterData monsterData) {
		monsterData.battleData_.HitRateParameterRankAdd(mine.addParameterRank_);

		return monsterData.uniqueName_ + "の\n"
			+ "めいちゅうりつが　" + RankStringGenerate(mine.addParameterRank_);
	}

	private delegate string AddParameterExecuteFunc(AddParameterRankState mine, IMonsterData monsterData);
	private AddParameterExecuteFunc[] addParameterExecutes_ = new AddParameterExecuteFunc[(int)AddParameterRank.Max] {
		NoneAddParameterExecute,
		AttackAddParameterExecute,
		DefenseAddParameterExecute,
		SpecialAttackAddParameterExecute,
		SpecialDefenseAddParameterExecute,
		SpeedAddParameterExecute,
		AvoidRateAddParameterExecute,
		HitRateAddParameterExecute
	};
	public string AddParameterExecute(IMonsterData monsterData) { return addParameterExecutes_[(int)state_](this, monsterData); }

	static private string RankStringGenerate(int addParameterRank) {
		string rankString = "";

		if (Math.Abs(addParameterRank) == 1) {
			if (addParameterRank > 0) rankString = "あがった！";
			else rankString = "さがった！";
		}
		else if (Math.Abs(addParameterRank) == 2) {
			if (addParameterRank > 0) rankString = "ぐーんとあがった！";
			else rankString = "がくっとさがった！";
		}
		else if (Math.Abs(addParameterRank) >= 3) {
			if (addParameterRank > 0) rankString = "ぐぐーんとあがった！";
			else rankString = "がくーんとさがった！";
		}

		return rankString;
	}
}
