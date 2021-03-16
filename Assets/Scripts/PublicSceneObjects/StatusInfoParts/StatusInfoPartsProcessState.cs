using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusInfoPartsProcess {
	None
	, ColorUpdate
	, AllColorUpdate
	, IdleMove
	, Max
}

public class StatusInfoPartsProcessState {
	public StatusInfoPartsProcessState(StatusInfoPartsProcess setState) {
		state_ = setState;
	}

	public StatusInfoPartsProcess state_;

	//None
	static private StatusInfoPartsProcess NoneUpdate(StatusInfoPartsProcessState mine, StatusInfoParts statusInfoParts) {
		return mine.state_;
	}

	//ColorUpdate
	static private StatusInfoPartsProcess ColorUpdateUpdate(StatusInfoPartsProcessState mine, StatusInfoParts statusInfoParts) {
		if(statusInfoParts.GetTimeCounter().measure(Time.deltaTime, statusInfoParts.GetTimeRegulation())) {
			BaseParts baseParts = statusInfoParts.GetBaseParts();
			FrameParts frameParts = statusInfoParts.GetFrameParts();
			AbnormalStateInfoParts firstAbnormalStateInfoParts = statusInfoParts.GetFirstAbnormalStateInfoParts();
			AbnormalStateInfoParts secondAbnormalStateInfoParts = statusInfoParts.GetSecondAbnormalStateInfoParts();

			baseParts.GetBaseSprite().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				t13.UnityUtil.ColorForColor32(baseParts.GetBaseSprite().color)
				, statusInfoParts.GetTimeFlucts(0)
				, 255
				, statusInfoParts.GetTimeRegulation()
				, statusInfoParts.GetTimeRegulation()
				);
			baseParts.GetHpLogoText().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				t13.UnityUtil.ColorForColor32(baseParts.GetHpLogoText().color)
				, statusInfoParts.GetTimeFlucts(1)
				, statusInfoParts.GetEndColor().a
				, statusInfoParts.GetTimeRegulation()
				, statusInfoParts.GetTimeRegulation()
				);
			baseParts.GetInfoNameEventText().GetText().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				t13.UnityUtil.ColorForColor32(baseParts.GetInfoNameEventText().GetText().color)
				, statusInfoParts.GetTimeFlucts(2)
				, statusInfoParts.GetEndColor().a
				, statusInfoParts.GetTimeRegulation()
				, statusInfoParts.GetTimeRegulation()
				);

			frameParts.GetFrameSprite().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				t13.UnityUtil.ColorForColor32(frameParts.GetFrameSprite().color)
				, statusInfoParts.GetTimeFlucts(4)
				, statusInfoParts.GetEndColor().a
				, statusInfoParts.GetTimeRegulation()
				, statusInfoParts.GetTimeRegulation()
				);
			if (frameParts.GetHpGaugeParts().GetInfoText() != null) {
				frameParts.GetHpGaugeParts().GetInfoText().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
					t13.UnityUtil.ColorForColor32(frameParts.GetHpGaugeParts().GetInfoText().color)
					, statusInfoParts.GetTimeFlucts(5)
					, statusInfoParts.GetEndColor().a
					, statusInfoParts.GetTimeRegulation()
					, statusInfoParts.GetTimeRegulation()
					);
			}
			frameParts.GetHpGaugeParts().GetGauge().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				t13.UnityUtil.ColorForColor32(frameParts.GetHpGaugeParts().GetGauge().color)
				, statusInfoParts.GetTimeFlucts(6)
				, statusInfoParts.GetEndColor().a
				, statusInfoParts.GetTimeRegulation()
				, statusInfoParts.GetTimeRegulation()
				);

			return StatusInfoPartsProcess.None;
		}
		else {
			BaseParts baseParts = statusInfoParts.GetBaseParts();
			FrameParts frameParts = statusInfoParts.GetFrameParts();
			AbnormalStateInfoParts firstAbnormalStateInfoParts = statusInfoParts.GetFirstAbnormalStateInfoParts();
			AbnormalStateInfoParts secondAbnormalStateInfoParts = statusInfoParts.GetSecondAbnormalStateInfoParts();

			baseParts.GetBaseSprite().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				t13.UnityUtil.ColorForColor32(baseParts.GetBaseSprite().color)
				, statusInfoParts.GetTimeFlucts(0)
				, 255
				, statusInfoParts.GetTimeCounter().count()
				, statusInfoParts.GetTimeRegulation()
				);
			baseParts.GetHpLogoText().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				t13.UnityUtil.ColorForColor32(baseParts.GetHpLogoText().color)
				, statusInfoParts.GetTimeFlucts(1)
				, statusInfoParts.GetEndColor().a
				, statusInfoParts.GetTimeCounter().count()
				, statusInfoParts.GetTimeRegulation()
				);
			baseParts.GetInfoNameEventText().GetText().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				t13.UnityUtil.ColorForColor32(baseParts.GetInfoNameEventText().GetText().color)
				, statusInfoParts.GetTimeFlucts(2)
				, statusInfoParts.GetEndColor().a
				, statusInfoParts.GetTimeCounter().count()
				, statusInfoParts.GetTimeRegulation()
				);

			frameParts.GetFrameSprite().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				t13.UnityUtil.ColorForColor32(frameParts.GetFrameSprite().color)
				, statusInfoParts.GetTimeFlucts(4)
				, statusInfoParts.GetEndColor().a
				, statusInfoParts.GetTimeCounter().count()
				, statusInfoParts.GetTimeRegulation()
				);
			if (frameParts.GetHpGaugeParts().GetInfoText() != null) {
				frameParts.GetHpGaugeParts().GetInfoText().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
					t13.UnityUtil.ColorForColor32(frameParts.GetHpGaugeParts().GetInfoText().color)
					, statusInfoParts.GetTimeFlucts(5)
					, statusInfoParts.GetEndColor().a
					, statusInfoParts.GetTimeCounter().count()
					, statusInfoParts.GetTimeRegulation()
					);
			}
			frameParts.GetHpGaugeParts().GetGauge().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				t13.UnityUtil.ColorForColor32(frameParts.GetHpGaugeParts().GetGauge().color)
				, statusInfoParts.GetTimeFlucts(6)
				, statusInfoParts.GetEndColor().a
				, statusInfoParts.GetTimeCounter().count()
				, statusInfoParts.GetTimeRegulation()
				);
		}

		return mine.state_;
	}

	//AllColorUpdate
	static private StatusInfoPartsProcess AllColorUpdateUpdate(StatusInfoPartsProcessState mine, StatusInfoParts statusInfoParts) {
		if (statusInfoParts.GetTimeCounter().measure(Time.deltaTime, statusInfoParts.GetTimeRegulation())) {
			BaseParts baseParts = statusInfoParts.GetBaseParts();
			FrameParts frameParts = statusInfoParts.GetFrameParts();
			AbnormalStateInfoParts firstAbnormalStateInfoParts = statusInfoParts.GetFirstAbnormalStateInfoParts();
			AbnormalStateInfoParts secondAbnormalStateInfoParts = statusInfoParts.GetSecondAbnormalStateInfoParts();

			baseParts.GetBaseSprite().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				t13.UnityUtil.ColorForColor32(baseParts.GetBaseSprite().color)
				, statusInfoParts.GetTimeFlucts(0)
				, statusInfoParts.GetEndColor().a
				, statusInfoParts.GetTimeRegulation()
				, statusInfoParts.GetTimeRegulation()
				);
			baseParts.GetHpLogoText().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				t13.UnityUtil.ColorForColor32(baseParts.GetHpLogoText().color)
				, statusInfoParts.GetTimeFlucts(1)
				, statusInfoParts.GetEndColor().a
				, statusInfoParts.GetTimeRegulation()
				, statusInfoParts.GetTimeRegulation()
				);
			baseParts.GetInfoNameEventText().GetText().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				t13.UnityUtil.ColorForColor32(baseParts.GetInfoNameEventText().GetText().color)
				, statusInfoParts.GetTimeFlucts(2)
				, statusInfoParts.GetEndColor().a
				, statusInfoParts.GetTimeRegulation()
				, statusInfoParts.GetTimeRegulation()
				);

			frameParts.GetFrameSprite().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				t13.UnityUtil.ColorForColor32(frameParts.GetFrameSprite().color)
				, statusInfoParts.GetTimeFlucts(4)
				, statusInfoParts.GetEndColor().a
				, statusInfoParts.GetTimeRegulation()
				, statusInfoParts.GetTimeRegulation()
				);
			if (frameParts.GetHpGaugeParts().GetInfoText() != null) {
				frameParts.GetHpGaugeParts().GetInfoText().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
					t13.UnityUtil.ColorForColor32(frameParts.GetHpGaugeParts().GetInfoText().color)
					, statusInfoParts.GetTimeFlucts(5)
					, statusInfoParts.GetEndColor().a
					, statusInfoParts.GetTimeRegulation()
					, statusInfoParts.GetTimeRegulation()
					);
			}
			frameParts.GetHpGaugeParts().GetGauge().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				t13.UnityUtil.ColorForColor32(frameParts.GetHpGaugeParts().GetGauge().color)
				, statusInfoParts.GetTimeFlucts(6)
				, statusInfoParts.GetEndColor().a
				, statusInfoParts.GetTimeRegulation()
				, statusInfoParts.GetTimeRegulation()
				);

			return StatusInfoPartsProcess.None;
		}
		else {
			BaseParts baseParts = statusInfoParts.GetBaseParts();
			FrameParts frameParts = statusInfoParts.GetFrameParts();
			AbnormalStateInfoParts firstAbnormalStateInfoParts = statusInfoParts.GetFirstAbnormalStateInfoParts();
			AbnormalStateInfoParts secondAbnormalStateInfoParts = statusInfoParts.GetSecondAbnormalStateInfoParts();

			baseParts.GetBaseSprite().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				t13.UnityUtil.ColorForColor32(baseParts.GetBaseSprite().color)
				, statusInfoParts.GetTimeFlucts(0)
				, statusInfoParts.GetEndColor().a
				, statusInfoParts.GetTimeCounter().count()
				, statusInfoParts.GetTimeRegulation()
				);
			baseParts.GetHpLogoText().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				t13.UnityUtil.ColorForColor32(baseParts.GetHpLogoText().color)
				, statusInfoParts.GetTimeFlucts(1)
				, statusInfoParts.GetEndColor().a
				, statusInfoParts.GetTimeCounter().count()
				, statusInfoParts.GetTimeRegulation()
				);
			baseParts.GetInfoNameEventText().GetText().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				t13.UnityUtil.ColorForColor32(baseParts.GetInfoNameEventText().GetText().color)
				, statusInfoParts.GetTimeFlucts(2)
				, statusInfoParts.GetEndColor().a
				, statusInfoParts.GetTimeCounter().count()
				, statusInfoParts.GetTimeRegulation()
				);

			frameParts.GetFrameSprite().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				t13.UnityUtil.ColorForColor32(frameParts.GetFrameSprite().color)
				, statusInfoParts.GetTimeFlucts(4)
				, statusInfoParts.GetEndColor().a
				, statusInfoParts.GetTimeCounter().count()
				, statusInfoParts.GetTimeRegulation()
				);
			if (frameParts.GetHpGaugeParts().GetInfoText() != null) {
				frameParts.GetHpGaugeParts().GetInfoText().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
					t13.UnityUtil.ColorForColor32(frameParts.GetHpGaugeParts().GetInfoText().color)
					, statusInfoParts.GetTimeFlucts(5)
					, statusInfoParts.GetEndColor().a
					, statusInfoParts.GetTimeCounter().count()
					, statusInfoParts.GetTimeRegulation()
					);
			}
			frameParts.GetHpGaugeParts().GetGauge().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				t13.UnityUtil.ColorForColor32(frameParts.GetHpGaugeParts().GetGauge().color)
				, statusInfoParts.GetTimeFlucts(6)
				, statusInfoParts.GetEndColor().a
				, statusInfoParts.GetTimeCounter().count()
				, statusInfoParts.GetTimeRegulation()
				);
		}

		return mine.state_;
	}

	//IdleMove
	static private StatusInfoPartsProcess IdleMoveUpdate(StatusInfoPartsProcessState mine, StatusInfoParts statusInfoParts) {
		if (statusInfoParts.GetTimeCounter().measure(Time.deltaTime, statusInfoParts.GetIdleTimeRegulation())) {
			Vector3 vec3 = new Vector3(
				statusInfoParts.gameObject.transform.position.x,
				statusInfoParts.gameObject.transform.position.y + statusInfoParts.GetProcessIdleState().addPos_,
				statusInfoParts.gameObject.transform.position.z
				);
			t13.UnityUtil.ObjectPosMove(statusInfoParts.gameObject, vec3);

			statusInfoParts.SetProcessIdleState(statusInfoParts.GetProcessIdleState().Next());
		}

		return mine.state_;
	}

	private delegate StatusInfoPartsProcess UpdateFunc(StatusInfoPartsProcessState mine, StatusInfoParts statusInfoParts);
	private UpdateFunc[] updateFuncs_ = new UpdateFunc[(int)StatusInfoPartsProcess.Max] {
		NoneUpdate
		, ColorUpdateUpdate
		, AllColorUpdateUpdate
		, IdleMoveUpdate
	};
	public StatusInfoPartsProcess Update(StatusInfoParts statusInfoParts) { return updateFuncs_[(int)state_](this, statusInfoParts); }
}
