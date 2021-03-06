using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBugMenuSceneProcessState {
	public virtual BugMenuSceneProcess Update(BugMenuManager bugMenuManager) { return BugMenuSceneProcess.None; }
}
