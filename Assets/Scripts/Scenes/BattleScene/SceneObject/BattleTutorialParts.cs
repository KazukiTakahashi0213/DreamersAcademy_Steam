using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTutorialParts : MonoBehaviour {
	[SerializeField] private List<Sprite> slideSprites_ = new List<Sprite>();
	[SerializeField] private SpriteRenderer battleTutorialSprite_ = null;
	[SerializeField] private SpriteRenderer battleTutorialLeftButtonSprite_ = null;
	[SerializeField] private SpriteRenderer battleTutorialRightButtonSprite_ = null;
	[SerializeField] private UpdateGameObject updateGameObject_ = null;

	public UpdateGameObject GetUpdateGameObject() { return updateGameObject_; }

	private int selectSlideNumber_ = 0;

	public void TutorialReset() {
		selectSlideNumber_ = 0;

		battleTutorialSprite_.sprite = slideSprites_[selectSlideNumber_];

		battleTutorialLeftButtonSprite_.gameObject.SetActive(false);
		battleTutorialRightButtonSprite_.gameObject.SetActive(true);
	}

	public void RightButtonDown() {
		if (selectSlideNumber_ == slideSprites_.Count-1) return;

		//最初のスライドだったら
		if(selectSlideNumber_ == 0) {
			battleTutorialLeftButtonSprite_.gameObject.SetActive(true);
		}
		//最後の前のスライドだったら
		if(selectSlideNumber_ == slideSprites_.Count-2) {
			battleTutorialRightButtonSprite_.gameObject.SetActive(false);
		}

		++selectSlideNumber_;

		battleTutorialSprite_.sprite = slideSprites_[selectSlideNumber_];
	}
	public void LeftButtonDown() {
		if (selectSlideNumber_ == 0) return;

		//最後のスライドだったら
		if (selectSlideNumber_ == slideSprites_.Count-1) {
			battleTutorialRightButtonSprite_.gameObject.SetActive(true);
		}
		//最初の前のスライドだったら
		if (selectSlideNumber_ == 1) {
			battleTutorialLeftButtonSprite_.gameObject.SetActive(false);
		}

		--selectSlideNumber_;

		battleTutorialSprite_.sprite = slideSprites_[selectSlideNumber_];
	}
}
