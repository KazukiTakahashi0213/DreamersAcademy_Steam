using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveInputProvider : IInputProvider {
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
	public bool RightSelectMouseButton() {
		return false;
	}
	public bool LeftSelectMouseButton() {
		return false;
	}
	public bool SelectEnter() {
		return false;
	}
	public bool SelectBack() {
		return false;
	}
	public bool SelectNovelWindowActive() {
		return false;
	}
	public bool SelectMenu() {
		return false;
	}
	public bool SelectBackMouseButton() {
		return false;
	}
	public bool SelectMouseLeftTrigger() {
		return false;
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
		if(UpSelect()
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
