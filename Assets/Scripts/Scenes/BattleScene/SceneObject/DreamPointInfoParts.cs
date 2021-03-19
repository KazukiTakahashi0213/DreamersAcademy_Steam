using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamPointInfoParts : MonoBehaviour {
	//シーンオブジェクト
	[SerializeField] private UpdateImage dreamPointGaugeMeterUpdateImage_ = null;
	[SerializeField] private UpdateGameObject updateGameObject_ = null;

	public UpdateGameObject GetUpdateGameObject() { return updateGameObject_; }

	public void DPEffectEventSet(float setDreamPoint) {
		for (int i = 0; i < 2; ++i) {
			//ステータスインフォのDPの点滅演出
			AllEventManager.GetInstance().UpdateImageSet(
				dreamPointGaugeMeterUpdateImage_
				, new Color(dreamPointGaugeMeterUpdateImage_.GetImage().color.r, dreamPointGaugeMeterUpdateImage_.GetImage().color.g, dreamPointGaugeMeterUpdateImage_.GetImage().color.b, 0.3f)
				);
			AllEventManager.GetInstance().UpdateImagesUpdateExecuteSet(UpdateImageEventManagerExecute.ChangeColor);
			AllEventManager.GetInstance().AllUpdateEventExecute();

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(0.1f);

			//ステータスインフォのDPの点滅演出
			AllEventManager.GetInstance().UpdateImageSet(
				dreamPointGaugeMeterUpdateImage_
				, new Color(dreamPointGaugeMeterUpdateImage_.GetImage().color.r, dreamPointGaugeMeterUpdateImage_.GetImage().color.g, dreamPointGaugeMeterUpdateImage_.GetImage().color.b, 1)
				);
			AllEventManager.GetInstance().UpdateImagesUpdateExecuteSet(UpdateImageEventManagerExecute.ChangeColor);
			AllEventManager.GetInstance().AllUpdateEventExecute();

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(0.1f);
		}

		float endFillAmount = t13.Utility.ValueForPercentage(100, setDreamPoint, 1);
		AllEventManager.GetInstance().UpdateImageSet(
			dreamPointGaugeMeterUpdateImage_
			, new Color32()
			, endFillAmount
			);
		AllEventManager.GetInstance().UpdateImagesUpdateExecuteSet(UpdateImageEventManagerExecute.FillAmountUpdate);
		AllEventManager.GetInstance().AllUpdateEventExecute(1.0f);
	}

	public void GaugeReset() {
		dreamPointGaugeMeterUpdateImage_.GetImage().fillAmount = 0;
	}
}
