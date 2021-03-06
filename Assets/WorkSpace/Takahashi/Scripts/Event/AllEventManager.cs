using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllEventManager {
	private t13.TimeCounter sceneCounter_ = new t13.TimeCounter();
	private List<t13.TimeFluct> sceneFlucts_ = new List<t13.TimeFluct>();
	private t13.Event<AllEventManager> sceneEvent_;

	public t13.TimeCounter GetSceneCounter() { return sceneCounter_; }
	public List<t13.TimeFluct> GetSceneFlucts() { return sceneFlucts_; }
	public bool EventUpdate() { return sceneEvent_.update(); }

	//eventの管理メンバ
	private EventSpriteRendererEventManager eventSpriteEventManager_ = new EventSpriteRendererEventManager();
	private UpdateGameObjectEventManager updateGameObjectEventManager_ = new UpdateGameObjectEventManager();
	private EventTextEventManager eventTextEventManager_ = new EventTextEventManager();
	private HpGaugePartsEventManager hpGaugePartsEventManager_ = new HpGaugePartsEventManager();
	private StatusInfoPartsEventManager statusInfoPartsEventManager_ = new StatusInfoPartsEventManager();
	private UpdateImageEventManager updateImageEventManager_ = new UpdateImageEventManager();

	private int updateEventExecuteCounter_ = 0;
	private List<float> eventTimeRegulation_ = new List<float>();
	private List<t13.TimeFluctProcess> eventTimeFluctProcesses_ = new List<t13.TimeFluctProcess>();

	private UpdateGameObjectEventManagerExecute updateGameObjectEventManagerExecute_ = UpdateGameObjectEventManagerExecute.None;
	private EventSpriteRendererEventManagerExecute eventSpriteRendererEventManagerExecute_ = EventSpriteRendererEventManagerExecute.None;
	private HpGaugePartsEventManagerExecute hpGaugePartsEventManagerExecute_ = HpGaugePartsEventManagerExecute.None;
	private EventTextEventManagerExecute eventTextEventManagerExecute_ = EventTextEventManagerExecute.None;
	private StatusInfoPartsEventManagerExecute statusInfoPartsEventManagerExecute_ = StatusInfoPartsEventManagerExecute.None;
	private UpdateImageEventManagerExecute updateImageEventManagerExecute_ = UpdateImageEventManagerExecute.None;

	private int eventActiveExecuteCounter_ = 0;
	private List<bool> eventActive_ = new List<bool>();

	private List<SceneState> sceneStates_ = new List<SceneState>();
	private List<SceneChangeMode> sceneChangeModes_ = new List<SceneChangeMode>();

	private int eventBGMAudioExecuteCounter_ = 0;
	private List<float> eventBGMAudioVolumes_ = new List<float>();
	private List<AudioClip> eventBGMAudioClips_ = new List<AudioClip>();

	private int eventSEAudioExecuteCounter_ = 0;
	private List<float> eventSEAudioVolumes_ = new List<float>();
	private List<AudioClip> eventSEAudioClips_ = new List<AudioClip>();

	private bool eventTriggerNextTrigger_ = false;
	private bool eventTriggerNextActive_ = false;
	public void EventTriggerNext() {
		if (eventTriggerNextActive_) {
			eventTriggerNextTrigger_ = true;
		}
	}

	private List<IInputProvider> inputProviders_ = new List<IInputProvider>();

	//EventManager
	public void EventWaitSet(float timeRegulation) {
		eventTimeRegulation_.Add(timeRegulation);
		eventTimeFluctProcesses_.Add(t13.TimeFluctProcess.None);

		sceneEvent_.func_add(WaitEvent);
	}
	public void EventTriggerSet() {
		sceneEvent_.func_add(TriggerEvent);
	}
	public void EventFinishSet() {
		sceneEvent_.func_add(EventFinishEvent);
	}
	public void AllUpdateEventExecute(float timeRegulation = 0, t13.TimeFluctProcess timeFluctProcess = t13.TimeFluctProcess.Liner) {
		eventTimeRegulation_.Add(timeRegulation);
		eventTimeFluctProcesses_.Add(timeFluctProcess);

		updateGameObjectEventManager_.UpdateGameObjectsExecuteSet(updateGameObjectEventManagerExecute_);
		eventSpriteEventManager_.EventSpriteRenderersExecuteSet(eventSpriteRendererEventManagerExecute_);
		hpGaugePartsEventManager_.HpGaugesPartsExecuteSet(hpGaugePartsEventManagerExecute_);
		eventTextEventManager_.EventTextsExecuteSet(eventTextEventManagerExecute_);
		statusInfoPartsEventManager_.EventStatusInfosPartsExecuteSet(statusInfoPartsEventManagerExecute_);
		updateImageEventManager_.UpdateImageExecuteSet(updateImageEventManagerExecute_);

		sceneEvent_.func_add(AllUpdateEventExecuteEvent);

		updateGameObjectEventManagerExecute_ = UpdateGameObjectEventManagerExecute.None;
		eventSpriteRendererEventManagerExecute_ = EventSpriteRendererEventManagerExecute.None;
		hpGaugePartsEventManagerExecute_ = HpGaugePartsEventManagerExecute.None;
		eventTextEventManagerExecute_ = EventTextEventManagerExecute.None;
		statusInfoPartsEventManagerExecute_ = StatusInfoPartsEventManagerExecute.None;
		updateImageEventManagerExecute_ = UpdateImageEventManagerExecute.None;
	}
	public void SceneChangeEventSet(SceneState sceneState, SceneChangeMode sceneChangeMode) {
		sceneStates_.Add(sceneState);
		sceneChangeModes_.Add(sceneChangeMode);

		sceneEvent_.func_add(SceneChangeEvent);
	}
	public void InputProviderChangeEventSet(IInputProvider inputProvider) {
		inputProviders_.Add(inputProvider);

		sceneEvent_.func_add(InputProviderChangeEvent);
	}
	public void BGMAudioVolumeChangeEventSet(float volume) {
		eventBGMAudioVolumes_.Add(volume);
		eventBGMAudioClips_.Add(null);

		sceneEvent_.func_add(BGMAudioVolumeChangeEvent);
	}
	public void BGMAudioClipChangeEventSet(AudioClip clip) {
		eventBGMAudioVolumes_.Add(0);
		eventBGMAudioClips_.Add(clip);

		sceneEvent_.func_add(BGMAudioClipChangeEvent);
	}
	public void BGMAudioPlayEventSet() {
		sceneEvent_.func_add(BGMAudioPlayEvent);
	}
	public void SEAudioVolumeChangeEventSet(float volume) {
		eventSEAudioVolumes_.Add(volume);
		eventSEAudioClips_.Add(null);

		sceneEvent_.func_add(SEAudioVolumeChangeEvent);
	}
	public void SEAudioPlayOneShotEventSet(AudioClip clip) {
		eventSEAudioVolumes_.Add(0);
		eventSEAudioClips_.Add(clip);

		sceneEvent_.func_add(SEAudioPlayOnshotEvent);
	}
	static private bool WaitEvent(AllEventManager mgr) {
		if (mgr.sceneCounter_.measure(Time.deltaTime, mgr.eventTimeRegulation_[mgr.updateEventExecuteCounter_])) {
			mgr.updateEventExecuteCounter_ += 1;

			return true;
		}

		return false;
	}
	static private bool TriggerEvent(AllEventManager mgr) {
		mgr.eventTriggerNextActive_ = true;

		if (mgr.eventTriggerNextTrigger_) {
			mgr.eventTriggerNextTrigger_ = false;
			mgr.eventTriggerNextActive_ = false;

			return true;
		}

		return false;
	}
	static private bool EventFinishEvent(AllEventManager mgr) {
		mgr.updateEventExecuteCounter_ = 0;
		mgr.eventTimeRegulation_.Clear();
		mgr.eventTimeFluctProcesses_.Clear();

		mgr.eventActiveExecuteCounter_ = 0;
		mgr.eventActive_.Clear();

		mgr.sceneStates_.Clear();
		mgr.sceneChangeModes_.Clear();

		mgr.inputProviders_.Clear();

		mgr.eventBGMAudioExecuteCounter_ = 0;
		mgr.eventBGMAudioVolumes_.Clear();
		mgr.eventBGMAudioClips_.Clear();

		mgr.eventSEAudioExecuteCounter_ = 0;
		mgr.eventSEAudioVolumes_.Clear();
		mgr.eventSEAudioClips_.Clear();

		mgr.eventSpriteEventManager_.EventSpriteRenderersClear();
		mgr.updateGameObjectEventManager_.UpdateGameObjectsClear();
		mgr.eventTextEventManager_.EventTextsClear();
		mgr.hpGaugePartsEventManager_.HpGaugesPartsClear();
		mgr.statusInfoPartsEventManager_.EventStatusInfosPartsClear();
		mgr.updateImageEventManager_.UpdateImagesClear();

		return mgr.sceneEvent_.event_finish();
	}
	static private bool AllUpdateEventExecuteEvent(AllEventManager mgr) {
		mgr.updateGameObjectEventManager_.UpdateGameObjectsUpdateExecute(mgr.eventTimeRegulation_[mgr.updateEventExecuteCounter_], mgr.eventTimeFluctProcesses_[mgr.updateEventExecuteCounter_]);
		mgr.eventSpriteEventManager_.EventSpriteRenderersUpdateExecute(mgr.eventTimeRegulation_[mgr.updateEventExecuteCounter_], mgr.eventTimeFluctProcesses_[mgr.updateEventExecuteCounter_]);
		mgr.hpGaugePartsEventManager_.HpGaugesPartsUpdateExecute(mgr.eventTimeRegulation_[mgr.updateEventExecuteCounter_], mgr.eventTimeFluctProcesses_[mgr.updateEventExecuteCounter_]);
		mgr.eventTextEventManager_.EventTextsUpdateExecute(mgr.eventTimeRegulation_[mgr.updateEventExecuteCounter_], mgr.eventTimeFluctProcesses_[mgr.updateEventExecuteCounter_]);
		mgr.statusInfoPartsEventManager_.EventStatusInfosPartsUpdateExecute(mgr.eventTimeRegulation_[mgr.updateEventExecuteCounter_], mgr.eventTimeFluctProcesses_[mgr.updateEventExecuteCounter_]);
		mgr.updateImageEventManager_.UpdateImagesUpdateExecute(mgr.eventTimeRegulation_[mgr.updateEventExecuteCounter_], mgr.eventTimeFluctProcesses_[mgr.updateEventExecuteCounter_]);

		mgr.sceneEvent_.func_insert(WaitEvent, mgr.sceneEvent_.funcs_num() + 1);

		return true;
	}
	static private bool SceneChangeEvent(AllEventManager mgr) {
		AllSceneManager.GetInstance().SceneChange(mgr.sceneStates_[0], mgr.sceneChangeModes_[0]);

		EventFinishEvent(mgr);

		return true;
	}
	static private bool InputProviderChangeEvent(AllEventManager mgr) {
		AllSceneManager.GetInstance().inputProvider_ = mgr.inputProviders_[0];

		EventFinishEvent(mgr);

		return true;
	}
	static private bool BGMAudioVolumeChangeEvent(AllEventManager mgr) {
		AllSceneManager.GetInstance().GetPublicAudioParts().GetBGMAudioSource().volume = mgr.eventBGMAudioVolumes_[mgr.eventBGMAudioExecuteCounter_];

		mgr.eventBGMAudioExecuteCounter_ += 1;

		return true;
	}
	static private bool BGMAudioClipChangeEvent(AllEventManager mgr) {
		AllSceneManager.GetInstance().GetPublicAudioParts().GetBGMAudioSource().clip = mgr.eventBGMAudioClips_[mgr.eventBGMAudioExecuteCounter_];

		mgr.eventBGMAudioExecuteCounter_ += 1;

		return true;
	}
	static private bool BGMAudioPlayEvent(AllEventManager mgr) {
		AllSceneManager.GetInstance().GetPublicAudioParts().GetBGMAudioSource().Play();

		return true;
	}
	static private bool SEAudioVolumeChangeEvent(AllEventManager mgr) {
		AllSceneManager.GetInstance().GetPublicAudioParts().GetSEAudioSource().volume = mgr.eventSEAudioVolumes_[mgr.eventSEAudioExecuteCounter_];

		mgr.eventSEAudioExecuteCounter_ += 1;

		return true;
	}
	static private bool SEAudioPlayOnshotEvent(AllEventManager mgr) {
		AllSceneManager.GetInstance().GetPublicAudioParts().GetSEAudioSource().PlayOneShot(mgr.eventSEAudioClips_[mgr.eventSEAudioExecuteCounter_]);

		mgr.eventSEAudioExecuteCounter_ += 1;

		return true;
	}

	//EventSpriteRenderer
	public void EventSpriteRendererSet(EventSpriteRenderer eventSprite, List<Sprite> sprites = null, Color32 color = new Color32()) {
		eventSpriteEventManager_.EventSpriteRendererSet(eventSprite, sprites, color);
	}
	public void EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute setExecute) {
		eventSpriteRendererEventManagerExecute_ = setExecute;
	}

	//UpdateGameObject
	public void UpdateGameObjectSet(UpdateGameObject updateGameObject, Vector3 endVec3 = new Vector3()) {
		updateGameObjectEventManager_.UpdateGameObjectSet(updateGameObject, endVec3);
	}
	public void UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute setExecute) {
		updateGameObjectEventManagerExecute_ = setExecute;
	}
	public void UpdateGameObjectsActiveSetExecute(bool setActive) {
		updateGameObjectEventManager_.UpdateGameObjectsExecuteSet();

		sceneEvent_.func_add(UpdateGameObjectsActiveSetExecuteEvent);

		eventActive_.Add(setActive);
	}
	static private bool UpdateGameObjectsActiveSetExecuteEvent(AllEventManager mgr) {
		mgr.updateGameObjectEventManager_.UpdateGameObjectsActiveSetExecute(mgr.eventActive_[mgr.eventActiveExecuteCounter_]);

		mgr.eventActiveExecuteCounter_ += 1;

		return true;
	}

	//EventText
	public void EventTextSet(EventText eventText, string setStr, Color32 color = new Color32()) {
		eventTextEventManager_.EventTextSet(eventText, setStr, color);
	}
	public void EventTextsUpdateExecuteSet(EventTextEventManagerExecute setExecute) {
		eventTextEventManagerExecute_ = setExecute;
	}

	//HpGaugeParts
	public void HpGaugePartsSet(HpGaugeParts setEventHpGauge, float endFillAmount = 0, IMonsterData setMonsterData = null) {
		hpGaugePartsEventManager_.HpGaugePartsSet(setEventHpGauge, setMonsterData, endFillAmount);
	}
	public void HpGaugePartsUpdateExecuteSet(HpGaugePartsEventManagerExecute setExecute) {
		hpGaugePartsEventManagerExecute_ = setExecute;
	}

	//StatusInfoParts
	public void EventStatusInfoPartsSet(StatusInfoParts eventStatusInfoParts, Color32 setColor = new Color32()) {
		statusInfoPartsEventManager_.EventStatusInfoPartsSet(eventStatusInfoParts, setColor);
	}
	public void StatusInfoPartsUpdateExecuteSet(StatusInfoPartsEventManagerExecute setExecute) {
		statusInfoPartsEventManagerExecute_ = setExecute;
	}

	//UpdateImage
	public void UpdateImageSet(UpdateImage updateImage, Color32 color = new Color32(), float endFillAmount = 1) {
		updateImageEventManager_.UpdateImageSet(updateImage, color, endFillAmount);
	}
	public void UpdateImagesUpdateExecuteSet(UpdateImageEventManagerExecute setExecute) {
		updateImageEventManagerExecute_ = setExecute;
	}

	//シングルトン
	public AllEventManager() {
		//イベントの初期化
		sceneEvent_ = new t13.Event<AllEventManager>(this);
	}

	static private AllEventManager instance_ = null;
	static public AllEventManager GetInstance() {
		if (instance_ != null) return instance_;

		instance_ = new AllEventManager();
		return instance_;
	}
	static public void ReleaseInstance() { instance_ = null; }
}
