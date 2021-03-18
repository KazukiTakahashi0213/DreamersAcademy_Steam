using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace t13 {

	public class UnityUtil {
		//moveObjectのx軸をmoveFluctで移動させる
		static public void ObjectInFluctUpdatePosX(GameObject moveObject, TimeFluct moveFluct, float endValue, float count, float regulation) {
			Vector3 objectPos = moveObject.transform.localPosition;

			objectPos.x = moveFluct.InFluct(count, objectPos.x, endValue, regulation);
			moveObject.transform.localPosition = objectPos;
		}
		//moveObjectのy軸をmoveFluctで移動させる
		static public void ObjectInFluctUpdatePosY(GameObject moveObject, t13.TimeFluct moveFluct, float endValue, float count, float regulation) {
			Vector3 objectPos = moveObject.transform.localPosition;

			objectPos.y = moveFluct.InFluct(count, objectPos.y, endValue, regulation);
			moveObject.transform.localPosition = objectPos;
		}
		//moveObjectのz軸をmoveFluctで移動させる
		static public void ObjectInFluctUpdatePosZ(GameObject moveObject, t13.TimeFluct moveFluct, float endValue, float count, float regulation) {
			Vector3 objectPos = moveObject.transform.localPosition;

			objectPos.z = moveFluct.InFluct(count, objectPos.z, endValue, regulation);
			moveObject.transform.localPosition = objectPos;
		}

		//moveObjectのx軸をmoveFluctで回転させる
		static public void ObjectInFluctUpdateRotX(GameObject moveObject, TimeFluct moveFluct, float endValue, float count, float regulation, float addEuler = 0) {
			Quaternion objectRot = moveObject.transform.localRotation;

			float angleAxis = moveFluct.InFluct(count, objectRot.eulerAngles.x + addEuler, endValue, regulation);
			moveObject.transform.localRotation = Quaternion.AngleAxis(angleAxis, new Vector3(1, 0, 0));
		}
		//moveObjectのy軸をmoveFluctで回転させる
		static public void ObjectInFluctUpdateRotY(GameObject moveObject, TimeFluct moveFluct, float endValue, float count, float regulation, float addEuler = 0) {
			Quaternion objectRot = moveObject.transform.localRotation;

			float angleAxis = moveFluct.InFluct(count, objectRot.eulerAngles.y + addEuler, endValue, regulation);
			moveObject.transform.localRotation = Quaternion.AngleAxis(angleAxis, new Vector3(0, 1, 0));
		}
		//moveObjectのz軸をmoveFluctで回転させる
		static public void ObjectInFluctUpdateRotZ(GameObject moveObject, TimeFluct moveFluct, float endValue, float count, float regulation, float addEuler = 0) {
			Quaternion objectRot = moveObject.transform.localRotation;

			float angleAxis = moveFluct.InFluct(count, objectRot.eulerAngles.z + addEuler, endValue, regulation);
			moveObject.transform.localRotation = Quaternion.AngleAxis(angleAxis, new Vector3(0, 0, 1));
		}

		//moveObjをmovePosに移動させる
		static public void ObjectPosMove(GameObject moveObj, Vector3 movePos) {
			Vector3 objPos = moveObj.transform.localPosition;
			objPos = movePos;

			moveObj.transform.localPosition = objPos;
		}
		//moveObjをmoveRotに移動させる
		static public void ObjectRotMove(GameObject moveObj, Quaternion moveRot) {
			Quaternion objRot = moveObj.transform.localRotation;
			objRot = moveRot;

			moveObj.transform.localRotation = objRot;
		}

		//moveObjをmovePos分、移動させる
		static public void ObjectPosAdd(GameObject moveObj, Vector3 movePos) {
			Vector3 objPos = moveObj.transform.localPosition;
			objPos += movePos;

			moveObj.transform.localPosition = objPos;
		}

		//fillImageをfillFluctで拡大縮小させる
		static public void ImageInFluctUpdate(Image fillImage, t13.TimeFluct fillFluct, float endFillAmount, float count, float regulation) {
			float imageFill = fillImage.fillAmount;

			imageFill = fillFluct.InFluct(count, imageFill, endFillAmount, regulation);
			fillImage.fillAmount = imageFill;
		}

		static public Color32 Color32InFluctUpdateRed(Color32 color, TimeFluct timeFluct, float endRed, float count, float timeRegulation) {
			float result = timeFluct.InFluct(count, color.r, endRed, timeRegulation);

			return new Color32((byte)result, color.g, color.b, color.a);
		}
		static public Color32 Color32InFluctUpdateGreen(Color32 color, TimeFluct timeFluct, float endGreen, float count, float timeRegulation) {
			float result = timeFluct.InFluct(count, color.g, endGreen, timeRegulation);

			return new Color32(color.r, (byte)result, color.b, color.a);
		}
		static public Color32 Color32InFluctUpdateBlue(Color32 color, TimeFluct timeFluct, float endBlue, float count, float timeRegulation) {
			float result = timeFluct.InFluct(count, color.b, endBlue, timeRegulation);

			return new Color32(color.r, color.g, (byte)result, color.a);
		}
		static public Color32 Color32InFluctUpdateAlpha(Color32 color, TimeFluct timeFluct, float endAlpha, float count, float timeRegulation) {
			float result = timeFluct.InFluct(count, color.a, endAlpha, timeRegulation);

			return new Color32(color.r, color.g, color.b, (byte)result);
		}

		static public Color32 ColorForColor32(Color color) {
			return new Color32((byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), (byte)(color.a * 255));
		}

		static public void GameQuit() {
			#if UNITY_EDITOR
						UnityEditor.EditorApplication.isPlaying = false;
			#elif UNITY_STANDALONE
			      UnityEngine.Application.Quit();
			#endif
		}

		static public GameObject[] MouseRayHitGameObjects() {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit[] raycast = Physics.RaycastAll(ray.origin, ray.direction);
			GameObject[] retGameObjects = new GameObject[raycast.Length];

			for (int i = 0; i < raycast.Length; ++i) {
				retGameObjects[i] = raycast[i].transform.parent.gameObject;
			}

			return retGameObjects;
		}
		static public GameObject[] MouseRayHit2DGameObjects() {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit2D[] raycast = Physics2D.RaycastAll(ray.origin, ray.direction);
			GameObject[] retGameObjects = new GameObject[raycast.Length];

			for(int i = 0;i < raycast.Length; ++i) {
				retGameObjects[i] = raycast[i].transform.parent.gameObject;
			}

			return retGameObjects;
		}
	}
}
