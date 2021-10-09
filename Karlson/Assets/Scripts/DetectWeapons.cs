using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class DetectWeapons : MonoBehaviour
{
	// Token: 0x06000136 RID: 310 RVA: 0x00008D24 File Offset: 0x00006F24
	public void Pickup()
	{
		if (this.hasGun || !this.HasWeapons())
		{
			return;
		}
		this.gun = this.GetWeapon();
		this.gunScript = (Pickup)this.gun.GetComponent(typeof(Pickup));
		if (this.gunScript.pickedUp)
		{
			this.gun = null;
			this.gunScript = null;
			return;
		}
		UnityEngine.Object.Destroy(this.gun.GetComponent<Rigidbody>());
		this.scale = this.gun.transform.localScale;
		this.gun.transform.parent = this.weaponPos;
		this.gun.transform.localScale = this.scale;
		this.hasGun = true;
		this.gunScript.PickupWeapon(true);
		AudioManager.Instance.Play("GunPickup");
		this.gun.layer = LayerMask.NameToLayer("Equipable");
	}

	// Token: 0x06000137 RID: 311 RVA: 0x00008E13 File Offset: 0x00007013
	public void Shoot(Vector3 dir)
	{
		if (!this.hasGun)
		{
			return;
		}
		this.gunScript.Use(dir);
	}

	// Token: 0x06000138 RID: 312 RVA: 0x00008E2A File Offset: 0x0000702A
	public void StopUse()
	{
		if (!this.hasGun)
		{
			return;
		}
		this.gunScript.StopUse();
	}

	// Token: 0x06000139 RID: 313 RVA: 0x00008E40 File Offset: 0x00007040
	public void Throw(Vector3 throwDir)
	{
		if (!this.hasGun)
		{
			return;
		}
		if (this.gun.GetComponent<Rigidbody>())
		{
			return;
		}
		this.gunScript.StopUse();
		this.hasGun = false;
		Rigidbody rigidbody = this.gun.AddComponent<Rigidbody>();
		rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		rigidbody.maxAngularVelocity = 20f;
		rigidbody.AddForce(throwDir * this.throwForce);
		rigidbody.AddRelativeTorque(new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f) * 0.4f), ForceMode.Impulse);
		this.gun.layer = LayerMask.NameToLayer("Gun");
		this.gunScript.Drop();
		this.gun.transform.parent = null;
		this.gun.transform.localScale = this.scale;
		this.gun = null;
		this.gunScript = null;
	}

	// Token: 0x0600013A RID: 314 RVA: 0x00008F3D File Offset: 0x0000713D
	public void Fire(Vector3 dir)
	{
		this.gunScript.Use(dir);
	}

	// Token: 0x0600013B RID: 315 RVA: 0x00008F4C File Offset: 0x0000714C
	private void Update()
	{
		if (!this.hasGun)
		{
			return;
		}
		this.gun.transform.localRotation = Quaternion.Slerp(this.gun.transform.localRotation, this.desiredRot, Time.deltaTime * this.speed);
		this.gun.transform.localPosition = Vector3.SmoothDamp(this.gun.transform.localPosition, this.desiredPos, ref this.posVel, 1f / this.speed);
		this.gunScript.OnAim();
	}

	// Token: 0x0600013C RID: 316 RVA: 0x00008FE1 File Offset: 0x000071E1
	private void Start()
	{
		this.weapons = new List<GameObject>();
	}

	// Token: 0x0600013D RID: 317 RVA: 0x00008FEE File Offset: 0x000071EE
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Gun") && !this.weapons.Contains(other.gameObject))
		{
			this.weapons.Add(other.gameObject);
		}
	}

	// Token: 0x0600013E RID: 318 RVA: 0x00009021 File Offset: 0x00007221
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Gun") && this.weapons.Contains(other.gameObject))
		{
			this.weapons.Remove(other.gameObject);
		}
	}

	// Token: 0x0600013F RID: 319 RVA: 0x00009058 File Offset: 0x00007258
	public GameObject GetWeapon()
	{
		if (this.weapons.Count == 1)
		{
			return this.weapons[0];
		}
		GameObject result = null;
		float num = float.PositiveInfinity;
		foreach (GameObject gameObject in this.weapons)
		{
			float num2 = Vector3.Distance(base.transform.position, gameObject.transform.position);
			if (num2 < num)
			{
				num = num2;
				result = gameObject;
			}
		}
		return result;
	}

	// Token: 0x06000140 RID: 320 RVA: 0x000090F0 File Offset: 0x000072F0
	public void ForcePickup(GameObject gun)
	{
		this.gunScript = (Pickup)gun.GetComponent(typeof(Pickup));
		this.gun = gun;
		if (this.gunScript.pickedUp)
		{
			gun = null;
			this.gunScript = null;
			return;
		}
		UnityEngine.Object.Destroy(gun.GetComponent<Rigidbody>());
		this.scale = gun.transform.localScale;
		gun.transform.parent = this.weaponPos;
		gun.transform.localScale = this.scale;
		this.hasGun = true;
		this.gunScript.PickupWeapon(true);
		gun.layer = LayerMask.NameToLayer("Equipable");
	}

	// Token: 0x06000141 RID: 321 RVA: 0x00009198 File Offset: 0x00007398
	public float GetRecoil()
	{
		return this.gunScript.recoil;
	}

	// Token: 0x06000142 RID: 322 RVA: 0x000091A5 File Offset: 0x000073A5
	public bool HasWeapons()
	{
		return this.weapons.Count > 0;
	}

	// Token: 0x06000143 RID: 323 RVA: 0x000091B5 File Offset: 0x000073B5
	public bool IsGrappler()
	{
		return this.gun && this.gun.GetComponent(typeof(Grappler));
	}

	// Token: 0x06000144 RID: 324 RVA: 0x000091E0 File Offset: 0x000073E0
	public Vector3 GetGrapplerPoint()
	{
		if (this.IsGrappler())
		{
			return ((Grappler)this.gun.GetComponent(typeof(Grappler))).GetGrapplePoint();
		}
		return Vector3.zero;
	}

	// Token: 0x06000145 RID: 325 RVA: 0x0000920F File Offset: 0x0000740F
	public Pickup GetWeaponScript()
	{
		return this.gunScript;
	}

	// Token: 0x06000146 RID: 326 RVA: 0x00009217 File Offset: 0x00007417
	public bool HasGun()
	{
		return this.hasGun;
	}

	// Token: 0x04000128 RID: 296
	public Transform weaponPos;

	// Token: 0x04000129 RID: 297
	private List<GameObject> weapons;

	// Token: 0x0400012A RID: 298
	private bool hasGun;

	// Token: 0x0400012B RID: 299
	private GameObject gun;

	// Token: 0x0400012C RID: 300
	private Pickup gunScript;

	// Token: 0x0400012D RID: 301
	private float speed = 10f;

	// Token: 0x0400012E RID: 302
	private Quaternion desiredRot = Quaternion.Euler(0f, 90f, 0f);

	// Token: 0x0400012F RID: 303
	private Vector3 desiredPos = Vector3.zero;

	// Token: 0x04000130 RID: 304
	private Vector3 posVel;

	// Token: 0x04000131 RID: 305
	private float throwForce = 1000f;

	// Token: 0x04000132 RID: 306
	private Vector3 scale;
}
