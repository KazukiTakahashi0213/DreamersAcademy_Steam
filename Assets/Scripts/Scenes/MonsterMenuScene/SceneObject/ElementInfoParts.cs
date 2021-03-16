using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementInfoParts : MonoBehaviour {
	[SerializeField] private EventSpriteRenderer elementInfoEventSprite_ = null;
	[SerializeField] private EventText elementInfoEventText_ = null;

	public void ElementReflect(ElementTypeState elementTypeState) {
		//タイプの反映
		elementInfoEventSprite_.GetSpriteRenderer().color = elementTypeState.GetColor();

		//もしタイプがNoneだったら
		if (elementTypeState.state_ == ElementType.None) {
			//透明にする
			elementInfoEventText_.GetText().color = new Color32(0, 0, 0, 0);
		}
		else {
			//表示する
			elementInfoEventText_.GetText().color = new Color32(241, 241, 241, 255);
		}

		//名前の反映
		elementInfoEventText_.GetText().text = elementTypeState.GetName();
	}
	public void ElementReflectEventSet(ElementTypeState elementTypeState) {
		AllEventManager eventManager = AllEventManager.GetInstance();

		//タイプの反映
		eventManager.EventSpriteRendererSet(elementInfoEventSprite_, null, elementTypeState.GetColor());
		eventManager.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		eventManager.AllUpdateEventExecute();

		//もしタイプがNoneだったら
		if (elementTypeState.state_ == ElementType.None) {
			//透明にする
			eventManager.EventTextSet(elementInfoEventText_, "", new Color32(0, 0, 0, 0));
			eventManager.EventTextsUpdateExecuteSet(EventTextEventManagerExecute.ChangeColor);
			eventManager.AllUpdateEventExecute();
		}
		else {
			//表示する
			eventManager.EventTextSet(elementInfoEventText_, "", new Color32(241, 241, 241, 255));
			eventManager.EventTextsUpdateExecuteSet(EventTextEventManagerExecute.ChangeColor);
			eventManager.AllUpdateEventExecute();
		}

		//名前の反映
		eventManager.EventTextSet(elementInfoEventText_, elementTypeState.GetName());
		eventManager.EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
		eventManager.AllUpdateEventExecute();
	}
}
