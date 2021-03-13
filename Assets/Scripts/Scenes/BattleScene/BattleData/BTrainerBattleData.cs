using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTrainerBattleData {
	//共通のdp
	private int dreamPoint_ = 0;
	public int GetDreamPoint() { return dreamPoint_; }
	public void DreamPointAddValue(int addValue) {
		//DPが100より下だったら
		if (dreamPoint_ < 100) {
			dreamPoint_ += addValue;

			//DPが100より上だったら
			if (dreamPoint_ > 100) dreamPoint_ = 100;
		}
	}
	public void DreamPointReset() { dreamPoint_ = 0; }

	//交換するか否かのフラグ
	public bool changeMonsterActive_ = false;
	//交換する手持ちの番号
	public int changeMonsterNumber_ = 0;

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
