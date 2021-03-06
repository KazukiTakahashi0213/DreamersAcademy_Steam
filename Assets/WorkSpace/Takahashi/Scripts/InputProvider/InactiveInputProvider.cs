﻿using System.Collections;
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
}
