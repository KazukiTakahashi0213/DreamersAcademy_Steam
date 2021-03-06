using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterParameterBarParts : MonoBehaviour {
	[SerializeField] private Image monsterParameterBarImage_ = null;
	[SerializeField] private Text parameterLogoText_ = null;
	[SerializeField] private Text parameterValueText_ = null;

	public Image GetMonsterParameterBarImage() { return monsterParameterBarImage_; }
	public Text GetParameterLogoText() { return parameterLogoText_; }
	public Text GetParameterValueText() { return parameterValueText_; }

	public void ParameterReflect(float referValue) {
		float imagePercentage = t13.Utility.ValueForPercentage(300, referValue, 1);
		monsterParameterBarImage_.fillAmount = imagePercentage;

		parameterValueText_.text = t13.Utility.HarfSizeForFullSize(referValue.ToString());
	}
}
