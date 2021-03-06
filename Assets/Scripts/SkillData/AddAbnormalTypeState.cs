using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AddAbnormalType {
	None
	,Burns
	,Poison
	,Sleep
	,Confusion
	,Hero
	,Max
}

public class AddAbnormalTypeState {
	public AddAbnormalTypeState(AddAbnormalType setState) {
		state_ = setState;
	}

	public AddAbnormalType state_;

	//None
	static private string NoneAddAbnormalTypeExecute(AddAbnormalTypeState mine, IMonsterData monsterData) {
		return "None";
	}
	//Burns
	static private string BurnsAddAbnormalTypeExecute(AddAbnormalTypeState mine, IMonsterData monsterData) {
		if(monsterData.battleData_.firstAbnormalState_.state_ == AbnormalType.Hero) {
			return monsterData.uniqueName_ + "には\n" +
				"きかないようだ・・・";
		}

		if (monsterData.battleData_.firstAbnormalState_.state_ == AbnormalType.None) {
			monsterData.battleData_.firstAbnormalState_.state_ = AbnormalType.Burns;
		}
		else if (monsterData.battleData_.secondAbnormalState_.state_ == AbnormalType.None
			&& monsterData.battleData_.firstAbnormalState_.state_ != AbnormalType.Burns) {
			monsterData.battleData_.secondAbnormalState_.state_ = AbnormalType.Burns;
		}
		else {
			return monsterData.uniqueName_ + "には\n" + 
				"きかないようだ・・・";
		}

		return monsterData.uniqueName_ + "は\n" +
			"やけどを　おった！";
	}
	//Poison
	static private string PoisonAddAbnormalTypeExecute(AddAbnormalTypeState mine, IMonsterData monsterData) {
		if (monsterData.battleData_.firstAbnormalState_.state_ == AbnormalType.Hero) {
			return monsterData.uniqueName_ + "には\n" +
				"きかないようだ・・・";
		}

		if (monsterData.battleData_.firstAbnormalState_.state_ == AbnormalType.None) {
			monsterData.battleData_.firstAbnormalState_.state_ = AbnormalType.Poison;
		}
		else if (monsterData.battleData_.secondAbnormalState_.state_ == AbnormalType.None
			&& monsterData.battleData_.firstAbnormalState_.state_ != AbnormalType.Poison) {
			monsterData.battleData_.secondAbnormalState_.state_ = AbnormalType.Poison;
		}
		else {
			return monsterData.uniqueName_ + "には\n" +
				"きかないようだ・・・";
		}

		return monsterData.uniqueName_ + "は\n" +
			"どくに　おかされた！";
	}
	//Sleep
	static private string SleepAddAbnormalTypeExecute(AddAbnormalTypeState mine, IMonsterData monsterData) {
		if (monsterData.battleData_.firstAbnormalState_.state_ == AbnormalType.Hero) {
			return monsterData.uniqueName_ + "には\n" +
				"きかないようだ・・・";
		}

		if (monsterData.battleData_.firstAbnormalState_.state_ == AbnormalType.None) {
			monsterData.battleData_.firstAbnormalState_.state_ = AbnormalType.Sleep;
		}
		else if (monsterData.battleData_.secondAbnormalState_.state_ == AbnormalType.None 
			&& monsterData.battleData_.firstAbnormalState_.state_ != AbnormalType.Sleep) {
			monsterData.battleData_.secondAbnormalState_.state_ = AbnormalType.Sleep;
		}
		else {
			return monsterData.uniqueName_ + "には\n" +
				"きかないようだ・・・";
		}

		return monsterData.uniqueName_ + "は\n" +
			"ねむりに　おちた！";
	}
	//Confusion
	static private string ConfusionAddAbnormalTypeExecute(AddAbnormalTypeState mine, IMonsterData monsterData) {
		if (monsterData.battleData_.firstAbnormalState_.state_ == AbnormalType.Hero) {
			return monsterData.uniqueName_ + "には\n" +
				"きかないようだ・・・";
		}

		if (monsterData.battleData_.firstAbnormalState_.state_ == AbnormalType.None) {
			monsterData.battleData_.firstAbnormalState_.state_ = AbnormalType.Confusion;
		}
		else if (monsterData.battleData_.secondAbnormalState_.state_ == AbnormalType.None
			&& monsterData.battleData_.firstAbnormalState_.state_ != AbnormalType.Confusion) {
			monsterData.battleData_.secondAbnormalState_.state_ = AbnormalType.Confusion;
		}
		else {
			return monsterData.uniqueName_ + "には\n" +
				"きかないようだ・・・";
		}

		return monsterData.uniqueName_ + "は\n" +
			"こんらんした！";
	}
	//Hero
	static private string HeroAddAbnormalTypeExecute(AddAbnormalTypeState mine, IMonsterData monsterData) {
		monsterData.battleData_.firstAbnormalState_.state_ = AbnormalType.Hero;
		monsterData.battleData_.secondAbnormalState_.state_ = AbnormalType.None;

		return monsterData.uniqueName_ + "は\n" +
			"ゆめで　みたされた！";
	}

	private delegate string AddAbnormalTypeExecuteFunc(AddAbnormalTypeState mine, IMonsterData monsterData);
	private AddAbnormalTypeExecuteFunc[] addAbnormalTypeExecutes_ = new AddAbnormalTypeExecuteFunc[(int)AddAbnormalType.Max] {
		NoneAddAbnormalTypeExecute,
		BurnsAddAbnormalTypeExecute,
		PoisonAddAbnormalTypeExecute,
		SleepAddAbnormalTypeExecute,
		ConfusionAddAbnormalTypeExecute,
		HeroAddAbnormalTypeExecute
	};
	public string AddAbnormalTypeExecute(IMonsterData monsterData) { return addAbnormalTypeExecutes_[(int)state_](this, monsterData); }
}
