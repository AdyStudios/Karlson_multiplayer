using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000029 RID: 41
public class Enemy : MonoBehaviour
{
	// Token: 0x06000148 RID: 328 RVA: 0x0000926E File Offset: 0x0000746E
	private void Start()
	{
		this.ragdoll = (RagdollController)base.GetComponent(typeof(RagdollController));
		this.animator = base.GetComponentInChildren<Animator>();
		this.agent = base.GetComponent<NavMeshAgent>();
		this.GiveGun();
	}

	// Token: 0x06000149 RID: 329 RVA: 0x000092A9 File Offset: 0x000074A9
	private void LateUpdate()
	{
		this.FindPlayer();
		this.Aim();
	}

	// Token: 0x0600014A RID: 330 RVA: 0x000092B8 File Offset: 0x000074B8
	private void Aim()
	{
		if (this.currentGun == null)
		{
			return;
		}
		if (this.ragdoll.IsRagdoll())
		{
			return;
		}
		if (!this.animator.GetBool("Aiming"))
		{
			return;
		}
		Vector3 vector = this.target.transform.position - base.transform.position;
		if (Vector3.Angle(base.transform.forward, vector) > 70f)
		{
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(vector), Time.deltaTime * this.hipSpeed);
		}
		this.head.transform.rotation = Quaternion.Slerp(this.head.transform.rotation, Quaternion.LookRotation(vector), Time.deltaTime * this.headAndHandSpeed);
		this.rightArm.transform.rotation = Quaternion.Slerp(this.head.transform.rotation, Quaternion.LookRotation(vector), Time.deltaTime * this.headAndHandSpeed);
		this.leftArm.transform.rotation = Quaternion.Slerp(this.head.transform.rotation, Quaternion.LookRotation(vector), Time.deltaTime * this.headAndHandSpeed);
		if (this.readyToShoot)
		{
			this.gunScript.Use(this.target.position);
			this.readyToShoot = false;
			base.Invoke("Cooldown", this.attackSpeed + UnityEngine.Random.Range(this.attackSpeed, this.attackSpeed * 5f));
		}
	}

	// Token: 0x0600014B RID: 331 RVA: 0x00009450 File Offset: 0x00007650
	private void FindPlayer()
	{
		this.FindTarget();
		if (!this.agent || !this.target)
		{
			return;
		}
		Vector3 normalized = (this.target.position - base.transform.position).normalized;
		RaycastHit[] array = Physics.RaycastAll(base.transform.position + normalized, normalized, (float)this.objectsAndPlayer);
		if (array.Length < 1)
		{
			return;
		}
		bool flag = false;
		float num = 1001f;
		float num2 = 1000f;
		for (int i = 0; i < array.Length; i++)
		{
			int layer = array[i].collider.gameObject.layer;
			if (!(array[i].collider.transform.root.gameObject.name == base.gameObject.name) && layer != LayerMask.NameToLayer("TransparentFX"))
			{
				if (layer == LayerMask.NameToLayer("Player"))
				{
					num = array[i].distance;
					flag = true;
				}
				else if (array[i].distance < num2)
				{
					num2 = array[i].distance;
				}
			}
		}
		if (!flag)
		{
			return;
		}
		if (num2 < num && num != 1001f)
		{
			this.readyToShoot = false;
			if (this.animator.GetBool("Running") && this.agent.remainingDistance < 0.2f)
			{
				this.animator.SetBool("Running", false);
				this.spottedPlayer = false;
			}
			if (!this.spottedPlayer || !this.agent.isOnNavMesh || this.animator.GetBool("Running"))
			{
				return;
			}
			MonoBehaviour.print("oof");
			this.takingAim = false;
			this.agent.destination = this.target.transform.position;
			this.animator.SetBool("Running", true);
			this.animator.SetBool("Aiming", false);
			this.readyToShoot = false;
			return;
		}
		else
		{
			if (this.takingAim || this.animator.GetBool("Aiming"))
			{
				return;
			}
			if (!this.spottedPlayer)
			{
				this.spottedPlayer = true;
			}
			base.Invoke("TakeAim", UnityEngine.Random.Range(0.3f, 1f));
			this.takingAim = true;
			return;
		}
	}

	// Token: 0x0600014C RID: 332 RVA: 0x000096B4 File Offset: 0x000078B4
	private void TakeAim()
	{
		this.animator.SetBool("Running", false);
		this.animator.SetBool("Aiming", true);
		base.CancelInvoke();
		base.Invoke("Cooldown", UnityEngine.Random.Range(0.3f, 1f));
		if (this.agent && this.agent.isOnNavMesh)
		{
			this.agent.destination = base.transform.position;
		}
	}

	// Token: 0x0600014D RID: 333 RVA: 0x00009734 File Offset: 0x00007934
	private void GiveGun()
	{
		if (this.startGun == null)
		{
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.startGun);
		UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
		this.gunScript = (Weapon)gameObject.GetComponent(typeof(Weapon));
		this.gunScript.PickupWeapon(false);
		gameObject.transform.parent = this.gunPosition;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		this.currentGun = gameObject;
		this.attackSpeed = this.gunScript.GetAttackSpeed();
	}

	// Token: 0x0600014E RID: 334 RVA: 0x000097D7 File Offset: 0x000079D7
	private void Cooldown()
	{
		this.readyToShoot = true;
	}

	// Token: 0x0600014F RID: 335 RVA: 0x000097E0 File Offset: 0x000079E0
	private void FindTarget()
	{
		if (this.target != null)
		{
			return;
		}
		if (!PlayerMovement.Instance)
		{
			return;
		}
		this.target = PlayerMovement.Instance.playerCam;
	}

	// Token: 0x06000150 RID: 336 RVA: 0x00009810 File Offset: 0x00007A10
	public void DropGun(Vector3 dir)
	{
		if (this.gunScript == null)
		{
			return;
		}
		this.gunScript.Drop();
		Rigidbody rigidbody = this.currentGun.AddComponent<Rigidbody>();
		rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		this.currentGun.transform.parent = null;
		rigidbody.AddForce(dir, ForceMode.Impulse);
		float d = 10f;
		rigidbody.AddTorque(new Vector3((float)UnityEngine.Random.Range(-1, 1), (float)UnityEngine.Random.Range(-1, 1), (float)UnityEngine.Random.Range(-1, 1)) * d);
		this.gunScript = null;
	}

	// Token: 0x06000151 RID: 337 RVA: 0x00009897 File Offset: 0x00007A97
	public bool IsDead()
	{
		return this.ragdoll.IsRagdoll();
	}

	// Token: 0x04000133 RID: 307
	private float hipSpeed = 3f;

	// Token: 0x04000134 RID: 308
	private float headAndHandSpeed = 4f;

	// Token: 0x04000135 RID: 309
	private Transform target;

	// Token: 0x04000136 RID: 310
	public LayerMask objectsAndPlayer;

	// Token: 0x04000137 RID: 311
	private NavMeshAgent agent;

	// Token: 0x04000138 RID: 312
	private bool spottedPlayer;

	// Token: 0x04000139 RID: 313
	private Animator animator;

	// Token: 0x0400013A RID: 314
	public GameObject startGun;

	// Token: 0x0400013B RID: 315
	public Transform gunPosition;

	// Token: 0x0400013C RID: 316
	private Weapon gunScript;

	// Token: 0x0400013D RID: 317
	public GameObject currentGun;

	// Token: 0x0400013E RID: 318
	private float attackSpeed;

	// Token: 0x0400013F RID: 319
	private bool readyToShoot;

	// Token: 0x04000140 RID: 320
	private RagdollController ragdoll;

	// Token: 0x04000141 RID: 321
	public Transform leftArm;

	// Token: 0x04000142 RID: 322
	public Transform rightArm;

	// Token: 0x04000143 RID: 323
	public Transform head;

	// Token: 0x04000144 RID: 324
	public Transform hips;

	// Token: 0x04000145 RID: 325
	public Transform player;

	// Token: 0x04000146 RID: 326
	private bool takingAim;
}
