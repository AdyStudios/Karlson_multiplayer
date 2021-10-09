using System;

// Token: 0x0200002C RID: 44
public class PlayerSave
{
	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600015A RID: 346 RVA: 0x000098CC File Offset: 0x00007ACC
	// (set) Token: 0x0600015B RID: 347 RVA: 0x000098D4 File Offset: 0x00007AD4
	public bool cameraShake { get; set; } = true;

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x0600015C RID: 348 RVA: 0x000098DD File Offset: 0x00007ADD
	// (set) Token: 0x0600015D RID: 349 RVA: 0x000098E5 File Offset: 0x00007AE5
	public bool motionBlur { get; set; } = true;

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x0600015E RID: 350 RVA: 0x000098EE File Offset: 0x00007AEE
	// (set) Token: 0x0600015F RID: 351 RVA: 0x000098F6 File Offset: 0x00007AF6
	public bool slowmo { get; set; } = true;

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000160 RID: 352 RVA: 0x000098FF File Offset: 0x00007AFF
	// (set) Token: 0x06000161 RID: 353 RVA: 0x00009907 File Offset: 0x00007B07
	public bool graphics { get; set; } = true;

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000162 RID: 354 RVA: 0x00009910 File Offset: 0x00007B10
	// (set) Token: 0x06000163 RID: 355 RVA: 0x00009918 File Offset: 0x00007B18
	public bool muted { get; set; }

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000164 RID: 356 RVA: 0x00009921 File Offset: 0x00007B21
	// (set) Token: 0x06000165 RID: 357 RVA: 0x00009929 File Offset: 0x00007B29
	public float sensitivity { get; set; } = 1f;

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000166 RID: 358 RVA: 0x00009932 File Offset: 0x00007B32
	// (set) Token: 0x06000167 RID: 359 RVA: 0x0000993A File Offset: 0x00007B3A
	public float fov { get; set; } = 80f;

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000168 RID: 360 RVA: 0x00009943 File Offset: 0x00007B43
	// (set) Token: 0x06000169 RID: 361 RVA: 0x0000994B File Offset: 0x00007B4B
	public float volume { get; set; } = 0.75f;

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x0600016A RID: 362 RVA: 0x00009954 File Offset: 0x00007B54
	// (set) Token: 0x0600016B RID: 363 RVA: 0x0000995C File Offset: 0x00007B5C
	public float music { get; set; } = 0.5f;

	// Token: 0x04000147 RID: 327
	public float[] times = new float[100];
}
