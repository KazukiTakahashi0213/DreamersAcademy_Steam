using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterPartsProcessState {
	IMonsterPartsProcessState Update(MonsterParts monsterParts);
}
