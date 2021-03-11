using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillDataNumber {
	None
	, Max
}

public class SkillData : ISkillData {
	//EntryPoint
	public SkillData(SkillDataNumber skillDataNumber) {
		//初期化
		addSelfParameterRanks_ = new List<AddParameterRankState>();
		addOtherParameterRanks_ = new List<AddParameterRankState>();

		addSelfAbnormalStates_ = new List<AddAbnormalTypeState>();
		addOtherAbnormalStates_ = new List<AddAbnormalTypeState>();

		ResourcesSkillData data = ResourcesSkillDatasLoader.GetInstance().GetSkillDatas((int)skillDataNumber);

		skillNumber_ = data.skillNumber_;

		skillName_ = data.skillName_;

		effectValue_ = data.effectValue_;

		optionEffectTriggerRateValue_ = data.optionEffectTriggerRateValue_;
		hitRateValue_ = data.hitRateValue_;
		upDpValue_ = data.upDpValue_;

		playPoint_ = data.playPoint_;
		nowPlayPoint_ = playPoint_;

		elementType_ = new ElementTypeState((ElementType)data.elementType_);
		effectType_ = new EffectTypeState((EffectType)data.effectType_, EffectAttackType.Normal);

		triggerPriority_ = data.triggerPriority_;
		criticalParameterRank_ = data.criticalParameterRank_;

		if (data.effectName_ == "NoneEffect") {
			effectAnimeSprites_.Add(ResourcesGraphicsLoader.GetInstance().GetGraphics("SkillEffect/" + data.effectName_));
		}
		else {
			Sprite[] sprite = ResourcesGraphicsLoader.GetInstance().GetGraphicsAll("SkillEffect/" + data.effectName_);
			for (int i = 0; i < sprite.Length; ++i) {
				effectAnimeSprites_.Add(sprite[i]);
			}
			effectSound_ = ResourcesSoundsLoader.GetInstance().GetSounds("SE/SkillEffect/" + data.effectName_);
		}

		for (int i = 0; i < data.addSelfParameterRanks_.Length; ++i) {
			addSelfParameterRanks_.Add(
				new AddParameterRankState((AddParameterRank)data.addSelfParameterRanks_[i].addParameterRank_
				, data.addSelfParameterRanks_[i].value_
				));
		}
		for (int i = 0; i < data.addOtherParameterRanks_.Length; ++i) {
			addOtherParameterRanks_.Add(
				new AddParameterRankState((AddParameterRank)data.addOtherParameterRanks_[i].addParameterRank_
				, data.addOtherParameterRanks_[i].value_
				));
		}

		for (int i = 0; i < data.addSelfAbnormals_.Length; ++i) {
			addSelfAbnormalStates_.Add(new AddAbnormalTypeState((AddAbnormalType)data.addSelfAbnormals_[i].addAbnormal_));
		}
		for (int i = 0; i < data.addOtherAbnormals_.Length; ++i) {
			addOtherAbnormalStates_.Add(new AddAbnormalTypeState((AddAbnormalType)data.addOtherAbnormals_[i].addAbnormal_));
		}

		effectInfo_ = data.effectInfo_;
	}
	public SkillData(string skillDataName) {
		//初期化
		addSelfParameterRanks_ = new List<AddParameterRankState>();
		addOtherParameterRanks_ = new List<AddParameterRankState>();

		addSelfAbnormalStates_ = new List<AddAbnormalTypeState>();
		addOtherAbnormalStates_ = new List<AddAbnormalTypeState>();

		ResourcesSkillData data = ResourcesSkillDatasLoader.GetInstance().GetSkillDatas(skillDataName);

		skillNumber_ = data.skillNumber_;

		skillName_ = data.skillName_;

		effectValue_ = data.effectValue_;

		optionEffectTriggerRateValue_ = data.optionEffectTriggerRateValue_;
		hitRateValue_ = data.hitRateValue_;
		upDpValue_ = data.upDpValue_;

		playPoint_ = data.playPoint_;
		nowPlayPoint_ = playPoint_;

		elementType_ = new ElementTypeState((ElementType)data.elementType_);
		effectType_ = new EffectTypeState((EffectType)data.effectType_, EffectAttackType.Normal);

		triggerPriority_ = data.triggerPriority_;
		criticalParameterRank_ = data.criticalParameterRank_;

		if (data.effectName_ == "NoneEffect") {
			effectAnimeSprites_.Add(ResourcesGraphicsLoader.GetInstance().GetGraphics("SkillEffect/" + data.effectName_));
		}
		else {
			Sprite[] sprite = ResourcesGraphicsLoader.GetInstance().GetGraphicsAll("SkillEffect/" + data.effectName_);
			for (int i = 0; i < sprite.Length; ++i) {
				effectAnimeSprites_.Add(sprite[i]);
			}
			effectSound_ = ResourcesSoundsLoader.GetInstance().GetSounds("SE/SkillEffect/" + data.effectName_);
		}

		for (int i = 0; i < data.addSelfParameterRanks_.Length; ++i) {
			addSelfParameterRanks_.Add(
				new AddParameterRankState((AddParameterRank)data.addSelfParameterRanks_[i].addParameterRank_
				, data.addSelfParameterRanks_[i].value_
				));
		}
		for (int i = 0; i < data.addOtherParameterRanks_.Length; ++i) {
			addOtherParameterRanks_.Add(
				new AddParameterRankState((AddParameterRank)data.addOtherParameterRanks_[i].addParameterRank_
				, data.addOtherParameterRanks_[i].value_
				));
		}

		for (int i = 0; i < data.addSelfAbnormals_.Length; ++i) {
			addSelfAbnormalStates_.Add(new AddAbnormalTypeState((AddAbnormalType)data.addSelfAbnormals_[i].addAbnormal_));
		}
		for (int i = 0; i < data.addOtherAbnormals_.Length; ++i) {
			addOtherAbnormalStates_.Add(new AddAbnormalTypeState((AddAbnormalType)data.addOtherAbnormals_[i].addAbnormal_));
		}

		effectInfo_ = data.effectInfo_;
	}

	public int skillNumber_ { get; }
	public string skillName_ { get; }

	public float effectValue_ { get; }

	public List<AddParameterRankState> addSelfParameterRanks_ { get; }
	public List<AddParameterRankState> addOtherParameterRanks_ { get; }

	public List<AddAbnormalTypeState> addSelfAbnormalStates_ { get; }
	public List<AddAbnormalTypeState> addOtherAbnormalStates_ { get; }

	public int optionEffectTriggerRateValue_ { get; }
	public int hitRateValue_ { get; }
	public int upDpValue_ { get; }

	public int playPoint_ { get; }
	public int nowPlayPoint_ { get; set; }

	public ElementTypeState elementType_ { get; }
	public EffectTypeState effectType_ { get; }

	public int triggerPriority_ { get; }
	public int criticalParameterRank_ { get; }

	private List<Sprite> effectAnimeSprites_ = new List<Sprite>();
	private AudioClip effectSound_ = null;

	public string effectInfo_ { get; }

	public void EffectAnimetionEventSet(EffectParts targetEffectParts) {
		//SE
		AllEventManager.GetInstance().SEAudioPlayOneShotEventSet(effectSound_);

		//アニメーション
		AllEventManager.GetInstance().EventSpriteRendererSet(targetEffectParts.GetEventSpriteRenderer(), effectAnimeSprites_);
		AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.Anime);
		AllEventManager.GetInstance().AllUpdateEventExecute(0.35f);
	}
}
