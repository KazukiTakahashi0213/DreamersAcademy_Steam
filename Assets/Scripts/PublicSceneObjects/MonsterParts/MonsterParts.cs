using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterParts : MonoBehaviour {
	//EntryPoint
	void Update() {
		processState_ = processState_.Update(this);
	}

	[SerializeField] private SpriteRenderer monsterSprite_ = null;
	[SerializeField] private EventSpriteRenderer eventMonsterSprite_ = null;
	[SerializeField] private UpdateGameObject eventGameObject_ = null;
	[SerializeField] float idleTimeRegulation_ = 0.5f;
	[SerializeField] private float entryPosY_ = 0;

	private IMonsterPartsProcessState processState_ = new MonsterPartsProcessNone();
	private IMonsterPartsProcessIdleState processIdleState_ = new MonsterPartsProcessIdleDown();

	private t13.TimeFluct timeFluct_ = new t13.TimeFluct();
	private t13.TimeCounter timeCounter_ = new t13.TimeCounter();

	public SpriteRenderer GetMonsterSprite() { return monsterSprite_; }
	public EventSpriteRenderer GetEventMonsterSprite() { return eventMonsterSprite_; }
	public UpdateGameObject GetEventGameObject() { return eventGameObject_; }
	public float GetIdleTimeRegulation() { return idleTimeRegulation_; }

	public void SetProcessIdleState(IMonsterPartsProcessIdleState state) { processIdleState_ = state; }
	public IMonsterPartsProcessIdleState GetProcessIdleState() { return processIdleState_; }

	public t13.TimeFluct GetTimeFluct() { return timeFluct_; }
	public t13.TimeCounter GetTimeCounter() { return timeCounter_; }

	public void ProcessIdleStart() {
		processState_ = new MonsterPartsProcessIdle();
	}
	public void ProcessIdleEnd() {
		t13.UnityUtil.ObjectPosMove(gameObject, new Vector3(transform.position.x, entryPosY_, transform.position.z));

		processIdleState_ = new MonsterPartsProcessIdleDown();

		timeCounter_.reset();

		processState_ = new MonsterPartsProcessNone();
	}
}
