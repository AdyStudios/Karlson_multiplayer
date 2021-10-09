using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000022 RID: 34
public class RagdollController : MonoBehaviour
{
	// Token: 0x06000116 RID: 278 RVA: 0x00008471 File Offset: 0x00006671
	private void Start()
	{
		this.MakeStatic();
	}

	// Token: 0x06000117 RID: 279 RVA: 0x0000847C File Offset: 0x0000667C
	private void LateUpdate()
	{
	}

	// Token: 0x06000118 RID: 280 RVA: 0x0000848C File Offset: 0x0000668C
	public void MakeRagdoll(Vector3 dir)
	{
		if (this.isRagdoll)
		{
			return;
		}
		UnityEngine.Object.Destroy(base.GetComponent<NavMeshAgent>());
		UnityEngine.Object.Destroy(base.GetComponent("NavTest"));
		this.isRagdoll = true;
		UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
		base.GetComponentInChildren<Animator>().enabled = false;
		for (int i = 0; i < this.limbs.Length; i++)
		{
			this.AddRigid(i, dir);
			this.limbs[i].gameObject.layer = LayerMask.NameToLayer("Object");
			this.limbs[i].AddComponent(typeof(global::Object));
		}
	}

	// Token: 0x06000119 RID: 281 RVA: 0x0000852C File Offset: 0x0000672C
	private void AddRigid(int i, Vector3 dir)
	{
		GameObject gameObject = this.limbs[i];
		Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
		rigidbody.mass = this.mass[i];
		rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		rigidbody.AddForce(dir);
		if (i != 0)
		{
			CharacterJoint characterJoint = gameObject.AddComponent<CharacterJoint>();
			characterJoint.autoConfigureConnectedAnchor = true;
			characterJoint.connectedBody = this.FindConnectedBody(i);
			characterJoint.axis = this.axis[i];
			characterJoint.anchor = this.anchor[i];
			characterJoint.swingAxis = this.swingAxis[i];
		}
	}

	// Token: 0x0600011A RID: 282 RVA: 0x000085B8 File Offset: 0x000067B8
	private Rigidbody FindConnectedBody(int i)
	{
		int num = 0;
		if (i == 2)
		{
			num = 1;
		}
		if (i == 4)
		{
			num = 3;
		}
		if (i == 7)
		{
			num = 6;
		}
		if (i == 9)
		{
			num = 8;
		}
		if (i == 10)
		{
			num = 5;
		}
		return this.limbs[num].GetComponent<Rigidbody>();
	}

	// Token: 0x0600011B RID: 283 RVA: 0x000085F4 File Offset: 0x000067F4
	private void MakeStatic()
	{
		int num = this.limbs.Length;
		this.c = new CharacterJoint[num];
		Rigidbody[] array = new Rigidbody[num];
		this.mass = new float[num];
		for (int i = 0; i < this.limbs.Length; i++)
		{
			array[i] = this.limbs[i].GetComponent<Rigidbody>();
			this.mass[i] = array[i].mass;
			this.c[i] = this.limbs[i].GetComponent<CharacterJoint>();
		}
		this.axis = new Vector3[num];
		this.anchor = new Vector3[num];
		this.swingAxis = new Vector3[num];
		for (int j = 0; j < this.c.Length; j++)
		{
			if (!(this.c[j] == null))
			{
				this.axis[j] = this.c[j].axis;
				this.anchor[j] = this.c[j].anchor;
				this.swingAxis[j] = this.c[j].swingAxis;
				UnityEngine.Object.Destroy(this.c[j]);
			}
		}
		Rigidbody[] array2 = array;
		for (int k = 0; k < array2.Length; k++)
		{
			UnityEngine.Object.Destroy(array2[k]);
		}
	}

	// Token: 0x0600011C RID: 284 RVA: 0x00008730 File Offset: 0x00006930
	public bool IsRagdoll()
	{
		return this.isRagdoll;
	}

	// Token: 0x040000FC RID: 252
	private CharacterJoint[] c;

	// Token: 0x040000FD RID: 253
	private Vector3[] axis;

	// Token: 0x040000FE RID: 254
	private Vector3[] anchor;

	// Token: 0x040000FF RID: 255
	private Vector3[] swingAxis;

	// Token: 0x04000100 RID: 256
	public GameObject hips;

	// Token: 0x04000101 RID: 257
	private float[] mass;

	// Token: 0x04000102 RID: 258
	public GameObject[] limbs;

	// Token: 0x04000103 RID: 259
	private bool isRagdoll;

	// Token: 0x04000104 RID: 260
	public Transform leftArm;

	// Token: 0x04000105 RID: 261
	public Transform rightArm;

	// Token: 0x04000106 RID: 262
	public Transform head;

	// Token: 0x04000107 RID: 263
	public Transform hand;

	// Token: 0x04000108 RID: 264
	public Transform hand2;
}
