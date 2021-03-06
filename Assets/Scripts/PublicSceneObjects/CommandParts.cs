using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandParts : MonoBehaviour {
	[SerializeField] private SpriteRenderer commandWindowSprite_ = null;
	[SerializeField] private List<Text> commandWindowTexts_ = null;
	[SerializeField] private UpdateGameObject eventGameObject_ = null;
	[SerializeField] private CursorParts cursorParts_ = null;

	public SpriteRenderer GetCommandWindowSprite() { return commandWindowSprite_; }
	public Text GetCommandWindowTexts(int value) { return commandWindowTexts_[value]; }
	public int GetCommandWindowTextsCount() { return commandWindowTexts_.Count; }
	public UpdateGameObject GetEventGameObject() { return eventGameObject_; }
	public CursorParts GetCursorParts() { return cursorParts_; }

	private int selectNumber_ = 0;
	public int GetSelectNumber() { return selectNumber_; }
	/// <summary>
	/// カーソルが動かせたらtrueを返す
	/// </summary>
	public bool CommandSelect(int addSelectNumber, Vector3 addCursorPos) {
		selectNumber_ += addSelectNumber;

		if (selectNumber_ == commandWindowTexts_.Count) {
			selectNumber_ -= 1;

			return false;
		}
		if(selectNumber_ < 0) {
			selectNumber_ += 1;

			return false;
		}

		//カーソルの移動
		t13.UnityUtil.ObjectPosAdd(cursorParts_.gameObject, addCursorPos);

		return true;
	}
	public void SelectReset(Vector3 startCursorPos) {
		selectNumber_ = 0;

		cursorParts_.transform.localPosition = startCursorPos;
	}
}
