using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventMoveMapTrigger {
	None
	, Trigger
	, Touch
	, Max
}

public class EventMoveMapTriggerState {
	public EventMoveMapTriggerState(EventMoveMapTrigger setState) {
		state_ = setState;
	}

	public EventMoveMapTrigger state_;

	//None
	private static bool NoneEventTrigger(EventMoveMapTriggerState mine, PlayerEntryZone playerEntryZone, PlayerMoveMap playerMoveMap) {
		return false;
	}

	//Trigger
	private static bool TriggerEventTrigger(EventMoveMapTriggerState mine, PlayerEntryZone playerEntryZone, PlayerMoveMap playerMoveMap) {
		if(playerEntryZone.is_collider
			&& AllSceneManager.GetInstance().inputProvider_.SelectEnter()) {

			if(playerMoveMap.direction == ObjectMoveMap.DIRECTION_STATUS.UP) {
				playerEntryZone._collision_object.direction = ObjectMoveMap.DIRECTION_STATUS.DOWN;
			}
			else if (playerMoveMap.direction == ObjectMoveMap.DIRECTION_STATUS.DOWN) {
				playerEntryZone._collision_object.direction = ObjectMoveMap.DIRECTION_STATUS.UP;
			}
			else if (playerMoveMap.direction == ObjectMoveMap.DIRECTION_STATUS.RIGHT) {
				playerEntryZone._collision_object.direction = ObjectMoveMap.DIRECTION_STATUS.LEFT;
			}
			else if (playerMoveMap.direction == ObjectMoveMap.DIRECTION_STATUS.LEFT) {
				playerEntryZone._collision_object.direction = ObjectMoveMap.DIRECTION_STATUS.RIGHT;
			}

			return true;
		}

		return false;
	}

	//Touch
	private static bool TouchEventTrigger(EventMoveMapTriggerState mine, PlayerEntryZone playerEntryZone, PlayerMoveMap playerMoveMap) {
		if (playerEntryZone.is_collider) {
			return true;
		}

		return false;
	}

	private delegate bool EventTriggerFunc(EventMoveMapTriggerState mine, PlayerEntryZone playerEntryZone, PlayerMoveMap playerMoveMap);

	private EventTriggerFunc[] eventTriggers_ = new EventTriggerFunc[(int)EventMoveMapTrigger.Max] {
		NoneEventTrigger
		, TriggerEventTrigger
		, TouchEventTrigger
	};
	public bool EventTrigger(PlayerEntryZone playerEntryZone, PlayerMoveMap playerMoveMap) { return eventTriggers_[(int)state_](this, playerEntryZone, playerMoveMap); }
}
