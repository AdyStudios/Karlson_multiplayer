using System;
using UnityEngine;

// Token: 0x02000038 RID: 56
public class UIManger : MonoBehaviour
{
	// Token: 0x1700001A RID: 26
	// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000AEBB File Offset: 0x000090BB
	// (set) Token: 0x060001C2 RID: 450 RVA: 0x0000AEC2 File Offset: 0x000090C2
	public static UIManger Instance { get; private set; }

	// Token: 0x060001C3 RID: 451 RVA: 0x0000AECA File Offset: 0x000090CA
	private void Awake()
	{
		UIManger.Instance = this;
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x0000AED2 File Offset: 0x000090D2
	private void Start()
	{
		this.gameUI.SetActive(false);
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x0000AEE0 File Offset: 0x000090E0
	public void StartGame()
	{
		this.gameUI.SetActive(true);
		this.DeadUI(false);
		this.WinUI(false);
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x0000AEFC File Offset: 0x000090FC
	public void GameUI(bool b)
	{
		this.gameUI.SetActive(b);
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x0000AF0A File Offset: 0x0000910A
	public void DeadUI(bool b)
	{
		this.deadUI.SetActive(b);
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x0000AF18 File Offset: 0x00009118
	public void WinUI(bool b)
	{
		this.winUI.SetActive(b);
		MonoBehaviour.print("setting win UI");
	}

	// Token: 0x0400018B RID: 395
	public GameObject gameUI;

	// Token: 0x0400018C RID: 396
	public GameObject deadUI;

	// Token: 0x0400018D RID: 397
	public GameObject winUI;
}
