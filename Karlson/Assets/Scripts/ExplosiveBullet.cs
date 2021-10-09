using System;
using UnityEngine;

// Token: 0x02000008 RID: 8
public class ExplosiveBullet : MonoBehaviour
{
	// Token: 0x0600001E RID: 30 RVA: 0x00002AE2 File Offset: 0x00000CE2
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
		UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.thumpAudio, base.transform.position, Quaternion.identity);
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002B10 File Offset: 0x00000D10
	private void OnCollisionEnter(Collision other)
	{
		UnityEngine.Object.Destroy(base.gameObject);
		UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.explosion, base.transform.position, Quaternion.identity);
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002B3D File Offset: 0x00000D3D
	private void Update()
	{
		this.rb.AddForce(Vector3.up * Time.deltaTime * 1000f);
	}

	// Token: 0x0400000C RID: 12
	private Rigidbody rb;
}
