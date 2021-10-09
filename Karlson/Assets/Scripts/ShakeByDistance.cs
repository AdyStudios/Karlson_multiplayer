using System;
using EZCameraShake;
using UnityEngine;

// Token: 0x02000009 RID: 9
public class ShakeByDistance : MonoBehaviour
{
	// Token: 0x06000022 RID: 34 RVA: 0x00002B63 File Offset: 0x00000D63
	private void Start()
	{
		this._shakeInstance = CameraShaker.Instance.StartShake(2f, 14f, 0f);
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00002B84 File Offset: 0x00000D84
	private void Update()
	{
		float num = Vector3.Distance(this.Player.transform.position, base.transform.position);
		this._shakeInstance.ScaleMagnitude = 1f - Mathf.Clamp01(num / this.Distance);
	}

	// Token: 0x0400000D RID: 13
	public GameObject Player;

	// Token: 0x0400000E RID: 14
	public float Distance = 10f;

	// Token: 0x0400000F RID: 15
	private CameraShakeInstance _shakeInstance;
}
