using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoFrameParts : MonoBehaviour{
	[SerializeField] private SpriteRenderer skillInfoFramePartsSprite_ = null;
	public SpriteRenderer GetSkillInfoFramePartsSprite() { return skillInfoFramePartsSprite_; }

	[SerializeField] private List<Text> texts_ = new List<Text>();
	public Text GetTexts(int value) { return texts_[value]; }
	public int GetTextsCount() { return texts_.Count; }

	public void SkillInfoReflect(ISkillData referSkillData) {
		//string playPointContext = t13.Utility.HarfSizeForFullSize(referSkillData.nowPlayPoint_.ToString()) + "／" + t13.Utility.HarfSizeForFullSize(referSkillData.playPoint_.ToString());

		texts_[0].text =
			//"PP　　　　" + playPointContext + "\n"
			"わざタイプ／" + referSkillData.elementType_.GetName() + "\n"
			+ "いりょく　　" + t13.Utility.HarfSizeForFullSize(referSkillData.effectValue_.ToString()) + "\n"
			+ "めいちゅう　" + t13.Utility.HarfSizeForFullSize(referSkillData.successRateValue_.ToString()) + "\n"
			//+ "アップ" + t13.Utility.HarfSizeForFullSize("DP") + "　" + t13.Utility.HarfSizeForFullSize(referSkillData.upDpValue_.ToString())
			;

		texts_[1].text = referSkillData.effectInfo_;
	}
	public void SkillInfoReset() {
		//string playPointContext = t13.Utility.HarfSizeForFullSize(referSkillData.nowPlayPoint_.ToString()) + "／" + t13.Utility.HarfSizeForFullSize(referSkillData.playPoint_.ToString());

		texts_[0].text =
			//"PP　　　　" + playPointContext + "\n"
			"わざタイプ／" + "ーー" + "\n"
			+ "いりょく　　" + "ーー" + "\n"
			+ "めいちゅう　" + "ーー" + "\n"
			//+ "アップ" + t13.Utility.HarfSizeForFullSize("DP") + "　" + t13.Utility.HarfSizeForFullSize(referSkillData.upDpValue_.ToString())
			;

		texts_[1].text = "ーー";
	}
}
