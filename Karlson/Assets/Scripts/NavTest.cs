using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x0200001C RID: 28
public class NavTest : MonoBehaviour
{
	// Token: 0x060000C0 RID: 192 RVA: 0x000060B2 File Offset: 0x000042B2
	private void Start()
	{
		this.agent = base.GetComponent<NavMeshAgent>();
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x000060C0 File Offset: 0x000042C0
	private void Update()
	{
		if (!PlayerMovement.Instance)
		{
			return;
		}
		Vector3 position = PlayerMovement.Instance.transform.position;
		if (this.agent.isOnNavMesh)
		{
			this.agent.destination = position;
			MonoBehaviour.print("goin");
		}
	}

	// Token: 0x0400009B RID: 155
	private NavMeshAgent agent;
}
