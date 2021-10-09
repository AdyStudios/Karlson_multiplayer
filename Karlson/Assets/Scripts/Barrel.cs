using System;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class Barrel : MonoBehaviour
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
		{
			Explosion explosion = (Explosion)UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.explosion, base.transform.position, Quaternion.identity).GetComponentInChildren(typeof(Explosion));
			UnityEngine.Object.Destroy(base.gameObject);
			base.CancelInvoke();
			this.done = true;
			Bullet bullet = (Bullet)other.gameObject.GetComponent(typeof(Bullet));
			if (bullet && bullet.player)
			{
				explosion.player = bullet.player;
			}
		}
	}

	// Token: 0x06000002 RID: 2 RVA: 0x000020FA File Offset: 0x000002FA
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer != LayerMask.NameToLayer("Bullet"))
		{
			return;
		}
		this.done = true;
		base.Invoke("Explode", 0.2f);
	}

	// Token: 0x06000003 RID: 3 RVA: 0x0000212B File Offset: 0x0000032B
	private void Explode()
	{
		UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.explosion, base.transform.position, Quaternion.identity);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04000001 RID: 1
	private bool done;
}
