using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesSoundsLoader {
	private List<string> filePaths_ = new List<string>();
	private List<AudioClip> datas_ = new List<AudioClip>();

	public AudioClip GetSounds(string filePath) {
		//２度目の読み込みだったら
		for (int i = 0; i < filePaths_.Count; ++i) {
			if (filePaths_[i] == filePath) {
				return datas_[i];
			}
		}

		AudioClip loadData = Resources.Load("Sounds/" + filePath) as AudioClip;

		filePaths_.Add(filePath);
		datas_.Add(loadData);

		return loadData;
	}

	//シングルトン
	private ResourcesSoundsLoader() { }

	static private ResourcesSoundsLoader instance_ = null;
	static public ResourcesSoundsLoader GetInstance() {
		if (instance_ != null) return instance_;

		instance_ = new ResourcesSoundsLoader();
		return instance_;
	}
	static public void ReleaseInstance() { instance_ = null; }
}
