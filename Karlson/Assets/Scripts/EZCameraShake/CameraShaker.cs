using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZCameraShake
{
	// Token: 0x0200003E RID: 62
	[AddComponentMenu("EZ Camera Shake/Camera Shaker")]
	public class CameraShaker : MonoBehaviour
	{
		// Token: 0x060001F1 RID: 497 RVA: 0x0000BA13 File Offset: 0x00009C13
		private void Awake()
		{
			CameraShaker.Instance = this;
			CameraShaker.instanceList.Add(base.gameObject.name, this);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000BA34 File Offset: 0x00009C34
		private void Update()
		{
			this.posAddShake = Vector3.zero;
			this.rotAddShake = Vector3.zero;
			int num = 0;
			while (num < this.cameraShakeInstances.Count && num < this.cameraShakeInstances.Count)
			{
				CameraShakeInstance cameraShakeInstance = this.cameraShakeInstances[num];
				if (cameraShakeInstance.CurrentState == CameraShakeState.Inactive && cameraShakeInstance.DeleteOnInactive)
				{
					this.cameraShakeInstances.RemoveAt(num);
					num--;
				}
				else if (cameraShakeInstance.CurrentState != CameraShakeState.Inactive)
				{
					this.posAddShake += CameraUtilities.MultiplyVectors(cameraShakeInstance.UpdateShake(), cameraShakeInstance.PositionInfluence);
					this.rotAddShake += CameraUtilities.MultiplyVectors(cameraShakeInstance.UpdateShake(), cameraShakeInstance.RotationInfluence);
				}
				num++;
			}
			base.transform.localPosition = this.posAddShake;
			base.transform.localEulerAngles = this.rotAddShake;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000BB24 File Offset: 0x00009D24
		public static CameraShaker GetInstance(string name)
		{
			CameraShaker result;
			if (CameraShaker.instanceList.TryGetValue(name, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000BB43 File Offset: 0x00009D43
		public CameraShakeInstance Shake(CameraShakeInstance shake)
		{
			this.cameraShakeInstances.Add(shake);
			return shake;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000BB54 File Offset: 0x00009D54
		public CameraShakeInstance ShakeOnce(float magnitude, float roughness, float fadeInTime, float fadeOutTime)
		{
			if (!GameState.Instance)
			{
				return null;
			}
			if (!GameState.Instance.shake)
			{
				return null;
			}
			CameraShakeInstance cameraShakeInstance = new CameraShakeInstance(magnitude, roughness, fadeInTime, fadeOutTime);
			cameraShakeInstance.PositionInfluence = this.DefaultPosInfluence;
			cameraShakeInstance.RotationInfluence = this.DefaultRotInfluence;
			this.cameraShakeInstances.Add(cameraShakeInstance);
			return cameraShakeInstance;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000BBB0 File Offset: 0x00009DB0
		public CameraShakeInstance ShakeOnce(float magnitude, float roughness, float fadeInTime, float fadeOutTime, Vector3 posInfluence, Vector3 rotInfluence)
		{
			if (!GameState.Instance.shake)
			{
				return null;
			}
			CameraShakeInstance cameraShakeInstance = new CameraShakeInstance(magnitude, roughness, fadeInTime, fadeOutTime);
			cameraShakeInstance.PositionInfluence = posInfluence;
			cameraShakeInstance.RotationInfluence = rotInfluence;
			this.cameraShakeInstances.Add(cameraShakeInstance);
			return cameraShakeInstance;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000BBF4 File Offset: 0x00009DF4
		public CameraShakeInstance StartShake(float magnitude, float roughness, float fadeInTime)
		{
			if (!GameState.Instance.shake)
			{
				return null;
			}
			CameraShakeInstance cameraShakeInstance = new CameraShakeInstance(magnitude, roughness);
			cameraShakeInstance.PositionInfluence = this.DefaultPosInfluence;
			cameraShakeInstance.RotationInfluence = this.DefaultRotInfluence;
			cameraShakeInstance.StartFadeIn(fadeInTime);
			this.cameraShakeInstances.Add(cameraShakeInstance);
			return cameraShakeInstance;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000BC44 File Offset: 0x00009E44
		public CameraShakeInstance StartShake(float magnitude, float roughness, float fadeInTime, Vector3 posInfluence, Vector3 rotInfluence)
		{
			CameraShakeInstance cameraShakeInstance = new CameraShakeInstance(magnitude, roughness);
			cameraShakeInstance.PositionInfluence = posInfluence;
			cameraShakeInstance.RotationInfluence = rotInfluence;
			cameraShakeInstance.StartFadeIn(fadeInTime);
			this.cameraShakeInstances.Add(cameraShakeInstance);
			return cameraShakeInstance;
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000BC7D File Offset: 0x00009E7D
		public List<CameraShakeInstance> ShakeInstances
		{
			get
			{
				return new List<CameraShakeInstance>(this.cameraShakeInstances);
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000BC8A File Offset: 0x00009E8A
		private void OnDestroy()
		{
			CameraShaker.instanceList.Remove(base.gameObject.name);
		}

		// Token: 0x040001B2 RID: 434
		public static CameraShaker Instance;

		// Token: 0x040001B3 RID: 435
		private static Dictionary<string, CameraShaker> instanceList = new Dictionary<string, CameraShaker>();

		// Token: 0x040001B4 RID: 436
		public Vector3 DefaultPosInfluence = new Vector3(0.15f, 0.15f, 0.15f);

		// Token: 0x040001B5 RID: 437
		public Vector3 DefaultRotInfluence = new Vector3(1f, 1f, 1f);

		// Token: 0x040001B6 RID: 438
		private Vector3 posAddShake;

		// Token: 0x040001B7 RID: 439
		private Vector3 rotAddShake;

		// Token: 0x040001B8 RID: 440
		private List<CameraShakeInstance> cameraShakeInstances = new List<CameraShakeInstance>();
	}
}
