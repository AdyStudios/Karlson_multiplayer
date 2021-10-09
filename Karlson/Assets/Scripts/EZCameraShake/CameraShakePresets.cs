using System;
using UnityEngine;

namespace EZCameraShake
{
	// Token: 0x0200003D RID: 61
	public static class CameraShakePresets
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000B800 File Offset: 0x00009A00
		public static CameraShakeInstance Bump
		{
			get
			{
				return new CameraShakeInstance(2.5f, 4f, 0.1f, 0.75f)
				{
					PositionInfluence = Vector3.one * 0.15f,
					RotationInfluence = Vector3.one
				};
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000B83C File Offset: 0x00009A3C
		public static CameraShakeInstance Explosion
		{
			get
			{
				return new CameraShakeInstance(5f, 10f, 0f, 1.5f)
				{
					PositionInfluence = Vector3.one * 0.25f,
					RotationInfluence = new Vector3(4f, 1f, 1f)
				};
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060001EC RID: 492 RVA: 0x0000B894 File Offset: 0x00009A94
		public static CameraShakeInstance Earthquake
		{
			get
			{
				return new CameraShakeInstance(0.6f, 3.5f, 2f, 10f)
				{
					PositionInfluence = Vector3.one * 0.25f,
					RotationInfluence = new Vector3(1f, 1f, 4f)
				};
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000B8EC File Offset: 0x00009AEC
		public static CameraShakeInstance BadTrip
		{
			get
			{
				return new CameraShakeInstance(10f, 0.15f, 5f, 10f)
				{
					PositionInfluence = new Vector3(0f, 0f, 0.15f),
					RotationInfluence = new Vector3(2f, 1f, 4f)
				};
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060001EE RID: 494 RVA: 0x0000B946 File Offset: 0x00009B46
		public static CameraShakeInstance HandheldCamera
		{
			get
			{
				return new CameraShakeInstance(1f, 0.25f, 5f, 10f)
				{
					PositionInfluence = Vector3.zero,
					RotationInfluence = new Vector3(1f, 0.5f, 0.5f)
				};
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060001EF RID: 495 RVA: 0x0000B988 File Offset: 0x00009B88
		public static CameraShakeInstance Vibration
		{
			get
			{
				return new CameraShakeInstance(0.4f, 20f, 2f, 2f)
				{
					PositionInfluence = new Vector3(0f, 0.15f, 0f),
					RotationInfluence = new Vector3(1.25f, 0f, 4f)
				};
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x0000B9E2 File Offset: 0x00009BE2
		public static CameraShakeInstance RoughDriving
		{
			get
			{
				return new CameraShakeInstance(1f, 2f, 1f, 1f)
				{
					PositionInfluence = Vector3.zero,
					RotationInfluence = Vector3.one
				};
			}
		}
	}
}
