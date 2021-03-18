using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardNormalTriggerInputProvider : IInputProvider {
	public bool UpSelect() {
		return Input.GetKeyDown(KeyCode.UpArrow);
	}
	public bool DownSelect() {
		return Input.GetKeyDown(KeyCode.DownArrow);
	}
	public bool RightSelect() {
		return Input.GetKeyDown(KeyCode.RightArrow);
	}
	public bool LeftSelect() {
		return Input.GetKeyDown(KeyCode.LeftArrow);
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
		return Input.GetMouseButtonDown(0);
	}
	public bool SelectMouseRightButton() {
		return Input.GetMouseButtonDown(1);
	}
	public GameObject[] MouseRayHitGameObjects() {
		return t13.UnityUtil.MouseRayHit2DGameObjects();
	}
	public float MouseWheelValue() {
		return Input.GetAxis("Mouse ScrollWheel");
	}
}
