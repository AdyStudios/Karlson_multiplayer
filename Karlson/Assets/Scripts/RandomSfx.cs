using System;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class RandomSfx : MonoBehaviour
{
	// Token: 0x0600011E RID: 286 RVA: 0x00008738 File Offset: 0x00006938
	private void Awake()
	{
		AudioSource component = base.GetComponent<AudioSource>();
		component.clip = this.sounds[UnityEngine.Random.Range(0, this.sounds.Length - 1)];
		component.playOnAwake = true;
		component.pitch = 1f + UnityEngine.Random.Range(-0.3f, 0.1f);
		component.enabled = true;
	}

	// Token: 0x04000109 RID: 265
	public AudioClip[] sounds;
}
