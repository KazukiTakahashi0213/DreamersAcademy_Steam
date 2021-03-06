using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillDataNumber {
	None
	, Totugeki
	, Baandoraibu
	, Suimenngiri
	, Happabureedo
	, Shougekiha
	, Neppa
	, Wootaagan
	, Happabiimu
	, Heddoatakku
	, Bakunetupanchi
	, Taidaruhando
	, Gurandturii
	, Daihoukou
	, Neesisen
	, Maddoshotto
	, Siidobomu
	, Hasamiuchi
	, Hyakutonpuresu
	, Mienaite
	, Dairinshou
	, Areishotto
	, Mienaikoe
	, Tumedetuku
	, Aianpuresu
	, Sanninme
	, Musinokona
	, Aianbiimu
	, Turukamesuberi
	, Musinatakku
	, Faiasoodo
	, Suiryuuuchi
	, Grooturii
	, Midaresasi
	, Aiansoodo
	, Hyouipanchi
	, Otakebi
	, Tainetuhakka
	, Taihuun
	, Ibaranotoge
	, Kurosuekoo
	, Reerugan
	, Usikoku
	, Honoonokiri
	, Dokunokiri
	, Nemurinokiri
	, Memainokiri
	, Kousoku
	, Ketui
	, Kenbu
	, Mahouseisei
	, Tetunokokoro
	, Sinkousin
	, Baasutoappu
	, Rimeikaa
	, Max
}

public class SkillData : ISkillData {
	//EntryPoint
	public SkillData(SkillDataNumber skillDataNumber) {
		//初期化
		addPlayerParameterRanks_ = new List<AddParameterRankState>();
		addEnemyParameterRanks_ = new List<AddParameterRankState>();

		addPlayerAbnormalStates_ = new List<AddAbnormalTypeState>();
		addEnemyAbnormalStates_ = new List<AddAbnormalTypeState>();

		ResourcesSkillData data = ResourcesSkillDatasLoader.GetInstance().GetSkillDatas((int)skillDataNumber);

		skillNumber_ = (int)skillDataNumber;

		skillName_ = data.skillNname_;

		effectValue_ = data.effectValue_;

		optionEffectTriggerRateValue_ = data.optionEffectTriggerRateValue_;
		hitRateValue_ = data.hitRateValue_;
		upDpValue_ = data.upDpValue_;

		playPoint_ = data.playPoint_;
		nowPlayPoint_ = playPoint_;

		elementType_ = new ElementTypeState((ElementType)data.elementType_);
		effectType_ = new EffectTypeState((EffectType)data.effectType_[0], (EffectAttackType)data.effectType_[1]);

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

		for (int i = 0; i < data.addPlayerParameterRanks_.Length; ++i) {
			addPlayerParameterRanks_.Add(
				new AddParameterRankState((AddParameterRank)data.addPlayerParameterRanks_[i].addParameterRank_
				, data.addPlayerParameterRanks_[i].value_
				));
		}
		for (int i = 0; i < data.addEnemyParameterRanks_.Length; ++i) {
			addEnemyParameterRanks_.Add(
				new AddParameterRankState((AddParameterRank)data.addEnemyParameterRanks_[i].addParameterRank_
				, data.addEnemyParameterRanks_[i].value_
				));
		}

		for (int i = 0; i < data.addPlayerAbnormals_.Length; ++i) {
			addPlayerAbnormalStates_.Add(new AddAbnormalTypeState((AddAbnormalType)data.addPlayerAbnormals_[i].addAbnormal_));
		}
		for (int i = 0; i < data.addEnemyAbnormals_.Length; ++i) {
			addEnemyAbnormalStates_.Add(new AddAbnormalTypeState((AddAbnormalType)data.addEnemyAbnormals_[i].addAbnormal_));
		}

		effectInfo_ = data.effectInfo_;
	}

	public int skillNumber_ { get; }
	public string skillName_ { get; }

	public float effectValue_ { get; }

	public List<AddParameterRankState> addPlayerParameterRanks_ { get; }
	public List<AddParameterRankState> addEnemyParameterRanks_ { get; }

	public List<AddAbnormalTypeState> addPlayerAbnormalStates_ { get; }
	public List<AddAbnormalTypeState> addEnemyAbnormalStates_ { get; }

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
