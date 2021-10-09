using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

// Token: 0x02000035 RID: 53
public class SlowmoEffect : MonoBehaviour
{
	// Token: 0x17000018 RID: 24
	// (get) Token: 0x060001AE RID: 430 RVA: 0x0000AAE4 File Offset: 0x00008CE4
	// (set) Token: 0x060001AF RID: 431 RVA: 0x0000AAEB File Offset: 0x00008CEB
	public static SlowmoEffect Instance { get; private set; }

	// Token: 0x060001B0 RID: 432 RVA: 0x0000AAF3 File Offset: 0x00008CF3
	private void Start()
	{
		this.cg = this.pp.GetSetting<ColorGrading>();
		SlowmoEffect.Instance = this;
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x0000AB0C File Offset: 0x00008D0C
	private void Update()
	{
		if (!this.af || !this.lf)
		{
			return;
		}
		if (!Game.Instance.playing || !Camera.main)
		{
			if (this.cg.hueShift.value != 0f)
			{
				this.cg.hueShift.value = 0f;
			}
			return;
		}
		float timeScale = Time.timeScale;
		float num = (1f - timeScale) * 2f;
		if ((double)num > 0.7)
		{
			num = 0.7f;
		}
		this.blackFx.color = new Color(1f, 1f, 1f, num);
		float target = PlayerMovement.Instance.GetActionMeter();
		float target2 = 0f;
		if (timeScale < 0.9f)
		{
			target = 400f;
			target2 = -20f;
		}
		this.frequency = Mathf.SmoothDamp(this.frequency, target, ref this.vel, 0.1f);
		this.hue = Mathf.SmoothDamp(this.hue, target2, ref this.hueVel, 0.2f);
		if (this.af)
		{
			this.af.distortionLevel = num * 0.2f;
		}
		if (this.lf)
		{
			this.lf.cutoffFrequency = this.frequency;
		}
		if (this.cg)
		{
			this.cg.hueShift.value = this.hue;
		}
		if (!Game.Instance.playing)
		{
			this.cg.hueShift.value = 0f;
		}
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x0000ACA0 File Offset: 0x00008EA0
	public void NewScene(AudioLowPassFilter l, AudioDistortionFilter d)
	{
		this.lf = l;
		this.af = d;
	}

	// Token: 0x0400017D RID: 381
	public Image blackFx;

	// Token: 0x0400017E RID: 382
	public PostProcessProfile pp;

	// Token: 0x0400017F RID: 383
	private ColorGrading cg;

	// Token: 0x04000181 RID: 385
	private float frequency;

	// Token: 0x04000182 RID: 386
	private float vel;

	// Token: 0x04000183 RID: 387
	private float hue;

	// Token: 0x04000184 RID: 388
	private float hueVel;

	// Token: 0x04000185 RID: 389
	private AudioDistortionFilter af;

	// Token: 0x04000186 RID: 390
	private AudioLowPassFilter lf;
}
