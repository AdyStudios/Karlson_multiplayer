using System;
using Audio;
using UnityEngine;

// Token: 0x0200002F RID: 47
public class Bullet : MonoBehaviour
{
	// Token: 0x0600017D RID: 381 RVA: 0x00009DDB File Offset: 0x00007FDB
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x0600017E RID: 382 RVA: 0x00009DEC File Offset: 0x00007FEC
	private void OnCollisionEnter(Collision other)
	{
		if (this.done)
		{
			return;
		}
		this.done = true;
		if (this.explosive)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			((Explosion)UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.explosion, other.contacts[0].point, Quaternion.identity).GetComponentInChildren(typeof(Explosion))).player = this.player;
			return;
		}
		this.BulletExplosion(other.contacts[0]);
		UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.bulletHitAudio, other.contacts[0].point, Quaternion.identity);
		int layer = other.gameObject.layer;
		if (layer == LayerMask.NameToLayer("Player"))
		{
			this.HitPlayer(other.gameObject);
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (layer == LayerMask.NameToLayer("Enemy"))
		{
			if (this.col == Color.blue)
			{
				AudioManager.Instance.Play("Hitmarker");
				MonoBehaviour.print("HITMARKER");
			}
			UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.enemyHitAudio, other.contacts[0].point, Quaternion.identity);
			((RagdollController)other.transform.root.GetComponent(typeof(RagdollController))).MakeRagdoll(-base.transform.right * 350f);
			if (other.gameObject.GetComponent<Rigidbody>())
			{
				other.gameObject.GetComponent<Rigidbody>().AddForce(-base.transform.right * 1500f);
			}
			((Enemy)other.transform.root.GetComponent(typeof(Enemy))).DropGun(Vector3.up);
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (layer == LayerMask.NameToLayer("Bullet"))
		{
			if (other.gameObject.name == base.gameObject.name)
			{
				return;
			}
			UnityEngine.Object.Destroy(base.gameObject);
			UnityEngine.Object.Destroy(other.gameObject);
			this.BulletExplosion(other.contacts[0]);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600017F RID: 383 RVA: 0x0000A038 File Offset: 0x00008238
	private void HitPlayer(GameObject other)
	{
		PlayerMovement.Instance.KillPlayer();
	}

	// Token: 0x06000180 RID: 384 RVA: 0x0000A044 File Offset: 0x00008244
	private void Update()
	{
		if (!this.explosive)
		{
			return;
		}
		this.rb.AddForce(Vector3.up * Time.deltaTime * 1000f);
	}

	// Token: 0x06000181 RID: 385 RVA: 0x0000A074 File Offset: 0x00008274
	private void BulletExplosion(ContactPoint contact)
	{
		Vector3 point = contact.point;
		Vector3 normal = contact.normal;
		ParticleSystem component = UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.bulletDestroy, point + normal * 0.05f, Quaternion.identity).GetComponent<ParticleSystem>();
		component.transform.rotation = Quaternion.LookRotation(normal);
		component.startColor = Color.blue;
	}

	// Token: 0x06000182 RID: 386 RVA: 0x0000A0D8 File Offset: 0x000082D8
	public void SetBullet(float damage, float push, Color col)
	{
		this.damage = damage;
		this.push = push;
		this.col = col;
		if (this.changeCol)
		{
			SpriteRenderer[] componentsInChildren = base.GetComponentsInChildren<SpriteRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].color = col;
			}
		}
		TrailRenderer componentInChildren = base.GetComponentInChildren<TrailRenderer>();
		if (componentInChildren == null)
		{
			return;
		}
		componentInChildren.startColor = col;
		componentInChildren.endColor = col;
	}

	// Token: 0x04000155 RID: 341
	public bool changeCol;

	// Token: 0x04000156 RID: 342
	public bool player;

	// Token: 0x04000157 RID: 343
	private float damage;

	// Token: 0x04000158 RID: 344
	private float push;

	// Token: 0x04000159 RID: 345
	private bool done;

	// Token: 0x0400015A RID: 346
	private Color col;

	// Token: 0x0400015B RID: 347
	public bool explosive;

	// Token: 0x0400015C RID: 348
	private GameObject limbHit;

	// Token: 0x0400015D RID: 349
	private Rigidbody rb;
}
