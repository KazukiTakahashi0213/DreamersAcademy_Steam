using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSpriteRendererEventManager {
	private EventSpriteRendererEventManagerExecuteState executeState_ = new EventSpriteRendererEventManagerExecuteState(EventSpriteRendererEventManagerExecute.None);

	private int eventSpriteRenderersExecuteCounter_ = 0;
	private List<EventSpriteRenderer> eventSpriteRenderers_ = new List<EventSpriteRenderer>();
	private List<List<Sprite>> animeSprites_ = new List<List<Sprite>>();
	private List<Color32> changeColorEnds_ = new List<Color32>();
	private List<List<EventSpriteRenderer>> executeEvenetSpriteRenderers_ = new List<List<EventSpriteRenderer>>();
	private List<List<List<Sprite>>> executeAnimeSprites_ = new List<List<List<Sprite>>>();
	private List<List<Color32>> executeChangeColorEnds_ = new List<List<Color32>>();
	private List<EventSpriteRendererEventManagerExecute> eventSpriteRendererEventManagerExecutes_ = new List<EventSpriteRendererEventManagerExecute>();

	public EventSpriteRendererEventManagerExecuteState GetExecuteState() { return executeState_; }

	public List<EventSpriteRenderer> GetExecuteEventSpriteRenderers() { return executeEvenetSpriteRenderers_[eventSpriteRenderersExecuteCounter_]; }
	public List<List<Sprite>> GetExecuteAnimeSprites() { return executeAnimeSprites_[eventSpriteRenderersExecuteCounter_]; }
	public List<Color32> GetExecuteChangeColorEnds() { return executeChangeColorEnds_[eventSpriteRenderersExecuteCounter_]; }

	public void EventSpriteRendererSet(EventSpriteRenderer eventSpriteRenderers, List<Sprite> sprites, Color32 color) {
		eventSpriteRenderers_.Add(eventSpriteRenderers);
		animeSprites_.Add(sprites);
		changeColorEnds_.Add(color);
	}
	public void EventSpriteRenderersExecuteSet(EventSpriteRendererEventManagerExecute setExecute = EventSpriteRendererEventManagerExecute.None) {
		List<EventSpriteRenderer> addEventSpriteRenderers = new List<EventSpriteRenderer>();
		List<List<Sprite>> addSprites = new List<List<Sprite>>();
		List<Color32> addColor32s = new List<Color32>();

		for (int i = 0; i < eventSpriteRenderers_.Count; ++i) {
			addEventSpriteRenderers.Add(eventSpriteRenderers_[i]);
			addSprites.Add(animeSprites_[i]);
			addColor32s.Add(changeColorEnds_[i]);
		}

		executeEvenetSpriteRenderers_.Add(addEventSpriteRenderers);
		executeAnimeSprites_.Add(addSprites);
		executeChangeColorEnds_.Add(addColor32s);
		eventSpriteRendererEventManagerExecutes_.Add(setExecute);

		eventSpriteRenderers_.Clear();
		animeSprites_.Clear();
		changeColorEnds_.Clear();
	}

	public void EventSpriteRenderersUpdateExecute(float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {
		executeState_.state_ = eventSpriteRendererEventManagerExecutes_[eventSpriteRenderersExecuteCounter_];

		executeState_.Execute(this, timeRegulation, timeFluctProcess);

		eventSpriteRenderersExecuteCounter_ += 1;
	}

	public void EventSpriteRenderersClear() {
		eventSpriteRenderers_.Clear();
		animeSprites_.Clear();
		changeColorEnds_.Clear();
		executeEvenetSpriteRenderers_.Clear();
		executeAnimeSprites_.Clear();
		executeChangeColorEnds_.Clear();
		eventSpriteRendererEventManagerExecutes_.Clear();

		eventSpriteRenderersExecuteCounter_ = 0;
	}
}
