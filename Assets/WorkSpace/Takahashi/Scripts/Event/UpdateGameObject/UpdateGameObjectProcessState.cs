using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpdateGameObjectProcess {
	None
	, PosMove
	, RotMove
	, Max
}

public class UpdateGameObjectProcessState {
	public UpdateGameObjectProcessState(UpdateGameObjectProcess setState) {
		state_ = setState;
	}

	public UpdateGameObjectProcess state_;

	//None
	static private UpdateGameObjectProcess NoneUpdate(UpdateGameObjectProcessState mine, UpdateGameObject updateGameObject) {
		return mine.state_;
	}

	//PosMove
	static private UpdateGameObjectProcess PosMoveUpdate(UpdateGameObjectProcessState mine, UpdateGameObject updateGameObject) {
		if (updateGameObject.GetTimeCounter().measure(Time.deltaTime, updateGameObject.GetTimeRegulation())) {
			t13.UnityUtil.ObjectInFluctUpdatePosX(
				updateGameObject.GetGameObject(),
				updateGameObject.GetTimeFlucts(0),
				updateGameObject.GetEndVec3().x,
				updateGameObject.GetTimeRegulation(),
				updateGameObject.GetTimeRegulation()
				);

			t13.UnityUtil.ObjectInFluctUpdatePosY(
				updateGameObject.GetGameObject(),
				updateGameObject.GetTimeFlucts(1),
				updateGameObject.GetEndVec3().y,
				updateGameObject.GetTimeRegulation(),
				updateGameObject.GetTimeRegulation()
				);

			t13.UnityUtil.ObjectInFluctUpdatePosZ(
				updateGameObject.GetGameObject(),
				updateGameObject.GetTimeFlucts(2),
				updateGameObject.GetEndVec3().z,
				updateGameObject.GetTimeRegulation(),
				updateGameObject.GetTimeRegulation()
				);

			return UpdateGameObjectProcess.None;
		}
		else {
			t13.UnityUtil.ObjectInFluctUpdatePosX(
				updateGameObject.GetGameObject(),
				updateGameObject.GetTimeFlucts(0),
				updateGameObject.GetEndVec3().x,
				updateGameObject.GetTimeCounter().count(),
				updateGameObject.GetTimeRegulation()
				);

			t13.UnityUtil.ObjectInFluctUpdatePosY(
				updateGameObject.GetGameObject(),
				updateGameObject.GetTimeFlucts(1),
				updateGameObject.GetEndVec3().y,
				updateGameObject.GetTimeCounter().count(),
				updateGameObject.GetTimeRegulation()
				);

			t13.UnityUtil.ObjectInFluctUpdatePosZ(
				updateGameObject.GetGameObject(),
				updateGameObject.GetTimeFlucts(2),
				updateGameObject.GetEndVec3().z,
				updateGameObject.GetTimeCounter().count(),
				updateGameObject.GetTimeRegulation()
				);
		}

		return mine.state_;
	}

	//RotMove
	static private UpdateGameObjectProcess RotMoveUpdate(UpdateGameObjectProcessState mine, UpdateGameObject updateGameObject) {
		if (updateGameObject.GetTimeCounter().measure(Time.deltaTime, updateGameObject.GetTimeRegulation())) {
			t13.UnityUtil.ObjectInFluctUpdateRotX(
				updateGameObject.GetGameObject(),
				updateGameObject.GetTimeFlucts(0),
				updateGameObject.GetEndVec3().x,
				updateGameObject.GetTimeRegulation(),
				updateGameObject.GetTimeRegulation(),
				updateGameObject.addEulerVec3_.x
				);

			t13.UnityUtil.ObjectInFluctUpdateRotY(
				updateGameObject.GetGameObject(),
				updateGameObject.GetTimeFlucts(1),
				updateGameObject.GetEndVec3().y,
				updateGameObject.GetTimeRegulation(),
				updateGameObject.GetTimeRegulation(),
				updateGameObject.addEulerVec3_.y
				);

			t13.UnityUtil.ObjectInFluctUpdateRotZ(
				updateGameObject.GetGameObject(),
				updateGameObject.GetTimeFlucts(2),
				updateGameObject.GetEndVec3().z,
				updateGameObject.GetTimeRegulation(),
				updateGameObject.GetTimeRegulation(),
				updateGameObject.addEulerVec3_.z
				);

			updateGameObject.addEulerVec3_.x = updateGameObject.transform.localRotation.eulerAngles.x;
			updateGameObject.addEulerVec3_.y = updateGameObject.transform.localRotation.eulerAngles.y;
			updateGameObject.addEulerVec3_.z = updateGameObject.transform.localRotation.eulerAngles.z;

			return UpdateGameObjectProcess.None;
		}
		else {
			t13.UnityUtil.ObjectInFluctUpdateRotX(
				updateGameObject.GetGameObject(),
				updateGameObject.GetTimeFlucts(0),
				updateGameObject.GetEndVec3().x,
				updateGameObject.GetTimeCounter().count(),
				updateGameObject.GetTimeRegulation(),
				updateGameObject.addEulerVec3_.x
				);

			t13.UnityUtil.ObjectInFluctUpdateRotY(
				updateGameObject.GetGameObject(),
				updateGameObject.GetTimeFlucts(1),
				updateGameObject.GetEndVec3().y,
				updateGameObject.GetTimeCounter().count(),
				updateGameObject.GetTimeRegulation(),
				updateGameObject.addEulerVec3_.y
				);

			t13.UnityUtil.ObjectInFluctUpdateRotZ(
				updateGameObject.GetGameObject(),
				updateGameObject.GetTimeFlucts(2),
				updateGameObject.GetEndVec3().z,
				updateGameObject.GetTimeCounter().count(),
				updateGameObject.GetTimeRegulation(),
				updateGameObject.addEulerVec3_.z
				);
		}

		return mine.state_;
	}

	private delegate UpdateGameObjectProcess UpdateFunc(UpdateGameObjectProcessState mine, UpdateGameObject updateGameObject);

	private UpdateFunc[] updateFuncs_ = new UpdateFunc[(int)UpdateGameObjectProcess.Max] {
		NoneUpdate
		, PosMoveUpdate
		, RotMoveUpdate
	};
	public UpdateGameObjectProcess Update(UpdateGameObject updateGameObject) { return updateFuncs_[(int)state_](this, updateGameObject); }
}
