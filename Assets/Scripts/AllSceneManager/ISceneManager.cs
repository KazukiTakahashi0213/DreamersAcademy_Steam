using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneManager {
	void SceneStart();
	void SceneUpdate();
	void SceneEnd();

	GameObject GetGameObject();
}
