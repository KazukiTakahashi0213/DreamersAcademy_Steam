using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MagazineParts : MonoBehaviour {
	[SerializeField] private EventSpriteRenderer magazineEventSpriteRenderer_ = null;
	[SerializeField] private List<MonsterSDParts> monsterSDsParts_ = null;
	[SerializeField] private List<UpdateGameObject> battleMonsterFrame_ = new List<UpdateGameObject>();
	[SerializeField] private UpdateGameObject eventGameObject_ = null;

	public EventSpriteRenderer GetMagazineEventSpriteRenderer() { return magazineEventSpriteRenderer_; }
	public MonsterSDParts GetMonsterSDsParts(int number) { return monsterSDsParts_[number]; }
	public int GetMonsterSDsPartsCount() { return monsterSDsParts_.Count; }
	public UpdateGameObject GetEventGameObject() { return eventGameObject_; }
	public UpdateGameObject GetBattleMonsterFrame(int number) { return battleMonsterFrame_[number]; }
	public int GetBattleMonsterFrameCount() { return battleMonsterFrame_.Count; }

	private const float UPDATE_TIME_REGULATION = 0.4f;

	public void Initialize() {
		//Eulerの初期化
		eventGameObject_.addEulerVec3_ = new Vector3(0, 0, 0);
		for (int i = 0; i < monsterSDsParts_.Count; ++i) {
			monsterSDsParts_[i].GetEventGameObject().addEulerVec3_ = new Vector3(0, 0, 0);
		}
		for (int i = 0; i < battleMonsterFrame_.Count; ++i) {
			battleMonsterFrame_[i].addEulerVec3_ = new Vector3(0, 0, 0);
		}

		//Rotの初期化
		t13.UnityUtil.ObjectRotMove(gameObject, Quaternion.AngleAxis(0, new Vector3(0, 0, 1)));
		for (int i = 0; i < monsterSDsParts_.Count; ++i) {
			t13.UnityUtil.ObjectRotMove(monsterSDsParts_[i].gameObject, Quaternion.AngleAxis(0, new Vector3(0, 0, 1)));
		}
		for (int i = 0; i < battleMonsterFrame_.Count; ++i) {
			t13.UnityUtil.ObjectRotMove(battleMonsterFrame_[i].gameObject, Quaternion.AngleAxis(0, new Vector3(0, 0, 1)));
		}
	}

	public void UpRollMagazineParts() {
		AllEventManager.GetInstance().UpdateGameObjectSet(eventGameObject_, new Vector3(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z + (360.0f / monsterSDsParts_.Count)));

		for(int i = 0;i < monsterSDsParts_.Count; ++i) {
			AllEventManager.GetInstance().UpdateGameObjectSet(monsterSDsParts_[i].GetEventGameObject(), new Vector3(monsterSDsParts_[i].transform.localRotation.eulerAngles.x, monsterSDsParts_[i].transform.localRotation.eulerAngles.y, monsterSDsParts_[i].transform.localRotation.eulerAngles.z - (360.0f / monsterSDsParts_.Count)));
		}
		
		for(int i = 0;i < battleMonsterFrame_.Count; ++i) {
			AllEventManager.GetInstance().UpdateGameObjectSet(battleMonsterFrame_[i], new Vector3(battleMonsterFrame_[i].transform.localRotation.eulerAngles.x, battleMonsterFrame_[i].transform.localRotation.eulerAngles.y, battleMonsterFrame_[i].transform.localRotation.eulerAngles.z - (360.0f / monsterSDsParts_.Count)));
		}

		AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.RotMove);
		AllEventManager.GetInstance().AllUpdateEventExecute(UPDATE_TIME_REGULATION);
	}
	public void DownRollMagazineParts() {
		AllEventManager.GetInstance().UpdateGameObjectSet(eventGameObject_, new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z -(360.0f / monsterSDsParts_.Count)));

		for (int i = 0; i < monsterSDsParts_.Count; ++i) {
			AllEventManager.GetInstance().UpdateGameObjectSet(monsterSDsParts_[i].GetEventGameObject(), new Vector3(monsterSDsParts_[i].transform.localRotation.eulerAngles.x, monsterSDsParts_[i].transform.localRotation.eulerAngles.y, monsterSDsParts_[i].transform.localRotation.eulerAngles.z + (360.0f / monsterSDsParts_.Count)));
		}

		for (int i = 0; i < battleMonsterFrame_.Count; ++i) {
			AllEventManager.GetInstance().UpdateGameObjectSet(battleMonsterFrame_[i], new Vector3(battleMonsterFrame_[i].transform.localRotation.eulerAngles.x, battleMonsterFrame_[i].transform.localRotation.eulerAngles.y, battleMonsterFrame_[i].transform.localRotation.eulerAngles.z + (360.0f / monsterSDsParts_.Count)));
		}

		AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.RotMove);
		AllEventManager.GetInstance().AllUpdateEventExecute(UPDATE_TIME_REGULATION);
	}
}
