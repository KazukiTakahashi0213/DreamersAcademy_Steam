using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorObjectsParts : MonoBehaviour {
	[SerializeField] private List<EventMoveMap> eventMoveMaps_ = new List<EventMoveMap>();

	public EventMoveMap GetEventMoveMaps(int value) { return eventMoveMaps_[value]; }
	public int GetEventMoveMapsCount() { return eventMoveMaps_.Count; }
}
