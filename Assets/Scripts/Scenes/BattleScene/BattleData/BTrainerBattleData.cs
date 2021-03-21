using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTrainerBattleData {
	//共通のdp
	private int dreamPoint_ = 0;
	public int GetDreamPoint() { return dreamPoint_; }
	public void DreamPointAddValue(int addValue) {
		//負の数だったら
		if (addValue < 0) {
			//DPが0より上だったら
			if (dreamPoint_ > 0) {
				dreamPoint_ += addValue;

				//DPが0より下だったら
				if (dreamPoint_ < 0) dreamPoint_ = 0;
			}
		}
		else {
			//DPが100より下だったら
			if (dreamPoint_ < 100) {
				dreamPoint_ += addValue;

				//DPが100より上だったら
				if (dreamPoint_ > 100) dreamPoint_ = 100;
			}
		}
	}
	public void DreamPointReset() { dreamPoint_ = 0; }

	//交換するか否かのフラグ
	public bool changeMonsterActive_ = false;
	//交換する手持ちの番号
	public int changeMonsterNumber_ = 0;
	//交換するモンスターの技の番号
	public int changeMonsterSkillNumber_ = 0;

	//戦えるモンスターの数
	protected int battleActiveMonsterSize_ = 0;
	public int GetBattleActiveMonsterSize() { return battleActiveMonsterSize_; }

	virtual public void MonsterAdd(IMonsterData addMonster) { }

	virtual public IMonsterData GetMonsterDatas(int num) { return null; }
	virtual public int GetMonsterDatasLength() { return -1; }
	virtual public int GetHaveMonsterSize() { return -1; }
	virtual public string GetUniqueTrainerName() { return ""; }

	//倒れた時の処理
	virtual public void MonsterDownEventSet(BattleManager manager) { }

	//交換処理
	virtual public void MonsterChangeEventSet(BattleManager manager) { }
}
