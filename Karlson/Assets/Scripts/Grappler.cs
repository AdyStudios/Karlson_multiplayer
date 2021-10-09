using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class Grappler : Pickup
{
	// Token: 0x06000053 RID: 83 RVA: 0x0000357C File Offset: 0x0000177C
	private void Start()
	{
		this.tip = base.transform.GetChild(0);
		this.lr = base.GetComponent<LineRenderer>();
		this.lr.positionCount = this.positions;
		this.aim.transform.parent = null;
		this.aim.SetActive(false);
	}

	// Token: 0x06000054 RID: 84 RVA: 0x000035D8 File Offset: 0x000017D8
	public override void Use(Vector3 attackDirection)
	{
		if (this.grappling)
		{
			return;
		}
		this.grappling = true;
		Transform playerCamTransform = PlayerMovement.Instance.GetPlayerCamTransform();
		Transform transform = PlayerMovement.Instance.transform;
		RaycastHit[] array = Physics.RaycastAll(playerCamTransform.position, playerCamTransform.forward, 70f, this.whatIsGround);
		if (array.Length < 1)
		{
			if (this.nearestPoint == Vector3.zero)
			{
				return;
			}
			this.grapplePoint = this.nearestPoint;
		}
		else
		{
			this.grapplePoint = array[0].point;
		}
		this.joint = transform.gameObject.AddComponent<SpringJoint>();
		this.joint.autoConfigureConnectedAnchor = false;
		this.joint.connectedAnchor = this.grapplePoint;
		this.joint.maxDistance = Vector2.Distance(this.grapplePoint, transform.position) * 0.8f;
		this.joint.minDistance = Vector2.Distance(this.grapplePoint, transform.position) * 0.25f;
		this.joint.spring = 4.5f;
		this.joint.damper = 7f;
		this.joint.massScale = 4.5f;
		this.endPoint = this.tip.position;
		this.offsetMultiplier = 2f;
		this.lr.positionCount = this.positions;
		AudioManager.Instance.PlayPitched("Grapple", 0.2f);
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00003760 File Offset: 0x00001960
	public override void OnAim()
	{
		if (this.grappling)
		{
			return;
		}
		Transform playerCamTransform = PlayerMovement.Instance.GetPlayerCamTransform();
		List<RaycastHit> list = Physics.RaycastAll(playerCamTransform.position, playerCamTransform.forward, 70f, this.whatIsGround).ToList<RaycastHit>();
		if (list.Count > 0)
		{
			this.aim.SetActive(false);
			this.aim.transform.localScale = Vector3.zero;
			return;
		}
		int num = 50;
		int num2 = 10;
		float d = 0.035f;
		bool flag = list.Count > 0;
		int num3 = 0;
		while (num3 < num2 && !flag)
		{
			for (int i = 0; i < num; i++)
			{
				float f = 6.2831855f / (float)num * (float)i;
				float d2 = Mathf.Cos(f);
				float d3 = Mathf.Sin(f);
				Vector3 a = playerCamTransform.right * d2 + playerCamTransform.up * d3;
				list.AddRange(Physics.RaycastAll(playerCamTransform.position, playerCamTransform.forward + a * d * (float)num3, 70f, this.whatIsGround));
			}
			if (list.Count > 0)
			{
				break;
			}
			num3++;
		}
		this.nearestPoint = this.FindNearestPoint(list);
		if (list.Count > 0 && !this.grappling)
		{
			this.aim.SetActive(true);
			this.aim.transform.position = Vector3.SmoothDamp(this.aim.transform.position, this.nearestPoint, ref this.aimVel, 0.05f);
			Vector3 target = 0.025f * list[0].distance * Vector3.one;
			this.aim.transform.localScale = Vector3.SmoothDamp(this.aim.transform.localScale, target, ref this.scaleVel, 0.2f);
			return;
		}
		this.aim.SetActive(false);
		this.aim.transform.localScale = Vector3.zero;
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00003980 File Offset: 0x00001B80
	private Vector3 FindNearestPoint(List<RaycastHit> hits)
	{
		Transform playerCamTransform = PlayerMovement.Instance.GetPlayerCamTransform();
		Vector3 result = Vector3.zero;
		float num = float.PositiveInfinity;
		for (int i = 0; i < hits.Count; i++)
		{
			if (hits[i].distance < num)
			{
				num = hits[i].distance;
				result = hits[i].collider.ClosestPoint(playerCamTransform.position + playerCamTransform.forward * num);
			}
		}
		return result;
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00003A07 File Offset: 0x00001C07
	public override void StopUse()
	{
		UnityEngine.Object.Destroy(this.joint);
		this.grapplePoint = Vector3.zero;
		this.grappling = false;
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00003A26 File Offset: 0x00001C26
	private void LateUpdate()
	{
		this.DrawGrapple();
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00003A30 File Offset: 0x00001C30
	private void DrawGrapple()
	{
		if (this.grapplePoint == Vector3.zero || this.joint == null)
		{
			this.lr.positionCount = 0;
			return;
		}
		this.endPoint = Vector3.Lerp(this.endPoint, this.grapplePoint, Time.deltaTime * 15f);
		this.offsetMultiplier = Mathf.SmoothDamp(this.offsetMultiplier, 0f, ref this.offsetVel, 0.1f);
		Vector3 position = this.tip.position;
		float num = Vector3.Distance(this.endPoint, position);
		this.lr.SetPosition(0, position);
		this.lr.SetPosition(this.positions - 1, this.endPoint);
		float num2 = num;
		float num3 = 1f;
		for (int i = 1; i < this.positions - 1; i++)
		{
			float num4 = (float)i / (float)this.positions;
			float num5 = num4 * this.offsetMultiplier;
			float num6 = (Mathf.Sin(num5 * num2) - 0.5f) * num3 * (num5 * 2f);
			Vector3 normalized = (this.endPoint - position).normalized;
			float num7 = Mathf.Sin(num4 * 180f * 0.017453292f);
			float num8 = Mathf.Cos(this.offsetMultiplier * 90f * 0.017453292f);
			//Vector3 position2 = position + (this.endPoint - position) / (float)this.positions * (float)i + (num8 * num6 * Vector2.Perpendicular(normalized) + this.offsetMultiplier * num7 * Vector3.down);
			Vector3 position2 = new Vector3(0,0,0);
			this.lr.SetPosition(i, position2);
		}
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00003BEB File Offset: 0x00001DEB
	public Vector3 GetGrapplePoint()
	{
		return this.grapplePoint;
	}

	// Token: 0x0400002B RID: 43
	private Transform tip;

	// Token: 0x0400002C RID: 44
	private bool grappling;

	// Token: 0x0400002D RID: 45
	public LayerMask whatIsGround;

	// Token: 0x0400002E RID: 46
	private Vector3 grapplePoint;

	// Token: 0x0400002F RID: 47
	private SpringJoint joint;

	// Token: 0x04000030 RID: 48
	private LineRenderer lr;

	// Token: 0x04000031 RID: 49
	private Vector3 endPoint;

	// Token: 0x04000032 RID: 50
	private float offsetMultiplier;

	// Token: 0x04000033 RID: 51
	private float offsetVel;

	// Token: 0x04000034 RID: 52
	public GameObject aim;

	// Token: 0x04000035 RID: 53
	private int positions = 100;

	// Token: 0x04000036 RID: 54
	private Vector3 aimVel;

	// Token: 0x04000037 RID: 55
	private Vector3 scaleVel;

	// Token: 0x04000038 RID: 56
	private Vector3 nearestPoint;
}
