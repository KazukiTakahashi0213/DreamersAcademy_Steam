using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusInfoPartsEventManager {
	private StatusInfoPartsEventManagerExecuteState executeState_ = new StatusInfoPartsEventManagerExecuteState(StatusInfoPartsEventManagerExecute.None);

	private int eventStatusInfosPartsExecuteCounter_ = 0;
	private List<StatusInfoParts> eventStatusInfosParts_ = new List<StatusInfoParts>();
	private List<Color32> endColors_ = new List<Color32>();
	private List<List<StatusInfoParts>> executeEventStatusInfosParts_ = new List<List<StatusInfoParts>>();
	private List<List<Color32>> executeEndColors_ = new List<List<Color32>>();
	private List<StatusInfoPartsEventManagerExecute> statusInfoPartsEventManagerExecutes_ = new List<StatusInfoPartsEventManagerExecute>();

	public StatusInfoPartsEventManagerExecuteState GetExecuteState() { return executeState_; }

	public List<StatusInfoParts> GetexecuteEventStatusInfosParts() { return executeEventStatusInfosParts_[eventStatusInfosPartsExecuteCounter_]; }
	public List<Color32> GetExecuteEndColors() { return executeEndColors_[eventStatusInfosPartsExecuteCounter_]; }

	public void EventStatusInfoPartsSet(StatusInfoParts eventStatusInfoParts, Color32 endColor) {
		eventStatusInfosParts_.Add(eventStatusInfoParts);
		endColors_.Add(endColor);
	}
	public void EventStatusInfosPartsExecuteSet(StatusInfoPartsEventManagerExecute setExecute = StatusInfoPartsEventManagerExecute.None) {
		List<StatusInfoParts> addEventStatusInfoParts = new List<StatusInfoParts>();
		List<Color32> addEndColor = new List<Color32>();

		for (int i = 0; i < eventStatusInfosParts_.Count; ++i) {
			addEventStatusInfoParts.Add(eventStatusInfosParts_[i]);
			addEndColor.Add(endColors_[i]);
		}

		executeEventStatusInfosParts_.Add(addEventStatusInfoParts);
		executeEndColors_.Add(addEndColor);
		statusInfoPartsEventManagerExecutes_.Add(setExecute);

		eventStatusInfosParts_.Clear();
		endColors_.Clear();
	}

	public void EventStatusInfosPartsUpdateExecute(float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {
		executeState_.state_ = statusInfoPartsEventManagerExecutes_[eventStatusInfosPartsExecuteCounter_];

		executeState_.Execute(this, timeRegulation, timeFluctProcess);

		eventStatusInfosPartsExecuteCounter_ += 1;
	}

	public void EventStatusInfosPartsClear() {
		eventStatusInfosParts_.Clear();
		endColors_.Clear();
		executeEventStatusInfosParts_.Clear();
		executeEndColors_.Clear();
		statusInfoPartsEventManagerExecutes_.Clear();

		eventStatusInfosPartsExecuteCounter_ = 0;
	}
}
