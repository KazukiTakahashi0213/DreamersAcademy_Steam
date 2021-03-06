using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventSpriteRendererProcess {
	None
	, Anime
	, ChangeColor
	, Blink
	, Max
}

public class EventSpriteRendererProcessState {
	public EventSpriteRendererProcessState(EventSpriteRendererProcess setState) {
		state_ = setState;
	}

	public EventSpriteRendererProcess state_;

	//None
	static private EventSpriteRendererProcess NoneUpdate(EventSpriteRendererProcessState mine, EventSpriteRenderer eventSpriteRenderer) {
		return mine.state_;
	}

	//Anime
	static private EventSpriteRendererProcess AnimeUpdate(EventSpriteRendererProcessState mine, EventSpriteRenderer eventSpriteRenderer) {
		if (eventSpriteRenderer.GetTimeCounter().measure(Time.deltaTime, eventSpriteRenderer.GetTimeRegulation())) {
			eventSpriteRenderer.SetNowAnimeSpriteNumber(eventSpriteRenderer.GetNowAnimeSpriteNumber() + 1);

			if (eventSpriteRenderer.GetNowAnimeSpriteNumber() >= eventSpriteRenderer.GetAnimeSprites().Count) {
				eventSpriteRenderer.SetNowAnimeSpriteNumber(0);
				eventSpriteRenderer.GetSpriteRenderer().sprite = null;

				return EventSpriteRendererProcess.None;
			}

			eventSpriteRenderer.GetSpriteRenderer().sprite = eventSpriteRenderer.GetAnimeSprites()[eventSpriteRenderer.GetNowAnimeSpriteNumber()];

			return mine.state_;
		}

		return mine.state_;
	}

	//ChangeColor
	static private EventSpriteRendererProcess ChangeColorUpdate(EventSpriteRendererProcessState mine, EventSpriteRenderer eventSpriteRenderer) {
		if (eventSpriteRenderer.GetTimeCounter().measure(Time.deltaTime, eventSpriteRenderer.GetTimeRegulation())) {
			eventSpriteRenderer.GetSpriteRenderer().color = t13.UnityUtil.Color32InFluctUpdateRed(
				eventSpriteRenderer.GetSpriteRenderer().color
				, eventSpriteRenderer.GetTimeFlucts(0)
				, eventSpriteRenderer.GetChangeEndColor().r
				, eventSpriteRenderer.GetTimeRegulation()
				, eventSpriteRenderer.GetTimeRegulation()
				);

			eventSpriteRenderer.GetSpriteRenderer().color = t13.UnityUtil.Color32InFluctUpdateGreen(
				eventSpriteRenderer.GetSpriteRenderer().color
				, eventSpriteRenderer.GetTimeFlucts(1)
				, eventSpriteRenderer.GetChangeEndColor().g
				, eventSpriteRenderer.GetTimeRegulation()
				, eventSpriteRenderer.GetTimeRegulation()
				);

			eventSpriteRenderer.GetSpriteRenderer().color = t13.UnityUtil.Color32InFluctUpdateBlue(
				eventSpriteRenderer.GetSpriteRenderer().color
				, eventSpriteRenderer.GetTimeFlucts(2)
				, eventSpriteRenderer.GetChangeEndColor().b
				, eventSpriteRenderer.GetTimeRegulation()
				, eventSpriteRenderer.GetTimeRegulation()
				);

			eventSpriteRenderer.GetSpriteRenderer().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				eventSpriteRenderer.GetSpriteRenderer().color
				, eventSpriteRenderer.GetTimeFlucts(3)
				, eventSpriteRenderer.GetChangeEndColor().a
				, eventSpriteRenderer.GetTimeRegulation()
				, eventSpriteRenderer.GetTimeRegulation()
				);

			return EventSpriteRendererProcess.None;
		}
		else {
			eventSpriteRenderer.GetSpriteRenderer().color = t13.UnityUtil.Color32InFluctUpdateRed(
				eventSpriteRenderer.GetSpriteRenderer().color
				, eventSpriteRenderer.GetTimeFlucts(0)
				, eventSpriteRenderer.GetChangeEndColor().r
				, eventSpriteRenderer.GetTimeCounter().count()
				, eventSpriteRenderer.GetTimeRegulation()
				);

			eventSpriteRenderer.GetSpriteRenderer().color = t13.UnityUtil.Color32InFluctUpdateGreen(
				eventSpriteRenderer.GetSpriteRenderer().color
				, eventSpriteRenderer.GetTimeFlucts(1)
				, eventSpriteRenderer.GetChangeEndColor().g
				, eventSpriteRenderer.GetTimeCounter().count()
				, eventSpriteRenderer.GetTimeRegulation()
				);

			eventSpriteRenderer.GetSpriteRenderer().color = t13.UnityUtil.Color32InFluctUpdateBlue(
				eventSpriteRenderer.GetSpriteRenderer().color
				, eventSpriteRenderer.GetTimeFlucts(2)
				, eventSpriteRenderer.GetChangeEndColor().b
				, eventSpriteRenderer.GetTimeCounter().count()
				, eventSpriteRenderer.GetTimeRegulation()
				);

			eventSpriteRenderer.GetSpriteRenderer().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				eventSpriteRenderer.GetSpriteRenderer().color
				, eventSpriteRenderer.GetTimeFlucts(3)
				, eventSpriteRenderer.GetChangeEndColor().a
				, eventSpriteRenderer.GetTimeCounter().count()
				, eventSpriteRenderer.GetTimeRegulation()
				);
		}

		return mine.state_;
	}

	//Blink
	static private EventSpriteRendererProcess BlinkUpdate(EventSpriteRendererProcessState mine, EventSpriteRenderer eventSpriteRenderer) {
		if(eventSpriteRenderer.GetTimeCounter().measure(Time.deltaTime, eventSpriteRenderer.blinkTimeRegulation_)) {
			eventSpriteRenderer.GetSpriteRenderer().color = new Color(eventSpriteRenderer.GetSpriteRenderer().color.r, eventSpriteRenderer.GetSpriteRenderer().color.g, eventSpriteRenderer.GetSpriteRenderer().color.b, eventSpriteRenderer.GetBlinkState().AlphaValue());

			eventSpriteRenderer.GetBlinkState().state_ = eventSpriteRenderer.GetBlinkState().Next();
		}

		return mine.state_;
	}

	private delegate EventSpriteRendererProcess UpdateFunc(EventSpriteRendererProcessState mine, EventSpriteRenderer eventSpriteRenderer);

	private UpdateFunc[] updateFuncs_ = new UpdateFunc[(int)EventSpriteRendererProcess.Max] {
		NoneUpdate
		, AnimeUpdate
		, ChangeColorUpdate
		, BlinkUpdate
	};
	public EventSpriteRendererProcess Update(EventSpriteRenderer eventSpriteRenderer) { return updateFuncs_[(int)state_](this, eventSpriteRenderer); }
}
