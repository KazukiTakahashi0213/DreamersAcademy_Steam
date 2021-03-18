using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputProvider {
	bool UpSelect();
	bool DownSelect();
	bool RightSelect();
	bool LeftSelect();
	bool SelectEnter();
	bool SelectBack();
	bool SelectNovelWindowActive();
	bool SelectMenu();
	bool SelectMouseLeftButton();
	bool SelectMouseRightButton();
	GameObject[] MouseRayHitGameObjects();
	float MouseWheelValue();
}
