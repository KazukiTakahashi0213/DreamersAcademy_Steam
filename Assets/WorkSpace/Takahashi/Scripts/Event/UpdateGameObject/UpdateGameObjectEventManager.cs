using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateGameObjectEventManager {
	private UpdateGameObjectEventManagerExecuteState executeState_ = new UpdateGameObjectEventManagerExecuteState(UpdateGameObjectEventManagerExecute.None);

	private int updateGameObjectsExecuteCounter_ = 0;
	private List<UpdateGameObject> updateGameObjects_ = new List<UpdateGameObject>();
	private List<Vector3> endVec3s_ = new List<Vector3>();
	private List<List<UpdateGameObject>> executeUpdateGameObjects_ = new List<List<UpdateGameObject>>();
	private List<List<Vector3>> executeEndVec3s_ = new List<List<Vector3>>();
	private List<UpdateGameObjectEventManagerExecute> updateGameObjectEventManagerExecutes_ = new List<UpdateGameObjectEventManagerExecute>();

	public UpdateGameObjectEventManagerExecuteState GetExecuteState() { return executeState_; }

	public UpdateGameObject GetExecuteUpdateGameObjects(int value) { return executeUpdateGameObjects_[updateGameObjectsExecuteCounter_][value]; }
	public int GetExecuteUpdateGameObjectsCount() { return executeUpdateGameObjects_[updateGameObjectsExecuteCounter_].Count; }
	public Vector3 GetExecuteEndVec3s(int value) { return executeEndVec3s_[updateGameObjectsExecuteCounter_][value]; }

	public void UpdateGameObjectSet(UpdateGameObject updateGameObject, Vector3 endValue) {
		updateGameObjects_.Add(updateGameObject);
		endVec3s_.Add(endValue);
	}
	public void UpdateGameObjectsExecuteSet(UpdateGameObjectEventManagerExecute setExecute = UpdateGameObjectEventManagerExecute.None) {
		List<UpdateGameObject> addGameObjects = new List<UpdateGameObject>();
		List<Vector3> addVec3s = new List<Vector3>();

		for (int i = 0; i < updateGameObjects_.Count; ++i) {
			addGameObjects.Add(updateGameObjects_[i]);
			addVec3s.Add(endVec3s_[i]);
		}

		executeUpdateGameObjects_.Add(addGameObjects);
		executeEndVec3s_.Add(addVec3s);
		updateGameObjectEventManagerExecutes_.Add(setExecute);

		updateGameObjects_.Clear();
		endVec3s_.Clear();
	}

	public void UpdateGameObjectsUpdateExecute(float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {
		executeState_.state_ = updateGameObjectEventManagerExecutes_[updateGameObjectsExecuteCounter_];

		executeState_.Execute(this, timeRegulation, timeFluctProcess);

		updateGameObjectsExecuteCounter_ += 1;
	}
	public void UpdateGameObjectsActiveSetExecute(bool setActive) {
		for (int i = 0; i < executeUpdateGameObjects_[updateGameObjectsExecuteCounter_].Count; ++i) {
			executeUpdateGameObjects_[updateGameObjectsExecuteCounter_][i].GetGameObject().SetActive(setActive);
		}

		updateGameObjectsExecuteCounter_ += 1;
	}

	public void UpdateGameObjectsClear() {
		updateGameObjects_.Clear();
		endVec3s_.Clear();
		executeUpdateGameObjects_.Clear();
		executeEndVec3s_.Clear();
		updateGameObjectEventManagerExecutes_.Clear();

		updateGameObjectsExecuteCounter_ = 0;
	}
}
