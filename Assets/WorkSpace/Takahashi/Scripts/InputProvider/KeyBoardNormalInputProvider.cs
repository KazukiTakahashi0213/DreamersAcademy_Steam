using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardNormalInputProvider : IInputProvider {
	public bool UpSelect() {
		if (Input.GetKey(KeyCode.UpArrow)
			|| Input.GetKey(KeyCode.W)
			) {
			return true;
		}

		return false;
	}
	public bool DownSelect() {
		if (Input.GetKey(KeyCode.DownArrow)
			|| Input.GetKey(KeyCode.S)
			) {
			return true;
		}

		return false;
	}
	public bool RightSelect() {
		if (Input.GetKey(KeyCode.RightArrow)
			|| Input.GetKey(KeyCode.D)
			) {
			return true;
		}

		return false;
	}
	public bool LeftSelect() {
		if (Input.GetKey(KeyCode.LeftArrow)
			|| Input.GetKey(KeyCode.A)
			) {
			return true;
		}

		return false;
	}
	public bool LeftSelectMouseButton() {
		GameObject[] hitGameObjects = MouseRayHitGameObjects();

		for(int i = 0;i < hitGameObjects.Length; ++i) {
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
		return Input.GetKey(KeyCode.W);
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
