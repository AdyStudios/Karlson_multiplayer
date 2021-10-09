using System;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class Gun : MonoBehaviour
{
	// Token: 0x17000003 RID: 3
	// (get) Token: 0x0600005C RID: 92 RVA: 0x00003C03 File Offset: 0x00001E03
	// (set) Token: 0x0600005D RID: 93 RVA: 0x00003C0A File Offset: 0x00001E0A
	public static Gun Instance { get; set; }

	// Token: 0x0600005E RID: 94 RVA: 0x00003C12 File Offset: 0x00001E12
	private void Start()
	{
		Gun.Instance = this;
		this.defaultPos = base.transform.localPosition;
		this.rb = PlayerMovement.Instance.GetRb();
	}

	// Token: 0x0600005F RID: 95 RVA: 0x00003C3C File Offset: 0x00001E3C
	private void Update()
	{
		if (PlayerMovement.Instance && !PlayerMovement.Instance.HasGun())
		{
			return;
		}
		this.MoveGun();
		Vector3 grapplePoint = PlayerMovement.Instance.GetGrapplePoint();
		Quaternion b = Quaternion.LookRotation((PlayerMovement.Instance.GetGrapplePoint() - base.transform.position).normalized);
		this.rotationOffset += Mathf.DeltaAngle(base.transform.parent.rotation.eulerAngles.y, this.prevRotation.y);
		if (this.rotationOffset > 90f)
		{
			this.rotationOffset = 90f;
		}
		if (this.rotationOffset < -90f)
		{
			this.rotationOffset = -90f;
		}
		this.rotationOffset = Mathf.SmoothDamp(this.rotationOffset, 0f, ref this.rotVelY, 0.025f);
		Vector3 b2 = new Vector3(0f, this.rotationOffset, this.rotationOffset);
		if (grapplePoint == Vector3.zero)
		{
			b = Quaternion.Euler(base.transform.parent.rotation.eulerAngles - b2);
		}
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * this.speed);
		Vector3 vector = PlayerMovement.Instance.FindVelRelativeToLook() * this.posOffset;
		float num = PlayerMovement.Instance.GetFallSpeed() * this.posOffset;
		if (num < -0.08f)
		{
			num = -0.08f;
		}
		Vector3 a = this.defaultPos - new Vector3(vector.x, num, vector.y);
		base.transform.localPosition = Vector3.SmoothDamp(base.transform.localPosition, a + this.desiredBob, ref this.posVel, this.posSpeed);
		this.prevRotation = base.transform.parent.rotation.eulerAngles;
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00003E48 File Offset: 0x00002048
	private void MoveGun()
	{
		if (!this.rb || !PlayerMovement.Instance.grounded)
		{
			return;
		}
		if (Mathf.Abs(this.rb.velocity.magnitude) < 4f)
		{
			this.desiredBob = Vector3.zero;
			return;
		}
		float x = Mathf.PingPong(Time.time * this.bobSpeed, this.xBob) - this.xBob / 2f;
		float y = Mathf.PingPong(Time.time * this.bobSpeed, this.yBob) - this.yBob / 2f;
		float z = Mathf.PingPong(Time.time * this.bobSpeed, this.zBob) - this.zBob / 2f;
		this.desiredBob = new Vector3(x, y, z);
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00003F18 File Offset: 0x00002118
	public void Shoot()
	{
		float recoil = PlayerMovement.Instance.GetRecoil();
		Vector3 b = (Vector3.up / 4f + Vector3.back / 1.5f) * recoil;
		base.transform.localPosition = base.transform.localPosition + b;
		Quaternion localRotation = Quaternion.Euler(-60f * recoil, 0f, 0f);
		base.transform.localRotation = localRotation;
	}

	// Token: 0x04000039 RID: 57
	private Vector3 rotationVel;

	// Token: 0x0400003A RID: 58
	private float speed = 8f;

	// Token: 0x0400003B RID: 59
	private float posSpeed = 0.075f;

	// Token: 0x0400003C RID: 60
	private float posOffset = 0.004f;

	// Token: 0x0400003D RID: 61
	private Vector3 defaultPos;

	// Token: 0x0400003E RID: 62
	private Vector3 posVel;

	// Token: 0x0400003F RID: 63
	private Rigidbody rb;

	// Token: 0x04000041 RID: 65
	private float rotationOffset;

	// Token: 0x04000042 RID: 66
	private float rotationOffsetZ;

	// Token: 0x04000043 RID: 67
	private float rotVelY;

	// Token: 0x04000044 RID: 68
	private float rotVelZ;

	// Token: 0x04000045 RID: 69
	private Vector3 prevRotation;

	// Token: 0x04000046 RID: 70
	private Vector3 desiredBob;

	// Token: 0x04000047 RID: 71
	private float xBob = 0.12f;

	// Token: 0x04000048 RID: 72
	private float yBob = 0.08f;

	// Token: 0x04000049 RID: 73
	private float zBob = 0.1f;

	// Token: 0x0400004A RID: 74
	private float bobSpeed = 0.45f;
}
