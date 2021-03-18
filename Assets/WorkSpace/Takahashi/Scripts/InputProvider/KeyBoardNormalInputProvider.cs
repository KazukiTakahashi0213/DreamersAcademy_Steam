using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardNormalInputProvider : IInputProvider {
	public bool UpSelect() {
		return Input.GetKey(KeyCode.UpArrow);
	}
	public bool DownSelect() {
		return Input.GetKey(KeyCode.DownArrow);
	}
	public bool RightSelect() {
		return Input.GetKey(KeyCode.RightArrow);
	}
	public bool LeftSelect() {
		return Input.GetKey(KeyCode.LeftArrow);
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
