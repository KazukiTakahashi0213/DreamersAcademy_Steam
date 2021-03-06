using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventTextProcess {
	None
	, CharaUpdate
	, ChangeColor
	, Max
}

public class EventTextProcessState {
	public EventTextProcessState(EventTextProcess setState) {
		state_ = setState;
	}

	public EventTextProcess state_;

	//None
	static private EventTextProcess NoneUpdate(EventTextProcessState mine, EventText eventText) {
		return mine.state_;
	}

	//CharaUpdate
	static private EventTextProcess CharaUpdateUpdate(EventTextProcessState mine, EventText eventText) {
		if (eventText.GetTimeCounter().measure(Time.deltaTime, eventText.GetTimeRegulation())) {
			//文章の処理
			eventText.GetText().text = t13.Utility.ContextUpdate(
				eventText.GetUpdateContext(),
				eventText.GetTimeRegulation(),
				eventText.GetTimeRegulation()
				);

			return EventTextProcess.None;
		}
		else {
			//文章の処理
			eventText.GetText().text = t13.Utility.ContextUpdate(
				eventText.GetUpdateContext(),
				eventText.GetTimeCounter().count(),
				eventText.GetTimeRegulation()
				);
		}

		return mine.state_;
	}

	//ChangeColor
	static private EventTextProcess ChangeColorUpdate(EventTextProcessState mine, EventText eventText) {
		if (eventText.GetTimeCounter().measure(Time.deltaTime, eventText.GetTimeRegulation())) {
			eventText.GetText().color = t13.UnityUtil.Color32InFluctUpdateRed(
				eventText.GetText().color
				, eventText.GetTimeFlucts(0)
				, eventText.GetChangeEndColor().r
				, eventText.GetTimeRegulation()
				, eventText.GetTimeRegulation()
				);

			eventText.GetText().color = t13.UnityUtil.Color32InFluctUpdateGreen(
				eventText.GetText().color
				, eventText.GetTimeFlucts(1)
				, eventText.GetChangeEndColor().g
				, eventText.GetTimeRegulation()
				, eventText.GetTimeRegulation()
				);

			eventText.GetText().color = t13.UnityUtil.Color32InFluctUpdateBlue(
				eventText.GetText().color
				, eventText.GetTimeFlucts(2)
				, eventText.GetChangeEndColor().b
				, eventText.GetTimeRegulation()
				, eventText.GetTimeRegulation()
				);

			eventText.GetText().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				eventText.GetText().color
				, eventText.GetTimeFlucts(3)
				, eventText.GetChangeEndColor().a
				, eventText.GetTimeRegulation()
				, eventText.GetTimeRegulation()
				);

			return EventTextProcess.None;
		}
		else {
			eventText.GetText().color = t13.UnityUtil.Color32InFluctUpdateRed(
				eventText.GetText().color
				, eventText.GetTimeFlucts(0)
				, eventText.GetChangeEndColor().r
				, eventText.GetTimeCounter().count()
				, eventText.GetTimeRegulation()
				);

			eventText.GetText().color = t13.UnityUtil.Color32InFluctUpdateGreen(
				eventText.GetText().color
				, eventText.GetTimeFlucts(1)
				, eventText.GetChangeEndColor().g
				, eventText.GetTimeCounter().count()
				, eventText.GetTimeRegulation()
				);

			eventText.GetText().color = t13.UnityUtil.Color32InFluctUpdateBlue(
				eventText.GetText().color
				, eventText.GetTimeFlucts(2)
				, eventText.GetChangeEndColor().b
				, eventText.GetTimeCounter().count()
				, eventText.GetTimeRegulation()
				);

			eventText.GetText().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				eventText.GetText().color
				, eventText.GetTimeFlucts(3)
				, eventText.GetChangeEndColor().a
				, eventText.GetTimeCounter().count()
				, eventText.GetTimeRegulation()
				);
		}

		return mine.state_;
	}

	private delegate EventTextProcess UpdateFunc(EventTextProcessState mine, EventText eventText);
	private UpdateFunc[] updateFuncs_ = new UpdateFunc[(int)EventTextProcess.Max] {
		NoneUpdate
		, CharaUpdateUpdate
		, ChangeColorUpdate
	};
	public EventTextProcess Update(EventText eventText) { return updateFuncs_[(int)state_](this, eventText); }
}
