using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerBattleData {
	//共通のdp
	public int dreamPoint_ = 0;

	virtual public void monsterAdd(IMonsterData addMonster) { }

	virtual public IMonsterData GetMonsterDatas(int num) { return null; }
	virtual public int GetMonsterDatasLength() { return -1; }
	virtual public int GetHaveMonsterSize() { return -1; }
	virtual public string GetUniqueTrainerName() { return ""; }

	//倒れた時の処理
	virtual public void MonsterDownEventSet(BattleManager manager) { }

	//交換処理
	virtual public void MonsterChangeEventSet(BattleManager manager) { }
}
