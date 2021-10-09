using System;
using UnityEngine;

// Token: 0x02000032 RID: 50
public abstract class Pickup : MonoBehaviour, IPickup
{
	// Token: 0x17000015 RID: 21
	// (get) Token: 0x06000190 RID: 400 RVA: 0x0000A546 File Offset: 0x00008746
	// (set) Token: 0x06000191 RID: 401 RVA: 0x0000A54E File Offset: 0x0000874E
	public bool pickedUp { get; set; }

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x06000192 RID: 402 RVA: 0x0000A557 File Offset: 0x00008757
	// (set) Token: 0x06000193 RID: 403 RVA: 0x0000A55F File Offset: 0x0000875F
	public bool readyToUse { get; set; }

	// Token: 0x06000194 RID: 404 RVA: 0x0000A568 File Offset: 0x00008768
	private void Awake()
	{
		this.readyToUse = true;
		this.outline = base.transform.GetChild(1);
	}

	// Token: 0x06000195 RID: 405 RVA: 0x0000A583 File Offset: 0x00008783
	private void Update()
	{
		bool pickedUp = this.pickedUp;
	}

	// Token: 0x06000196 RID: 406 RVA: 0x0000A58C File Offset: 0x0000878C
	public void PickupWeapon(bool player)
	{
		this.pickedUp = true;
		this.player = player;
		this.outline.gameObject.SetActive(false);
	}

	// Token: 0x06000197 RID: 407 RVA: 0x0000A5AD File Offset: 0x000087AD
	public void Drop()
	{
		this.readyToUse = true;
		base.Invoke("DropWeapon", 0.5f);
		this.thrown = true;
	}

	// Token: 0x06000198 RID: 408 RVA: 0x0000A5CD File Offset: 0x000087CD
	private void DropWeapon()
	{
		base.CancelInvoke();
		this.pickedUp = false;
		this.outline.gameObject.SetActive(true);
	}

	// Token: 0x06000199 RID: 409
	public abstract void Use(Vector3 attackDirection);

	// Token: 0x0600019A RID: 410
	public abstract void OnAim();

	// Token: 0x0600019B RID: 411
	public abstract void StopUse();

	// Token: 0x0600019C RID: 412 RVA: 0x0000A5ED File Offset: 0x000087ED
	public bool IsPickedUp()
	{
		return this.pickedUp;
	}

	// Token: 0x0600019D RID: 413 RVA: 0x0000A5F8 File Offset: 0x000087F8
	private void OnCollisionEnter(Collision other)
	{
		if (!this.thrown)
		{
			return;
		}
		if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		{
			UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.enemyHitAudio, other.contacts[0].point, Quaternion.identity);
			((RagdollController)other.transform.root.GetComponent(typeof(RagdollController))).MakeRagdoll(-base.transform.right * 60f);
			Rigidbody component = other.gameObject.GetComponent<Rigidbody>();
			if (component)
			{
				component.AddForce(-base.transform.right * 1500f);
			}
			((Enemy)other.transform.root.GetComponent(typeof(Enemy))).DropGun(Vector3.up);
		}
		this.thrown = false;
	}

	// Token: 0x0400016C RID: 364
	protected bool player;

	// Token: 0x0400016D RID: 365
	private bool thrown;

	// Token: 0x0400016E RID: 366
	public float recoil;

	// Token: 0x0400016F RID: 367
	private Transform outline;
}
