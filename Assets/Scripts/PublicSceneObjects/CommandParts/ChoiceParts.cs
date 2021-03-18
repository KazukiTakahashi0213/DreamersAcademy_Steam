using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceParts : MonoBehaviour {
	//シーンオブジェクト
	[SerializeField] private Text choiceText_ = null;
	[SerializeField] private BoxCollider2D choiceCollider_ = null;
	public void ColliderActiveSet(bool setActive) {
		choiceCollider_.transform.gameObject.SetActive(setActive);
	}

	public void ChoiceTextChange(string changeContext) {
		choiceText_.text = changeContext;
	}
	public void ChoiceColorChange(Color32 changeColor) {
		choiceText_.color = changeColor;
	}

	[SerializeField] private int choiceHorizontalNumber_ = 0;
	[SerializeField] private int choiceVerticalNumber_ = 0;
	public int GetChoiceHorizontalNumber() { return choiceHorizontalNumber_; }
	public int GetChoiceVerticalNumber() { return choiceVerticalNumber_; }
}
