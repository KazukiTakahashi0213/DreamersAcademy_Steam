using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletParts : MonoBehaviour {
	[SerializeField] private List<StatusInfoParts> eventStatusInfosParts_ = null;

	public StatusInfoParts GetEventStatusInfosParts(int number) { return eventStatusInfosParts_[number]; }
	public int GetEventStatusInfosPartsSize() { return eventStatusInfosParts_.Count; }

	private const float UPDATE_TIME_REGULATION = 0.4f;

	public void DownRollStatusInfoParts() {
		//1,2番目を-1.5fずらす
		for (int i = 1; i < eventStatusInfosParts_.Count - 2; ++i) {
			eventStatusInfosParts_[i].GetEventGameObject().ProcessStatePosMoveExecute(
				UPDATE_TIME_REGULATION
				, t13.TimeFluctProcess.Liner
				, new Vector3(eventStatusInfosParts_[i].transform.localPosition.x, eventStatusInfosParts_[i].transform.localPosition.y - 1.5f, eventStatusInfosParts_[i].transform.localPosition.z)
				) ;
			eventStatusInfosParts_[i].ProcessStateColorUpdateExecute(UPDATE_TIME_REGULATION, t13.TimeFluctProcess.Liner, new Color32(0, 0, 0, (byte)(255 / i)));
		}

		//0番目を映す
		eventStatusInfosParts_[0].ProcessStateColorUpdateExecute(UPDATE_TIME_REGULATION, t13.TimeFluctProcess.Liner, new Color32(0, 0, 0, 125));

		//3番目を消す
		eventStatusInfosParts_[3].ProcessStateAllColorUpdateExecute(UPDATE_TIME_REGULATION, t13.TimeFluctProcess.Liner, new Color32(0, 0, 0, 0));

		//4番目を上に移す
		eventStatusInfosParts_[4].GetEventGameObject().ProcessStatePosMoveExecute(
			0
			, t13.TimeFluctProcess.Liner
			, new Vector3(eventStatusInfosParts_[4].GetEventGameObject().transform.localPosition.x, 3.5f, eventStatusInfosParts_[4].GetEventGameObject().transform.localPosition.z)
			);

		StatusInfoParts temp = null;
		StatusInfoParts temp2 = null;

		temp = eventStatusInfosParts_[1];
		eventStatusInfosParts_[1] = eventStatusInfosParts_[0];

		temp2 = eventStatusInfosParts_[2];
		eventStatusInfosParts_[2] = temp;

		temp = eventStatusInfosParts_[3];
		eventStatusInfosParts_[3] = temp2;

		temp2 = eventStatusInfosParts_[4];
		eventStatusInfosParts_[4] = temp;

		eventStatusInfosParts_[0] = temp2;
	}
	public void UpRollStatusInfoParts() {
		//2,3番目を1.5fずらす
		for (int i = 2; i < eventStatusInfosParts_.Count - 1; ++i) {
			eventStatusInfosParts_[i].GetEventGameObject().ProcessStatePosMoveExecute(
				UPDATE_TIME_REGULATION
				, t13.TimeFluctProcess.Liner
				, new Vector3(eventStatusInfosParts_[i].transform.localPosition.x, eventStatusInfosParts_[i].transform.localPosition.y + 1.5f, eventStatusInfosParts_[i].transform.localPosition.z)
				);
			eventStatusInfosParts_[i].ProcessStateColorUpdateExecute(UPDATE_TIME_REGULATION, t13.TimeFluctProcess.Liner, new Color32(0, 0, 0, (byte)(255 / (((i + 1) % 2) + 1))));
		}

		//4番目を映す
		eventStatusInfosParts_[4].ProcessStateColorUpdateExecute(UPDATE_TIME_REGULATION, t13.TimeFluctProcess.Liner, new Color32(0, 0, 0, 125));

		//1番目を消す
		eventStatusInfosParts_[1].ProcessStateAllColorUpdateExecute(UPDATE_TIME_REGULATION, t13.TimeFluctProcess.Liner, new Color32(0, 0, 0, 0));

		//0番目を下に移す
		eventStatusInfosParts_[0].GetEventGameObject().ProcessStatePosMoveExecute(
			0
			, t13.TimeFluctProcess.Liner
			, new  Vector3(eventStatusInfosParts_[0].GetEventGameObject().transform.localPosition.x, 0.5f, eventStatusInfosParts_[0].GetEventGameObject().transform.localPosition.z)
			);

		StatusInfoParts beginData = eventStatusInfosParts_[0];

		for (int i = 0;i < eventStatusInfosParts_.Count - 1; ++i) {
			eventStatusInfosParts_[i] = eventStatusInfosParts_[i + 1];
		}

		eventStatusInfosParts_[eventStatusInfosParts_.Count - 1] = beginData;
	}
}
