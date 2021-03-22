using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputProvider {
	bool UpSelect();
	bool DownSelect();
	bool RightSelect();
	bool LeftSelect();
	bool UpSelectMouseButton();
	bool DownSelectMouseButton();
	bool RightSelectMouseButton();
	bool LeftSelectMouseButton();
	bool SelectEnter();
	bool SelectBack();
	bool SelectNovelWindowActive();
	bool SelectMenu();
	bool SelectBackMouseButton();
	bool SelectMouseLeftTrigger();
	bool SelectMouseRightTrigger();
	GameObject[] MouseRayHitGameObjects();
	float MouseWheelValue();

	bool AnyKeyTrigger();
}
