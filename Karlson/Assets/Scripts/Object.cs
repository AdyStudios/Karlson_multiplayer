using System;
using UnityEngine;

// Token: 0x0200001D RID: 29
public class Object : MonoBehaviour
{
	// Token: 0x060000C3 RID: 195 RVA: 0x00006110 File Offset: 0x00004310
	private void OnCollisionEnter(Collision other)
	{
		float num = other.relativeVelocity.magnitude * 0.025f;
		if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && this.hitReady && num > 0.8f)
		{
			this.hitReady = false;
			Vector3 normalized = base.GetComponent<Rigidbody>().velocity.normalized;
			UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.enemyHitAudio, other.contacts[0].point, Quaternion.identity);
			((RagdollController)other.transform.root.GetComponent(typeof(RagdollController))).MakeRagdoll(normalized * 350f);
			Rigidbody component = other.gameObject.GetComponent<Rigidbody>();
			if (component)
			{
				component.AddForce(normalized * 1100f);
			}
			((Enemy)other.transform.root.GetComponent(typeof(Enemy))).DropGun(Vector3.up);
		}
		if (!this.ready)
		{
			return;
		}
		this.ready = false;
		AudioSource component2 = UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.objectImpactAudio, base.transform.position, Quaternion.identity).GetComponent<AudioSource>();
		Rigidbody component3 = base.GetComponent<Rigidbody>();
		float num2 = 1f;
		if (component3)
		{
			num2 = component3.mass;
		}
		if (num2 < 0.3f)
		{
			num2 = 0.5f;
		}
		if (num2 > 1f)
		{
			num2 = 1f;
		}
		float volume = component2.volume;
		if (num > 1f)
		{
			num = 1f;
		}
		component2.volume = num * num2;
		base.Invoke("GetReady", 0.1f);
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x000062BF File Offset: 0x000044BF
	private void GetReady()
	{
		this.ready = true;
	}

	// Token: 0x0400009C RID: 156
	private bool ready = true;

	// Token: 0x0400009D RID: 157
	private bool hitReady = true;
}
