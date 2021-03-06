using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapSceneProcess {
	None
	, PlayerMove
	, EventExecute
	, MenuSelect
	, TradeMonsterSelect
	, Max
}

public class MapSceneProcessProvider {
	public MapSceneProcessProvider() {
		baseMapSceneProcessStates_.Add(new MapSceneProcessNone());
		baseMapSceneProcessStates_.Add(new MapSceneProcessPlayerMove());
		baseMapSceneProcessStates_.Add(new MapSceneProcessEventExecute());
		baseMapSceneProcessStates_.Add(new MapSceneProcessMenuSelect());
		baseMapSceneProcessStates_.Add(new MapSceneProcessTradeMonsterSelect());
	}

	public MapSceneProcess state_ = MapSceneProcess.None;

	private List<BMapSceneProcessState> baseMapSceneProcessStates_ = new List<BMapSceneProcessState>();

	public MapSceneProcess Update(MapManager mapManager) { return baseMapSceneProcessStates_[(int)state_].Update(mapManager); }
}
