using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPartsProcessIdle : IMonsterPartsProcessState {
	public IMonsterPartsProcessState Update(MonsterParts monsterParts) {
		if (monsterParts.GetTimeCounter().measure(Time.deltaTime, monsterParts.GetIdleTimeRegulation())) {
			Vector3 vec3 = new Vector3(
				monsterParts.GetEventGameObject().GetGameObject().transform.position.x,
				monsterParts.GetEventGameObject().GetGameObject().transform.position.y + monsterParts.GetProcessIdleState().addPos_,
				monsterParts.GetEventGameObject().GetGameObject().transform.position.z
				);
			t13.UnityUtil.ObjectPosMove(monsterParts.GetEventGameObject().GetGameObject(), vec3);

			monsterParts.SetProcessIdleState(monsterParts.GetProcessIdleState().Next());
		}

		return this;
	}
}
