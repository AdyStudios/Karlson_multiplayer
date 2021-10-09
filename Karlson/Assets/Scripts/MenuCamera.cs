using System;
using EZCameraShake;
using UnityEngine;

// Token: 0x02000017 RID: 23
public class MenuCamera : MonoBehaviour
{
	// Token: 0x06000077 RID: 119 RVA: 0x00004114 File Offset: 0x00002314
	private void Start()
	{
		this.startPos = base.transform.position;
		this.desiredPos = this.startPos;
		this.options += this.startPos;
		this.play += this.startPos;
		this.about += this.startPos;
		CameraShaker.Instance.StartShake(1f, 0.04f, 0.1f);
		this.startRot = Vector3.zero;
		this.playRot = new Vector3(0f, 90f, 0f);
		this.aboutRot = new Vector3(-90f, 0f, 0f);
	}

	// Token: 0x06000078 RID: 120 RVA: 0x000041DC File Offset: 0x000023DC
	private void Update()
	{
		base.transform.position = Vector3.SmoothDamp(base.transform.position, this.desiredPos, ref this.posVel, 0.4f);
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, this.desiredRot, Time.deltaTime * 4f);
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00004241 File Offset: 0x00002441
	public void Options()
	{
		this.desiredPos = this.options;
	}

	// Token: 0x0600007A RID: 122 RVA: 0x0000424F File Offset: 0x0000244F
	public void Main()
	{
		this.desiredPos = this.startPos;
		this.desiredRot = Quaternion.Euler(this.startRot);
	}

	// Token: 0x0600007B RID: 123 RVA: 0x0000426E File Offset: 0x0000246E
	public void Play()
	{
		this.desiredPos = this.play;
		this.desiredRot = Quaternion.Euler(this.playRot);
	}

	// Token: 0x0600007C RID: 124 RVA: 0x0000428D File Offset: 0x0000248D
	public void About()
	{
		this.desiredPos = this.about;
		this.desiredRot = Quaternion.Euler(this.aboutRot);
	}

	// Token: 0x0400004C RID: 76
	private Vector3 startPos;

	// Token: 0x0400004D RID: 77
	private Vector3 options = new Vector3(0f, 3.6f, 8f);

	// Token: 0x0400004E RID: 78
	private Vector3 play = new Vector3(1f, 4.6f, 5.5f);

	// Token: 0x0400004F RID: 79
	private Vector3 about = new Vector3(1f, 5.5f, 5.5f);

	// Token: 0x04000050 RID: 80
	private Vector3 desiredPos;

	// Token: 0x04000051 RID: 81
	private Vector3 posVel;

	// Token: 0x04000052 RID: 82
	private Vector3 startRot;

	// Token: 0x04000053 RID: 83
	private Vector3 playRot;

	// Token: 0x04000054 RID: 84
	private Vector3 aboutRot;

	// Token: 0x04000055 RID: 85
	private Quaternion desiredRot;
}
