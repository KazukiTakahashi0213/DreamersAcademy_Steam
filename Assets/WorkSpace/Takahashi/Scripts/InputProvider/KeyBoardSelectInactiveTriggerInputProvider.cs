using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardSelectInactiveTriggerInputProvider : IInputProvider {
	public bool UpSelect() {
		return false;
	}
	public bool DownSelect() {
		return false;
	}
	public bool RightSelect() {
		return false;
	}
	public bool LeftSelect() {
		return false;
	}
	public bool LeftSelectMouseButton() {
		return false;
	}
	public bool RightSelectMouseButton() {
		return false;
	}
	public bool SelectEnter() {
		return Input.GetKeyDown(KeyCode.Z);
	}
	public bool SelectBack() {
		return Input.GetKeyDown(KeyCode.X);
	}
	public bool SelectNovelWindowActive() {
		return Input.GetKeyDown(KeyCode.W);
	}
	public bool SelectMenu() {
		return Input.GetKeyDown(KeyCode.Space);
	}
	public bool SelectBackMouseButton() {
		GameObject[] hitGameObjects = MouseRayHitGameObjects();

		for (int i = 0; i < hitGameObjects.Length; ++i) {
			if (hitGameObjects[i].CompareTag("BackButton")) {
				if (SelectMouseLeftTrigger()) return true;
			}
		}

		return false;
	}
	public bool SelectMouseLeftTrigger() {
		return Input.GetMouseButtonDown(0);
	}
	public bool SelectMouseRightTrigger() {
		return false;
	}
	public GameObject[] MouseRayHitGameObjects() {
		return new GameObject[0];
	}
	public float MouseWheelValue() {
		return 0;
	}

	public bool AnyKeyTrigger() {
		if (UpSelect()
			|| DownSelect()
			|| RightSelect()
			|| LeftSelect()
			|| SelectEnter()
			|| SelectBack()
			|| SelectNovelWindowActive()
			|| SelectMenu()
			|| SelectMouseLeftTrigger()
			|| SelectMouseRightTrigger()
			) {
			return true;
		}

		return false;
	}
}
