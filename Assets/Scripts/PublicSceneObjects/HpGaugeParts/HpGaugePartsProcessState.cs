using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HpGaugePartsProcess {
    None,
    GaugeUpdate,
    Max
}

public class HpGaugePartsProcessState {
	public HpGaugePartsProcessState(HpGaugePartsProcess setState) {
		state_ = setState;
	}

    public HpGaugePartsProcess state_;

	//None
	static private HpGaugePartsProcess NoneUpdate(HpGaugePartsProcessState mine, HpGaugeParts hpGaugeParts) {
		return mine.state_;
	}

	//GaugeUpdate
	static private HpGaugePartsProcess GaugeUpdateUpdate(HpGaugePartsProcessState mine, HpGaugeParts hpGaugeParts) {
		if (hpGaugeParts.GetTimeCounter().measure(Time.deltaTime, hpGaugeParts.GetTimeRegulation())) {
			//イメージの拡大縮小の処理
			t13.UnityUtil.ImageInFluctUpdate(
				hpGaugeParts.GetGauge(),
				hpGaugeParts.GetTimeFluct(),
				hpGaugeParts.GetEndFillAmount(),
				hpGaugeParts.GetTimeRegulation(),
				hpGaugeParts.GetTimeRegulation()
				);

			//InfoTextの変更
			if (hpGaugeParts.GetInfoText() != null) {
				Text infoText = hpGaugeParts.GetInfoText();
				IMonsterData monsterData = hpGaugeParts.GetReferMonsterData();

				int result = (int)t13.Utility.ValueForPercentage(
					1,
					hpGaugeParts.GetEndFillAmount(),
					monsterData.RealHitPoint()
					);

				infoText.text = t13.Utility.HarfSizeForFullSize(result.ToString()) + "／" + t13.Utility.HarfSizeForFullSize(monsterData.RealHitPoint().ToString());
			}

			//緑 51,238,85,255
			//黄 238,209,51
			//赤 238,51,53
			//Gaugeの色の変更
			if (hpGaugeParts.GetGauge().fillAmount < 0.21f) {
				hpGaugeParts.GetGauge().color = new Color32(238, 51, 53, (byte)(hpGaugeParts.GetGauge().color.a * 255));
			}
			else if (hpGaugeParts.GetGauge().fillAmount < 0.52f) {
				hpGaugeParts.GetGauge().color = new Color32(238, 209, 51, (byte)(hpGaugeParts.GetGauge().color.a * 255));
			}
			else if (hpGaugeParts.GetGauge().fillAmount < 1.1f) {
				hpGaugeParts.GetGauge().color = new Color32(51, 238, 85, (byte)(hpGaugeParts.GetGauge().color.a * 255));
			}

			return HpGaugePartsProcess.None;
		}
		else {
			//イメージの拡大縮小の処理
			t13.UnityUtil.ImageInFluctUpdate(
				hpGaugeParts.GetGauge(),
				hpGaugeParts.GetTimeFluct(),
				hpGaugeParts.GetEndFillAmount(),
				hpGaugeParts.GetTimeCounter().count(),
				hpGaugeParts.GetTimeRegulation()
				) ;

			//InfoTextの変更
			if (hpGaugeParts.GetInfoText() != null) {
				Text infoText = hpGaugeParts.GetInfoText();
				IMonsterData monsterData = hpGaugeParts.GetReferMonsterData();

				int result = (int)t13.Utility.ValueForPercentage(
					1,
					hpGaugeParts.GetGauge().fillAmount,
					monsterData.RealHitPoint()
					);

				infoText.text = t13.Utility.HarfSizeForFullSize(result.ToString()) + "／" + t13.Utility.HarfSizeForFullSize(monsterData.RealHitPoint().ToString());
			}

			//緑 51,238,85,255
			//黄 238,209,51
			//赤 238,51,53
			//Gaugeの色の変更
			if (hpGaugeParts.GetGauge().fillAmount < 0.21f) {
				hpGaugeParts.GetGauge().color = new Color32(238, 51, 53, (byte)(hpGaugeParts.GetGauge().color.a * 255));
			}
			else if (hpGaugeParts.GetGauge().fillAmount < 0.52f) {
				hpGaugeParts.GetGauge().color = new Color32(238, 209, 51, (byte)(hpGaugeParts.GetGauge().color.a * 255));
			}
			else if (hpGaugeParts.GetGauge().fillAmount < 1.1f) {
				hpGaugeParts.GetGauge().color = new Color32(51, 238, 85, (byte)(hpGaugeParts.GetGauge().color.a * 255));
			}
		}

		return mine.state_;
	}

	private delegate HpGaugePartsProcess UpdateFunc(HpGaugePartsProcessState mine, HpGaugeParts hpGaugeParts);
	private UpdateFunc[] updateFuncs_ = new UpdateFunc[(int)HpGaugePartsProcess.Max] {
		NoneUpdate,
		GaugeUpdateUpdate
	};
	public HpGaugePartsProcess Update(HpGaugeParts hpGaugeParts) { return updateFuncs_[(int)state_](this, hpGaugeParts); }
}
