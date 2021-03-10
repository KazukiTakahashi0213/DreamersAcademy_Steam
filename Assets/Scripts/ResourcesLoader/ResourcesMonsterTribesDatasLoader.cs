using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourcesMonsterTribesData {
	public int monsterNumber_;
	public string monsterName_;

	public int tribesHitPoint_;
	public int tribesAttack_;
	public int tribesDefense_;
	public int tribesSpeed_;

	public int firstElement_;
	public int secondElement_;

	public string texName_;
}

public class ResourcesMonsterTribesDatasLoader {
	List<ResourcesMonsterTribesData> resourcesMonsterTribesDatas = null;
	List<string> resourcesMonsterTribesDataNames_ = new List<string>();

	public ResourcesMonsterTribesData GetMonsterDatas(int number) {
		if (resourcesMonsterTribesDatas != null) return resourcesMonsterTribesDatas[number];

		resourcesMonsterTribesDatas = new List<ResourcesMonsterTribesData>();

		TextAsset[] textAssets = Resources.LoadAll<TextAsset>("MonsterDatas");

		for (int i = 0; i < textAssets.Length; ++i) {
			ResourcesMonsterTribesData data = JsonUtility.FromJson<ResourcesMonsterTribesData>(textAssets[i].ToString());
			data.monsterNumber_ = i;
			resourcesMonsterTribesDatas.Add(data);
			resourcesMonsterTribesDataNames_.Add(data.monsterName_);
		}

		return resourcesMonsterTribesDatas[number];
	}
	public ResourcesMonsterTribesData GetMonsterDatas(string dataName) {
		if (resourcesMonsterTribesDatas != null) {
			for (int i = 0; i < resourcesMonsterTribesDataNames_.Count; ++i) {
				if (resourcesMonsterTribesDataNames_[i] == dataName) {
					return resourcesMonsterTribesDatas[i];
				}
			}

			return null;
		}

		resourcesMonsterTribesDatas = new List<ResourcesMonsterTribesData>();

		TextAsset[] textAssets = Resources.LoadAll<TextAsset>("MonsterDatas");

		for (int i = 0; i < textAssets.Length; ++i) {
			ResourcesMonsterTribesData data = JsonUtility.FromJson<ResourcesMonsterTribesData>(textAssets[i].ToString());
			data.monsterNumber_ = i;
			resourcesMonsterTribesDatas.Add(data);
			resourcesMonsterTribesDataNames_.Add(data.monsterName_);
		}

		for (int i = 0; i < resourcesMonsterTribesDataNames_.Count; ++i) {
			if (resourcesMonsterTribesDataNames_[i] == dataName) {
				return resourcesMonsterTribesDatas[i];
			}
		}

		return null;
	}

	//シングルトン
	private ResourcesMonsterTribesDatasLoader() { }

	static private ResourcesMonsterTribesDatasLoader instance_ = null;
	static public ResourcesMonsterTribesDatasLoader GetInstance() {
		if (instance_ != null) return instance_;

		instance_ = new ResourcesMonsterTribesDatasLoader();
		return instance_;
	}
	static public void ReleaseInstance() { instance_ = null; }
}
