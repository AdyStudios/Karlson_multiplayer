using System;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class Break : MonoBehaviour
{
	// Token: 0x06000007 RID: 7 RVA: 0x00002180 File Offset: 0x00000380
	private void OnCollisionEnter(Collision other)
	{
		if (this.done)
		{
			return;
		}
		if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			return;
		}
		Rigidbody component = other.gameObject.GetComponent<Rigidbody>();
		if (!component)
		{
			return;
		}
		if (component.velocity.magnitude > 18f)
		{
			if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
			{
				if (!PlayerMovement.Instance.IsCrouching())
				{
					return;
				}
				PlayerMovement.Instance.Slowmo(0.35f, 0.8f);
				this.BreakDoor(component);
			}
			this.BreakDoor(component);
		}
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00002220 File Offset: 0x00000420
	private void BreakDoor(Rigidbody rb)
	{
		Vector3 a = rb.velocity;
		float magnitude = a.magnitude;
		if (magnitude > 20f)
		{
			float d = magnitude / 20f;
			a /= d;
		}
		Rigidbody[] componentsInChildren = UnityEngine.Object.Instantiate<GameObject>(this.replace, base.transform.position, base.transform.rotation).GetComponentsInChildren<Rigidbody>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].velocity = a * 1.5f;
		}
		UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.destructionAudio, base.transform.position, Quaternion.identity);
		UnityEngine.Object.Destroy(base.gameObject);
		this.done = true;
	}

	// Token: 0x04000002 RID: 2
	public GameObject replace;

	// Token: 0x04000003 RID: 3
	private bool done;
}
