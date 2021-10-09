using System;
using UnityEngine;

// Token: 0x0200001F RID: 31
public class PlayerAudio : MonoBehaviour
{
	// Token: 0x060000D1 RID: 209 RVA: 0x000065A1 File Offset: 0x000047A1
	private void Start()
	{
		this.rb = PlayerMovement.Instance.GetRb();
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x000065B4 File Offset: 0x000047B4
	private void Update()
	{
		if (!this.rb)
		{
			return;
		}
		float num = this.rb.velocity.magnitude;
		if (PlayerMovement.Instance.grounded)
		{
			if (num < 20f)
			{
				num = 0f;
			}
			num = (num - 20f) / 30f;
		}
		else
		{
			num = (num - 10f) / 30f;
		}
		if (num > 1f)
		{
			num = 1f;
		}
		num *= 1f;
		this.currentVol = Mathf.SmoothDamp(this.currentVol, num, ref this.volVel, 0.2f);
		if (PlayerMovement.Instance.paused)
		{
			this.currentVol = 0f;
		}
		this.foley.volume = this.currentVol;
		this.wind.volume = this.currentVol * 0.5f;
	}

	// Token: 0x040000AB RID: 171
	private Rigidbody rb;

	// Token: 0x040000AC RID: 172
	public AudioSource wind;

	// Token: 0x040000AD RID: 173
	public AudioSource foley;

	// Token: 0x040000AE RID: 174
	private float currentVol;

	// Token: 0x040000AF RID: 175
	private float volVel;
}
