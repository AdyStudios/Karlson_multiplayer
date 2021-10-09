using System;
using UnityEngine;

namespace Audio
{
	// Token: 0x0200003A RID: 58
	[Serializable]
	public class Sound
	{
		// Token: 0x04000199 RID: 409
		public string name;

		// Token: 0x0400019A RID: 410
		public AudioClip clip;

		// Token: 0x0400019B RID: 411
		[Range(0f, 2f)]
		public float volume;

		// Token: 0x0400019C RID: 412
		[Range(0f, 2f)]
		public float pitch;

		// Token: 0x0400019D RID: 413
		public bool loop;

		// Token: 0x0400019E RID: 414
		public bool bypass;

		// Token: 0x0400019F RID: 415
		[HideInInspector]
		public AudioSource source;
	}
}
