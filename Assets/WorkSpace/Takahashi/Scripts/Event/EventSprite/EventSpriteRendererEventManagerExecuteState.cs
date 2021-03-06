using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventSpriteRendererEventManagerExecute {
	None
	, Anime
	, ChangeColor
	, SpriteSet
	, BlinkStart
	, BlinkEnd
	, Max
}

public class EventSpriteRendererEventManagerExecuteState {
	public EventSpriteRendererEventManagerExecuteState(EventSpriteRendererEventManagerExecute setState) {
		state_ = setState;
	}

	public EventSpriteRendererEventManagerExecute state_;

	//None
	static private void NoneExecute(EventSpriteRendererEventManagerExecuteState mine, EventSpriteRendererEventManager eventSpriteRendererEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {

	}

	//Anime
	static private void AnimeExecute(EventSpriteRendererEventManagerExecuteState mine, EventSpriteRendererEventManager eventSpriteRendererEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {
		for (int i = 0; i < eventSpriteRendererEventManager.GetExecuteEventSpriteRenderers().Count; ++i) {
			eventSpriteRendererEventManager.GetExecuteEventSpriteRenderers()[i].ProcessStateAnimeExecute(
				timeRegulation / eventSpriteRendererEventManager.GetExecuteAnimeSprites()[i].Count
				, eventSpriteRendererEventManager.GetExecuteAnimeSprites()[i]
				);
		}
	}

	//ChangeColor
	static private void ChangeColorExecute(EventSpriteRendererEventManagerExecuteState mine, EventSpriteRendererEventManager eventSpriteRendererEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {
		for (int i = 0; i < eventSpriteRendererEventManager.GetExecuteEventSpriteRenderers().Count; ++i) {
			eventSpriteRendererEventManager.GetExecuteEventSpriteRenderers()[i].ProcessStateChangeColorExecute(
				timeRegulation
				, timeFluctProcess
				, eventSpriteRendererEventManager.GetExecuteChangeColorEnds()[i]
				);
		}
	}

	//SpriteSet
	static private void SpriteSetExecute(EventSpriteRendererEventManagerExecuteState mine, EventSpriteRendererEventManager eventSpriteRendererEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {
		for (int i = 0; i < eventSpriteRendererEventManager.GetExecuteEventSpriteRenderers().Count; ++i) {
			eventSpriteRendererEventManager.GetExecuteEventSpriteRenderers()[i].SpriteSet(
				eventSpriteRendererEventManager.GetExecuteAnimeSprites()[i]
				);
		}
	}

	//BlinkStart
	static private void BlinkStartExecute(EventSpriteRendererEventManagerExecuteState mine, EventSpriteRendererEventManager eventSpriteRendererEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {
		for (int i = 0; i < eventSpriteRendererEventManager.GetExecuteEventSpriteRenderers().Count; ++i) {
			eventSpriteRendererEventManager.GetExecuteEventSpriteRenderers()[i].ProcessStateBlinkStartExecute(
				);
		}
	}
	//BlinkEnd
	static private void BlinkEndExecute(EventSpriteRendererEventManagerExecuteState mine, EventSpriteRendererEventManager eventSpriteRendererEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {
		for (int i = 0; i < eventSpriteRendererEventManager.GetExecuteEventSpriteRenderers().Count; ++i) {
			eventSpriteRendererEventManager.GetExecuteEventSpriteRenderers()[i].ProcessStateBlinkEndExecute(
				);
		}
	}

	private delegate void ExecuteFunc(EventSpriteRendererEventManagerExecuteState mine, EventSpriteRendererEventManager eventSpriteRendererEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess);

	private ExecuteFunc[] executeFuncs_ = new ExecuteFunc[(int)EventSpriteRendererEventManagerExecute.Max] {
		NoneExecute
		, AnimeExecute
		, ChangeColorExecute
		, SpriteSetExecute
		, BlinkStartExecute
		, BlinkEndExecute
	};
	public void Execute(EventSpriteRendererEventManager eventSpriteRendererEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) { executeFuncs_[(int)state_](this, eventSpriteRendererEventManager, timeRegulation, timeFluctProcess); }
}
