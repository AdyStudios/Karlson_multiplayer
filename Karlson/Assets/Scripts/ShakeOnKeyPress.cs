using System;
using EZCameraShake;
using UnityEngine;

// Token: 0x0200000A RID: 10
public class ShakeOnKeyPress : MonoBehaviour
{
	// Token: 0x06000025 RID: 37 RVA: 0x00002BE3 File Offset: 0x00000DE3
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			CameraShaker.Instance.ShakeOnce(this.Magnitude, this.Roughness, 0f, this.FadeOutTime);
		}
	}

	// Token: 0x04000010 RID: 16
	public float Magnitude = 2f;

	// Token: 0x04000011 RID: 17
	public float Roughness = 10f;

	// Token: 0x04000012 RID: 18
	public float FadeOutTime = 5f;
}
