using System;
using UnityEngine;

namespace EZCameraShake
{
	// Token: 0x0200003C RID: 60
	public class CameraShakeInstance
	{
		// Token: 0x060001DC RID: 476 RVA: 0x0000B4C4 File Offset: 0x000096C4
		public CameraShakeInstance(float magnitude, float roughness, float fadeInTime, float fadeOutTime)
		{
			this.Magnitude = magnitude;
			this.fadeOutDuration = fadeOutTime;
			this.fadeInDuration = fadeInTime;
			this.Roughness = roughness;
			if (fadeInTime > 0f)
			{
				this.sustain = true;
				this.currentFadeTime = 0f;
			}
			else
			{
				this.sustain = false;
				this.currentFadeTime = 1f;
			}
			this.tick = (float)UnityEngine.Random.Range(-100, 100);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000B550 File Offset: 0x00009750
		public CameraShakeInstance(float magnitude, float roughness)
		{
			this.Magnitude = magnitude;
			this.Roughness = roughness;
			this.sustain = true;
			this.tick = (float)UnityEngine.Random.Range(-100, 100);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000B5A8 File Offset: 0x000097A8
		public Vector3 UpdateShake()
		{
			this.amt.x = Mathf.PerlinNoise(this.tick, 0f) - 0.5f;
			this.amt.y = Mathf.PerlinNoise(0f, this.tick) - 0.5f;
			this.amt.z = Mathf.PerlinNoise(this.tick, this.tick) - 0.5f;
			if (this.fadeInDuration > 0f && this.sustain)
			{
				if (this.currentFadeTime < 1f)
				{
					this.currentFadeTime += Time.deltaTime / this.fadeInDuration;
				}
				else if (this.fadeOutDuration > 0f)
				{
					this.sustain = false;
				}
			}
			if (!this.sustain)
			{
				this.currentFadeTime -= Time.deltaTime / this.fadeOutDuration;
			}
			if (this.sustain)
			{
				this.tick += Time.deltaTime * this.Roughness * this.roughMod;
			}
			else
			{
				this.tick += Time.deltaTime * this.Roughness * this.roughMod * this.currentFadeTime;
			}
			return this.amt * this.Magnitude * this.magnMod * this.currentFadeTime;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000B703 File Offset: 0x00009903
		public void StartFadeOut(float fadeOutTime)
		{
			if (fadeOutTime == 0f)
			{
				this.currentFadeTime = 0f;
			}
			this.fadeOutDuration = fadeOutTime;
			this.fadeInDuration = 0f;
			this.sustain = false;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000B731 File Offset: 0x00009931
		public void StartFadeIn(float fadeInTime)
		{
			if (fadeInTime == 0f)
			{
				this.currentFadeTime = 1f;
			}
			this.fadeInDuration = fadeInTime;
			this.fadeOutDuration = 0f;
			this.sustain = true;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x0000B75F File Offset: 0x0000995F
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x0000B767 File Offset: 0x00009967
		public float ScaleRoughness
		{
			get
			{
				return this.roughMod;
			}
			set
			{
				this.roughMod = value;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000B770 File Offset: 0x00009970
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x0000B778 File Offset: 0x00009978
		public float ScaleMagnitude
		{
			get
			{
				return this.magnMod;
			}
			set
			{
				this.magnMod = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000B781 File Offset: 0x00009981
		public float NormalizedFadeTime
		{
			get
			{
				return this.currentFadeTime;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000B789 File Offset: 0x00009989
		private bool IsShaking
		{
			get
			{
				return this.currentFadeTime > 0f || this.sustain;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000B7A0 File Offset: 0x000099A0
		private bool IsFadingOut
		{
			get
			{
				return !this.sustain && this.currentFadeTime > 0f;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000B7B9 File Offset: 0x000099B9
		private bool IsFadingIn
		{
			get
			{
				return this.currentFadeTime < 1f && this.sustain && this.fadeInDuration > 0f;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000B7DF File Offset: 0x000099DF
		public CameraShakeState CurrentState
		{
			get
			{
				if (this.IsFadingIn)
				{
					return CameraShakeState.FadingIn;
				}
				if (this.IsFadingOut)
				{
					return CameraShakeState.FadingOut;
				}
				if (this.IsShaking)
				{
					return CameraShakeState.Sustained;
				}
				return CameraShakeState.Inactive;
			}
		}

		// Token: 0x040001A5 RID: 421
		public float Magnitude;

		// Token: 0x040001A6 RID: 422
		public float Roughness;

		// Token: 0x040001A7 RID: 423
		public Vector3 PositionInfluence;

		// Token: 0x040001A8 RID: 424
		public Vector3 RotationInfluence;

		// Token: 0x040001A9 RID: 425
		public bool DeleteOnInactive = true;

		// Token: 0x040001AA RID: 426
		private float roughMod = 1f;

		// Token: 0x040001AB RID: 427
		private float magnMod = 1f;

		// Token: 0x040001AC RID: 428
		private float fadeOutDuration;

		// Token: 0x040001AD RID: 429
		private float fadeInDuration;

		// Token: 0x040001AE RID: 430
		private bool sustain;

		// Token: 0x040001AF RID: 431
		private float currentFadeTime;

		// Token: 0x040001B0 RID: 432
		private float tick;

		// Token: 0x040001B1 RID: 433
		private Vector3 amt;
	}
}
