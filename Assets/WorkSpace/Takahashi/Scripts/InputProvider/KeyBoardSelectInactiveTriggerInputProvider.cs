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
	public bool SelectMouseLeftButton() {
		return false;
	}
	public bool SelectMouseRightButton() {
		return false;
	}
	public GameObject[] MouseRayHitGameObjects() {
		return new GameObject[0];
	}
	public float MouseWheelValue() {
		return 0;
	}
}
