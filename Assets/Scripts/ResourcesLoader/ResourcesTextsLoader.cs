using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesTextsLoader {
	private List<string> filePaths_ = new List<string>();
	private List<string> datas_ = new List<string>();

	public string GetTexts(string filePath) {
		//２度目の読み込みだったら
		for(int i = 0;i < filePaths_.Count; ++i) {
			if(filePaths_[i] == filePath) {
				return datas_[i];
			}
		}

		TextAsset loadData = Resources.Load("Texts/" + filePath, typeof(TextAsset)) as TextAsset;

		filePaths_.Add(filePath);
		datas_.Add(loadData.text);

		return loadData.text;
	}

	//シングルトン
	private ResourcesTextsLoader() { }

	static private ResourcesTextsLoader instance_ = null;
	static public ResourcesTextsLoader GetInstance() {
		if (instance_ != null) return instance_;

		instance_ = new ResourcesTextsLoader();
		return instance_;
	}
	static public void ReleaseInstance() { instance_ = null; }
}
