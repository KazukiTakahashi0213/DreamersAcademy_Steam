using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardNormalTriggerInputProvider : IInputProvider {
	public bool UpSelect() {
		if (Input.GetKeyDown(KeyCode.UpArrow)
			|| Input.GetKeyDown(KeyCode.W)
			) {
			return true;
		}

		return false;
	}
	public bool DownSelect() {
		if (Input.GetKeyDown(KeyCode.DownArrow)
			|| Input.GetKeyDown(KeyCode.S)
			) {
			return true;
		}

		return false;
	}
	public bool RightSelect() {
		if (Input.GetKeyDown(KeyCode.RightArrow)
			|| Input.GetKeyDown(KeyCode.D)
			) {
			return true;
		}

		return false;
	}
	public bool LeftSelect() {
		if (Input.GetKeyDown(KeyCode.LeftArrow)
			|| Input.GetKeyDown(KeyCode.A)
			) {
			return true;
		}

		return false;
	}
	public bool LeftSelectMouseButton() {
		GameObject[] hitGameObjects = MouseRayHitGameObjects();

		for (int i = 0; i < hitGameObjects.Length; ++i) {
			if (hitGameObjects[i].CompareTag("LeftButton")) {
				if (SelectMouseLeftTrigger()) return true;
			}
		}

		return false;
	}
	public bool RightSelectMouseButton() {
		GameObject[] hitGameObjects = MouseRayHitGameObjects();

		for (int i = 0; i < hitGameObjects.Length; ++i) {
			if (hitGameObjects[i].CompareTag("RightButton")) {
				if (SelectMouseLeftTrigger()) return true;
			}
		}

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
		return Input.GetMouseButtonDown(1);
	}
	public GameObject[] MouseRayHitGameObjects() {
		return t13.UnityUtil.MouseRayHit2DGameObjects();
	}
	public float MouseWheelValue() {
		return Input.GetAxis("Mouse ScrollWheel");
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
