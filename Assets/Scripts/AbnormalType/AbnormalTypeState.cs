using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbnormalType {
	None
	,Burns
	,Poison
	,Sleep
	,Confusion
	,Hero
	,Max
}

public class AbnormalTypeState {
	public AbnormalTypeState(AbnormalType setState) {
		state_ = setState;
	}

	public AbnormalType state_;

	//None

	//Burns
	
	//Poison
	
	//Sleep
	
	//Confusion
	
	//Hero

	private string[] names = new string[(int)AbnormalType.Max] {
		"None",
		"やけど",
		"どく",
		"ねむり",
		"こんらん",
		"ヒーロー",
	};
	public string GetName() { return names[(int)state_]; }

	private Color32[] colors = new Color32[(int)AbnormalType.Max] {
		new Color32(0, 0, 0, 0),
		new Color32(255, 109, 94, 255),
		new Color32(211, 94, 255, 255),
		new Color32(186, 186, 186, 255),
		new Color32(177, 255, 94, 255),
		new Color32(94, 120, 255, 255),
	};
	public Color32 GetColor() { return colors[(int)state_]; }
}
