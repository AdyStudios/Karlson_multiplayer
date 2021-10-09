using System;
using UnityEngine;

// Token: 0x02000030 RID: 48
public class Grappling : MonoBehaviour
{
	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000184 RID: 388 RVA: 0x0000A13F File Offset: 0x0000833F
	// (set) Token: 0x06000185 RID: 389 RVA: 0x0000A146 File Offset: 0x00008346
	public static Grappling Instance { get; set; }

	// Token: 0x06000186 RID: 390 RVA: 0x0000A14E File Offset: 0x0000834E
	private void Start()
	{
		Grappling.Instance = this;
		this.lr = base.GetComponentInChildren<LineRenderer>();
		this.lr.positionCount = 0;
	}

	// Token: 0x06000187 RID: 391 RVA: 0x0000A170 File Offset: 0x00008370
	private void Update()
	{
		this.DrawLine();
		if (this.connectedTransform == null)
		{
			return;
		}
		Vector2 vector = (this.connectedTransform.position - base.transform.position).normalized;
		Mathf.Atan2(vector.y, vector.x);
		this.joint = null;
	}

	// Token: 0x06000188 RID: 392 RVA: 0x0000A1DC File Offset: 0x000083DC
	private void DrawLine()
	{
		if (this.connectedTransform == null || this.joint == null)
		{
			this.ClearLine();
			return;
		}
		this.desiredPos = this.connectedTransform.position + this.offsetPoint;
		this.endPoint = Vector3.SmoothDamp(this.endPoint, this.desiredPos, ref this.ropeVel, 0.03f);
		this.offsetMultiplier = Mathf.SmoothDamp(this.offsetMultiplier, 0f, ref this.offsetVel, 0.12f);
		int num = 100;
		this.lr.positionCount = num;
		Vector3 position = base.transform.position;
		this.lr.SetPosition(0, position);
		this.lr.SetPosition(num - 1, this.endPoint);
		float num2 = 15f;
		float num3 = 0.5f;
		for (int i = 1; i < num - 1; i++)
		{
			float num4 = (float)i / (float)num;
			float num5 = num4 * this.offsetMultiplier;
			float num6 = (Mathf.Sin(num5 * num2) - 0.5f) * num3 * (num5 * 2f);
			Vector3 normalized = (this.endPoint - position).normalized;
			float num7 = Mathf.Sin(num4 * 180f * 0.017453292f);
			float num8 = Mathf.Cos(this.offsetMultiplier * 90f * 0.017453292f);
			//Vector3 position2 = position + (this.endPoint - position) / (float)num * (float)i + (num8 * num6 * Vector2.Perpendicular(normalized) + this.offsetMultiplier * num7 * Vector2.down);
			Vector3 position2 = new Vector3(0, 0, 0);
			this.lr.SetPosition(i, position2);
		}
	}

	// Token: 0x06000189 RID: 393 RVA: 0x0000A3A4 File Offset: 0x000085A4
	private void ClearLine()
	{
		this.lr.positionCount = 0;
	}

	// Token: 0x0600018A RID: 394 RVA: 0x0000A3B2 File Offset: 0x000085B2
	public void Use(Vector3 attackDirection)
	{
		if (!this.readyToUse)
		{
			return;
		}
		this.ShootRope(attackDirection);
		this.readyToUse = false;
	}

	// Token: 0x0600018B RID: 395 RVA: 0x0000A3CB File Offset: 0x000085CB
	public void StopUse()
	{
		if (this.joint == null)
		{
			return;
		}
		MonoBehaviour.print("STOPPING");
		this.connectedTransform = null;
		this.readyToUse = true;
	}

	// Token: 0x0600018C RID: 396 RVA: 0x0000A3F4 File Offset: 0x000085F4
	private void ShootRope(Vector3 dir)
	{
		RaycastHit[] array = Physics.RaycastAll(base.transform.position, dir, 10f, this.whatIsSickoMode);
		GameObject gameObject = null;
		RaycastHit raycastHit = default(RaycastHit);
		foreach (RaycastHit raycastHit2 in array)
		{
			gameObject = raycastHit2.collider.gameObject;
			if (gameObject.layer != LayerMask.NameToLayer("Player"))
			{
				raycastHit = raycastHit2;
				break;
			}
			gameObject = null;
		}
		if (gameObject == null || raycastHit.collider == null)
		{
			return;
		}
		this.connectedTransform = raycastHit.collider.transform;
		this.joint = base.gameObject.AddComponent<SpringJoint>();
		UnityEngine.Object component = gameObject.GetComponent<Rigidbody>();
		this.offsetPoint = raycastHit.point - raycastHit.collider.transform.position;
		this.joint.connectedBody = gameObject.GetComponent<Rigidbody>();
		if (component == null)
		{
			this.joint.connectedAnchor = raycastHit.point;
		}
		else
		{
			this.joint.connectedAnchor = this.offsetPoint;
		}
		this.joint.autoConfigureConnectedAnchor = false;
		this.endPoint = base.transform.position;
		this.offsetMultiplier = 1f;
	}

	// Token: 0x0400015E RID: 350
	private LineRenderer lr;

	// Token: 0x0400015F RID: 351
	public LayerMask whatIsSickoMode;

	// Token: 0x04000160 RID: 352
	private Transform connectedTransform;

	// Token: 0x04000161 RID: 353
	private SpringJoint joint;

	// Token: 0x04000162 RID: 354
	private Vector3 offsetPoint;

	// Token: 0x04000163 RID: 355
	private Vector3 endPoint;

	// Token: 0x04000164 RID: 356
	private Vector3 ropeVel;

	// Token: 0x04000165 RID: 357
	private Vector3 desiredPos;

	// Token: 0x04000166 RID: 358
	private float offsetMultiplier;

	// Token: 0x04000167 RID: 359
	private float offsetVel;

	// Token: 0x04000168 RID: 360
	private bool readyToUse = true;
}
