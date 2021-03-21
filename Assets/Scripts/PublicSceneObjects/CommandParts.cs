using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandParts : MonoBehaviour {
	[SerializeField] private SpriteRenderer commandWindowSprite_ = null;
	[SerializeField] private List<ChoiceParts> commandWindowChoicesParts_ = null;
	[SerializeField] private UpdateGameObject eventGameObject_ = null;
	[SerializeField] private CursorParts cursorParts_ = null;
	[SerializeField] private int commandNumberWidth_ = 1;

	public SpriteRenderer GetCommandWindowSprite() { return commandWindowSprite_; }
	public int GetCommandWindowTextsCount() { return commandWindowChoicesParts_.Count; }
	public UpdateGameObject GetEventGameObject() { return eventGameObject_; }
	public CursorParts GetCursorParts() { return cursorParts_; }

	public void CommandWindowChoiceTextChange(int value, string changeContext) {
		commandWindowChoicesParts_[value].ChoiceTextChange(changeContext);
	}
	public void CommandWindowChoiceColorChange(int value, Color32 changeColor) {
		commandWindowChoicesParts_[value].ChoiceColorChange(changeColor);
	}

	public void CommandWindowChoicesColliderActive() {
		for(int i = 0;i < commandWindowChoicesParts_.Count; ++i) {
			commandWindowChoicesParts_[i].ColliderActiveSet(true);
		}
	}
	public void CommandWindowChoicesColliderInactive() {
		for (int i = 0; i < commandWindowChoicesParts_.Count; ++i) {
			commandWindowChoicesParts_[i].ColliderActiveSet(false);
		}
	}

	private int selectHorizontalNumber_ = 0;
	private int selectVerticalNumber_ = 0;
	public int SelectNumber() { return selectVerticalNumber_ * commandNumberWidth_ + selectHorizontalNumber_; }

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

		if (SelectNumber() >= commandWindowChoicesParts_.Count) {
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
	/// <summary>
	/// ヒットしていなかったら-1を返す
	/// </summary>
	public int CommandSelectForNumber(Vector3 addHorizontalCursorPos, Vector3 addVerticalCursorPos) {
		GameObject[] hitGameObjects = AllSceneManager.GetInstance().inputProvider_.MouseRayHitGameObjects();

		for (int i = 0; i < hitGameObjects.Length; ++i) {
			if (hitGameObjects[i].CompareTag("CommandChoice")) {
				ChoiceParts choiceParts = hitGameObjects[i].GetComponent<ChoiceParts>();

				int setHorizontalNumber = choiceParts.GetChoiceHorizontalNumber();
				int setVerticalNumber = choiceParts.GetChoiceVerticalNumber();
				int setSelectNumber = setVerticalNumber * commandNumberWidth_ + setHorizontalNumber;

				choicePartsHit_ = true;

				//同じ番号だったら
				if (setSelectNumber == SelectNumber()) return -1;

				//横軸が同じ番号じゃなかったら
				if (setHorizontalNumber != selectHorizontalNumber_) {
					//横軸の番号が高かったら
					if (setHorizontalNumber > selectHorizontalNumber_) {
						while (selectHorizontalNumber_ < setHorizontalNumber) {
							CommandSelectRight(addHorizontalCursorPos);
						}
					}
					else {
						while (selectHorizontalNumber_ > setHorizontalNumber) {
							CommandSelectLeft(-addHorizontalCursorPos);
						}
					}
				}
				//縦軸が同じ番号じゃなかったら
				if (setVerticalNumber != selectVerticalNumber_) {
					//縦軸の番号が高かったら
					if (setVerticalNumber > selectVerticalNumber_) {
						while (selectVerticalNumber_ < setVerticalNumber) {
							CommandSelectDown(-addVerticalCursorPos);
						}
					}
					else {
						while (selectVerticalNumber_ > setVerticalNumber) {
							CommandSelectUp(addVerticalCursorPos);
						}
					}
				}

				return setSelectNumber;
			}
		}

		choicePartsHit_ = false;

		return -1;
	}

	private bool choicePartsHit_ = false;
	
	public bool MouseLeftButtonTriggerActive() {
		return AllSceneManager.GetInstance().inputProvider_.SelectMouseLeftTrigger() && choicePartsHit_;
	}
}
