using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParameterInfoFrameParts : MonoBehaviour {
	[SerializeField] private SpriteRenderer parameterInfoFrameSprite_ = null;
	[SerializeField] private List<MonsterParameterBarParts> monsterParameterBarsParts_ = null;
	[SerializeField] private Text monsterTypeInfoText_ = null;

	public SpriteRenderer GetParameterInfoFrameSprite_() { return parameterInfoFrameSprite_; }
	public MonsterParameterBarParts GetMonsterParameterBarsParts(int value) { return monsterParameterBarsParts_[value]; }
	public int GetMonsterParameterBarsPartsCount() { return monsterParameterBarsParts_.Count; }
	public Text GetMonsterTypeInfoText() { return monsterTypeInfoText_; }

	public void MonsterDataReflect(IMonsterData referMonsterData) {
		string elementInfoString = "";
		if(referMonsterData.tribesData_.firstElement_.state_ != ElementType.None) {
			elementInfoString += referMonsterData.tribesData_.firstElement_.GetName();
		}
		if (referMonsterData.tribesData_.secondElement_.state_ != ElementType.None) {
			elementInfoString += " /" + referMonsterData.tribesData_.secondElement_.GetName();
		}

		monsterTypeInfoText_.text = "タイプ　" + elementInfoString;

		monsterParameterBarsParts_[0].ParameterReflect(referMonsterData.RealHitPoint());
		monsterParameterBarsParts_[1].ParameterReflect(referMonsterData.RealAttack());
		monsterParameterBarsParts_[2].ParameterReflect(referMonsterData.RealDefense());
		monsterParameterBarsParts_[3].ParameterReflect(referMonsterData.RealSpeed());
	}
}
