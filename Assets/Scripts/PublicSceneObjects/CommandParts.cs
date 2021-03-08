using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandParts : MonoBehaviour {
	[SerializeField] private SpriteRenderer commandWindowSprite_ = null;
	[SerializeField] private List<Text> commandWindowTexts_ = null;
	[SerializeField] private UpdateGameObject eventGameObject_ = null;
	[SerializeField] private CursorParts cursorParts_ = null;
	[SerializeField] private int commandNumberWidth_ = 1;

	public SpriteRenderer GetCommandWindowSprite() { return commandWindowSprite_; }
	public Text GetCommandWindowTexts(int value) { return commandWindowTexts_[value]; }
	public int GetCommandWindowTextsCount() { return commandWindowTexts_.Count; }
	public UpdateGameObject GetEventGameObject() { return eventGameObject_; }
	public CursorParts GetCursorParts() { return cursorParts_; }

	private int selectHorizontalNumber_ = 0;
	private int selectVerticalNumber_ = 0;
	public int SelectNumber() { return selectVerticalNumber_ * commandNumberWidth_ + selectHorizontalNumber_; }
	/// <summary>
	/// カーソルが動かせたらtrueを返す
	/// </summary>
	//public bool CommandSelect(int addSelectNumber, Vector3 addCursorPos) {
	//	selectNumber_ += addSelectNumber;
	//
	//	if (selectNumber_ >= commandWindowTexts_.Count) {
	//		selectNumber_ -= addSelectNumber;
	//
	//		return false;
	//	}
	//	if(selectNumber_ < 0) {
	//		selectNumber_ -= addSelectNumber;
	//
	//		return false;
	//	}
	//
	//	//カーソルの移動
	//	t13.UnityUtil.ObjectPosAdd(cursorParts_.gameObject, addCursorPos);
	//
	//	return true;
	//}
	public bool CommandSelectUp(Vector3 addCursorPos) {
		selectVerticalNumber_ -= 1;

		if (selectVerticalNumber_ < 0) {
			selectVerticalNumber_ += 1;

			return false;
		}

		//カーソルの移動
		t13.UnityUtil.ObjectPosAdd(cursorParts_.gameObject, addCursorPos);

		return true;
	}
	public bool CommandSelectDown(Vector3 addCursorPos) {
		selectVerticalNumber_ += 1;

		if (SelectNumber() >= commandWindowTexts_.Count) {
			selectVerticalNumber_ -= 1;

			return false;
		}

		//カーソルの移動
		t13.UnityUtil.ObjectPosAdd(cursorParts_.gameObject, addCursorPos);

		return true;
	}
	public bool CommandSelectRight(Vector3 addCursorPos) {
		selectHorizontalNumber_ += 1;

		if (selectHorizontalNumber_ >= commandNumberWidth_) {
			selectHorizontalNumber_ -= 1;

			return false;
		}

		//カーソルの移動
		t13.UnityUtil.ObjectPosAdd(cursorParts_.gameObject, addCursorPos);

		return true;
	}
	public bool CommandSelectLeft(Vector3 addCursorPos) {
		selectHorizontalNumber_ -= 1;

		if (selectHorizontalNumber_ < 0) {
			selectHorizontalNumber_ += 1;

			return false;
		}

		//カーソルの移動
		t13.UnityUtil.ObjectPosAdd(cursorParts_.gameObject, addCursorPos);

		return true;
	}
	public void SelectReset(Vector3 startCursorPos) {
		selectVerticalNumber_ = 0;
		selectHorizontalNumber_ = 0;

		cursorParts_.transform.localPosition = startCursorPos;
	}
}
