using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;

// Token: 0x02000033 RID: 51
public class RangedWeapon : Weapon
{
	// Token: 0x0600019F RID: 415 RVA: 0x0000A6F0 File Offset: 0x000088F0
	private new void Start()
	{
		base.Start();
		this.rb = base.GetComponent<Rigidbody>();
		this.guntip = base.transform.GetChild(0);
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x0000A716 File Offset: 0x00008916
	public override void Use(Vector3 attackDirection)
	{
		if (!base.readyToUse || !base.pickedUp)
		{
			return;
		}
		this.SpawnProjectile(attackDirection);
		this.Recoil();
		base.readyToUse = false;
		base.Invoke("GetReady", this.attackSpeed);
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x00003381 File Offset: 0x00001581
	public override void OnAim()
	{
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x00003381 File Offset: 0x00001581
	public override void StopUse()
	{
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x0000A750 File Offset: 0x00008950
	private void SpawnProjectile(Vector3 attackDirection)
	{
		Vector3 vector = this.guntip.position - this.guntip.transform.right / 4f;
		Vector3 normalized = (attackDirection - vector).normalized;
		List<Collider> list = new List<Collider>();
		if (this.player)
		{
			PlayerMovement.Instance.GetRb().AddForce(base.transform.right * this.boostRecoil, ForceMode.Impulse);
		}
		for (int i = 0; i < this.bullets; i++)
		{
			UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.muzzle, vector, Quaternion.identity);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.projectile, vector, base.transform.rotation);
			Rigidbody componentInChildren = gameObject.GetComponentInChildren<Rigidbody>();
			this.projectileColliders = gameObject.GetComponentsInChildren<Collider>();
			this.RemoveCollisionWithPlayer();
			componentInChildren.transform.rotation = base.transform.rotation;
			Vector3 a = normalized + (this.guntip.transform.up * UnityEngine.Random.Range(-this.accuracy, this.accuracy) + this.guntip.transform.forward * UnityEngine.Random.Range(-this.accuracy, this.accuracy));
			componentInChildren.AddForce(componentInChildren.mass * this.force * a);
			Bullet bullet = (Bullet)gameObject.GetComponent(typeof(Bullet));
			if (bullet != null)
			{
				Color col = Color.red;
				if (this.player)
				{
					col = Color.blue;
					Gun.Instance.Shoot();
					if (bullet.explosive)
					{
						UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.thumpAudio, base.transform.position, Quaternion.identity);
					}
					else
					{
						AudioManager.Instance.PlayPitched("GunBass", 0.3f);
						AudioManager.Instance.PlayPitched("GunHigh", 0.3f);
						AudioManager.Instance.PlayPitched("GunLow", 0.3f);
					}
					componentInChildren.AddForce(componentInChildren.mass * this.force * a);
				}
				else
				{
					UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.gunShotAudio, base.transform.position, Quaternion.identity);
				}
				bullet.SetBullet(this.damage, this.pushBackForce, col);
				bullet.player = this.player;
			}
			foreach (Collider collider in list)
			{
				Physics.IgnoreCollision(collider, this.projectileColliders[0]);
			}
			list.Add(this.projectileColliders[0]);
		}
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x0000AA24 File Offset: 0x00008C24
	private void GetReady()
	{
		base.readyToUse = true;
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x0000AA30 File Offset: 0x00008C30
	private void Recoil()
	{
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x0000AA40 File Offset: 0x00008C40
	private void RemoveCollisionWithPlayer()
	{
		Collider[] array;
		if (this.player)
		{
			array = new Collider[]
			{
				PlayerMovement.Instance.GetPlayerCollider()
			};
		}
		else
		{
			array = base.transform.root.GetComponentsInChildren<Collider>();
		}
		for (int i = 0; i < array.Length; i++)
		{
			for (int j = 0; j < this.projectileColliders.Length; j++)
			{
				Physics.IgnoreCollision(array[i], this.projectileColliders[j], true);
			}
		}
	}

	// Token: 0x04000170 RID: 368
	public GameObject projectile;

	// Token: 0x04000171 RID: 369
	public float pushBackForce;

	// Token: 0x04000172 RID: 370
	public float force;

	// Token: 0x04000173 RID: 371
	public float accuracy;

	// Token: 0x04000174 RID: 372
	public int bullets;

	// Token: 0x04000175 RID: 373
	public float boostRecoil;

	// Token: 0x04000176 RID: 374
	private Transform guntip;

	// Token: 0x04000177 RID: 375
	private Rigidbody rb;

	// Token: 0x04000178 RID: 376
	private Collider[] projectileColliders;
}
