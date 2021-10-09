using System;
using EZCameraShake;
using UnityEngine;

// Token: 0x0200000B RID: 11
public class ShakeOnTrigger : MonoBehaviour
{
	// Token: 0x06000027 RID: 39 RVA: 0x00002C3C File Offset: 0x00000E3C
	private void Start()
	{
		this._shakeInstance = CameraShaker.Instance.StartShake(2f, 15f, 2f);
		this._shakeInstance.StartFadeOut(0f);
		this._shakeInstance.DeleteOnInactive = true;
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00002C79 File Offset: 0x00000E79
	private void OnTriggerEnter(Collider c)
	{
		if (c.CompareTag("Player"))
		{
			this._shakeInstance.StartFadeIn(1f);
		}
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00002C98 File Offset: 0x00000E98
	private void OnTriggerExit(Collider c)
	{
		if (c.CompareTag("Player"))
		{
			this._shakeInstance.StartFadeOut(3f);
		}
	}

	// Token: 0x04000013 RID: 19
	private CameraShakeInstance _shakeInstance;
}
