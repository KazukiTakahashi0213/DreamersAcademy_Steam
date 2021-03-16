using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsPathSupervisor {
	//シングルトン
	private GraphicsPathSupervisor() { }

	static private GraphicsPathSupervisor instance_ = null;
	static public GraphicsPathSupervisor GetInstance() {
		if (instance_ != null) return instance_;

		instance_ = new GraphicsPathSupervisor();
		return instance_;
	}
	static public void ReleaseInstance() { instance_ = null; }

	//SaveDataSelectScene
	public string GetPathGameStartInfo() { return "SaveDataSelectScene/GameStartInfo"; }
	public string GetPathGameContinueInfo() { return "SaveDataSelectScene/GameContinueInfo"; }
}
