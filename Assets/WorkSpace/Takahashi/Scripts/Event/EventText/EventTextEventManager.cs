using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTextEventManager {
	private EventTextEventManagerExecuteState executeState_ = new EventTextEventManagerExecuteState(EventTextEventManagerExecute.None);

	private int eventTextsExecuteCounter_ = 0;
	private List<EventText> eventTexts_ = new List<EventText>();
	private List<string> updateContexts_ = new List<string>();
	private List<Color32> changeColorEnds_ = new List<Color32>();
	private List<List<EventText>> executeEventTexts_ = new List<List<EventText>>();
	private List<List<string>> executeUpdateContexts_ = new List<List<string>>();
	private List<List<Color32>> executeChangeColorEnds_ = new List<List<Color32>>();
	private List<EventTextEventManagerExecute> eventTextEventManagerExecutes_ = new List<EventTextEventManagerExecute>();

	public EventTextEventManagerExecuteState GetExecuteState() { return executeState_; }

	public List<EventText> GetExecuteEventTexts() { return executeEventTexts_[eventTextsExecuteCounter_]; }
	public List<string> GetExecuteUpdateContexts() { return executeUpdateContexts_[eventTextsExecuteCounter_]; }
	public List<Color32> GetExecuteChangeColorEnds() { return executeChangeColorEnds_[eventTextsExecuteCounter_]; }

	public void EventTextSet(EventText eventText, string updateContext, Color32 color) {
		eventTexts_.Add(eventText);
		changeColorEnds_.Add(color);
		updateContexts_.Add(updateContext);
	}
	public void EventTextsExecuteSet(EventTextEventManagerExecute setState = EventTextEventManagerExecute.None) {
		List<EventText> addEventTexts = new List<EventText>();
		List<string> addUpdateContexts = new List<string>();
		List<Color32> addColor32s = new List<Color32>();

		for (int i = 0; i < eventTexts_.Count; ++i) {
			addEventTexts.Add(eventTexts_[i]);
			addUpdateContexts.Add(updateContexts_[i]);
			addColor32s.Add(changeColorEnds_[i]);
		}

		executeEventTexts_.Add(addEventTexts);
		executeUpdateContexts_.Add(addUpdateContexts);
		executeChangeColorEnds_.Add(addColor32s);
		eventTextEventManagerExecutes_.Add(setState);

		eventTexts_.Clear();
		updateContexts_.Clear();
		changeColorEnds_.Clear();
	}

	public void EventTextsUpdateExecute(float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {
		executeState_.state_ = eventTextEventManagerExecutes_[eventTextsExecuteCounter_];

		executeState_.Execute(this, timeRegulation, timeFluctProcess);

		eventTextsExecuteCounter_ += 1;
	}

	public void EventTextsClear() {
		eventTexts_.Clear();
		updateContexts_.Clear();
		changeColorEnds_.Clear();
		executeEventTexts_.Clear();
		executeUpdateContexts_.Clear();
		executeChangeColorEnds_.Clear();
		eventTextEventManagerExecutes_.Clear();

		eventTextsExecuteCounter_ = 0;
	}
}
