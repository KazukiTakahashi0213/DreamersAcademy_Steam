using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateGameObject : MonoBehaviour {
	//EntryPoint
	void Update() {
		//メイン処理
		processState_.state_ = processState_.Update(this);
	}

	private UpdateGameObjectProcessState processState_ = new UpdateGameObjectProcessState(UpdateGameObjectProcess.None);

	private t13.TimeFluct[] timeFlucts_ = new t13.TimeFluct[3]{
		new t13.TimeFluct()
		, new t13.TimeFluct()
		, new t13.TimeFluct()
	};
	private t13.TimeCounter timeCounter_ = new t13.TimeCounter();

	private float timeRegulation_ = 0;
	private Vector3 endVec3_ = new Vector3();
	public Vector3 addEulerVec3_ = new Vector3(0, 0, 0);

	public t13.TimeFluct GetTimeFlucts(int value) { return timeFlucts_[value]; }
	public t13.TimeCounter GetTimeCounter() { return timeCounter_; }

	public GameObject GetGameObject() { return gameObject; }

	public float GetTimeRegulation() { return timeRegulation_; }
	public Vector3 GetEndVec3() { return endVec3_; }

	public void ProcessStatePosMoveExecute(float timeRegulation, t13.TimeFluctProcess timeFluctProcess, Vector3 endVec3) {
		timeRegulation_ = timeRegulation;
		endVec3_ = endVec3;
		for (int i = 0; i < timeFlucts_.Length; ++i) {
			timeFlucts_[i].GetProcessState().state_ = timeFluctProcess;
		}

		processState_.state_ = UpdateGameObjectProcess.PosMove;
	}
	public void ProcessStateRotMoveExecute(float timeRegulation, t13.TimeFluctProcess timeFluctProcess, Vector3 endVec3) {
		timeRegulation_ = timeRegulation;
		endVec3_ = endVec3;
		for (int i = 0; i < timeFlucts_.Length; ++i) {
			timeFlucts_[i].GetProcessState().state_ = timeFluctProcess;
		}

		processState_.state_ = UpdateGameObjectProcess.RotMove;
	}
}
