using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourcesTrainerMonsterData {
	public string monsterName_;
	public string[] skillNames_;
}

[System.Serializable]
public class ResourcesEnemyTrainerData {
	public int trainerNumber_;
	public string trainerName_;
	public string jobName_;
	public string texName_;

	public int attackRate_;
	public int tradeRate_;

	public ResourcesTrainerMonsterData[] monsterDatas_;
}

public class ResourcesEnemyTrainerDatasLoader {
	//シングルトン
	private ResourcesEnemyTrainerDatasLoader() { }

	static private ResourcesEnemyTrainerDatasLoader instance_ = null;
	static public ResourcesEnemyTrainerDatasLoader GetInstance() {
		if (instance_ != null) return instance_;

		instance_ = new ResourcesEnemyTrainerDatasLoader();
		return instance_;
	}
	static public void ReleaseInstance() { instance_ = null; }

	//データ管理関係
	private List<ResourcesEnemyTrainerData> resourcesEnemyTrainerDatas = null;
	private List<string> resourcesEnemyTrainerDataNames_ = new List<string>();

	public ResourcesEnemyTrainerData GetEnemyTrainerDatas(int number) {
		if (resourcesEnemyTrainerDatas != null) return resourcesEnemyTrainerDatas[number];

		resourcesEnemyTrainerDatas = new List<ResourcesEnemyTrainerData>();

		TextAsset[] textAssets = Resources.LoadAll<TextAsset>("EnemyTrainerDatas");

		for (int i = 0; i < textAssets.Length; ++i) {
			ResourcesEnemyTrainerData data = JsonUtility.FromJson<ResourcesEnemyTrainerData>(textAssets[i].ToString());
			data.trainerNumber_ = i;
			resourcesEnemyTrainerDatas.Add(data);
			resourcesEnemyTrainerDataNames_.Add(data.trainerName_);
		}

		return resourcesEnemyTrainerDatas[number];
	}
	public ResourcesEnemyTrainerData GetMonsterDatas(string dataName) {
		if (resourcesEnemyTrainerDatas != null) {
			for (int i = 0; i < resourcesEnemyTrainerDataNames_.Count; ++i) {
				if (resourcesEnemyTrainerDataNames_[i] == dataName) {
					return resourcesEnemyTrainerDatas[i];
				}
			}

			return null;
		}

		resourcesEnemyTrainerDatas = new List<ResourcesEnemyTrainerData>();

		TextAsset[] textAssets = Resources.LoadAll<TextAsset>("EnemyTrainerDatas");

		for (int i = 0; i < textAssets.Length; ++i) {
			ResourcesEnemyTrainerData data = JsonUtility.FromJson<ResourcesEnemyTrainerData>(textAssets[i].ToString());
			data.trainerNumber_ = i;
			resourcesEnemyTrainerDatas.Add(data);
			resourcesEnemyTrainerDataNames_.Add(data.trainerName_);
		}

		for (int i = 0; i < resourcesEnemyTrainerDataNames_.Count; ++i) {
			if (resourcesEnemyTrainerDataNames_[i] == dataName) {
				return resourcesEnemyTrainerDatas[i];
			}
		}

		return null;
	}
}
