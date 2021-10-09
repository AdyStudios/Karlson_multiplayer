using System;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class MoveCamera : MonoBehaviour
{
	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000081 RID: 129 RVA: 0x0000438A File Offset: 0x0000258A
	// (set) Token: 0x06000082 RID: 130 RVA: 0x00004391 File Offset: 0x00002591
	public static MoveCamera Instance { get; private set; }

	// Token: 0x06000083 RID: 131 RVA: 0x0000439C File Offset: 0x0000259C
	private void Start()
	{
		MoveCamera.Instance = this;
		this.cam = base.transform.GetChild(0).GetComponent<Camera>();
		this.cam.fieldOfView = GameState.Instance.fov;
		this.offset = base.transform.position - this.player.transform.position;
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00004401 File Offset: 0x00002601
	private void Update()
	{
		base.transform.position = this.player.transform.position;
	}

	// Token: 0x06000085 RID: 133 RVA: 0x0000441E File Offset: 0x0000261E
	public void UpdateFov()
	{
		this.cam.fieldOfView = GameState.Instance.fov;
	}

	// Token: 0x04000056 RID: 86
	public Transform player;

	// Token: 0x04000057 RID: 87
	private Vector3 offset;

	// Token: 0x04000058 RID: 88
	private Camera cam;
}
