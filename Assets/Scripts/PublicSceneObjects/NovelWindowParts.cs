using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NovelWindowParts : MonoBehaviour {
	[SerializeField] SpriteRenderer novelWindowSprite_ = null;
	[SerializeField] EventText novelWindowEventText = null;
	[SerializeField] NovelBlinkIconParts novelBlinkIconParts_ = null;
	[SerializeField] UpdateGameObject updateGameObject_ = null;

	public SpriteRenderer GetNovelWindowSprite() { return novelWindowSprite_; }
	public Text GetNovelWindowText() { return novelWindowEventText.GetText(); }
	public EventText GetNovelWindowEventText() { return novelWindowEventText; }
	public NovelBlinkIconParts GetNovelBlinkIconParts() { return novelBlinkIconParts_; }
	public UpdateGameObject GetUpdateGameObject() { return updateGameObject_; }
}
