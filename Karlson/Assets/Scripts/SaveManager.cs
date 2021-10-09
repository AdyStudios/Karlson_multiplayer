using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

// Token: 0x0200002D RID: 45
public class SaveManager : MonoBehaviour
{
	// Token: 0x17000013 RID: 19
	// (get) Token: 0x0600016D RID: 365 RVA: 0x000099D0 File Offset: 0x00007BD0
	// (set) Token: 0x0600016E RID: 366 RVA: 0x000099D7 File Offset: 0x00007BD7
	public static SaveManager Instance { get; set; }

	// Token: 0x0600016F RID: 367 RVA: 0x000099DF File Offset: 0x00007BDF
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		SaveManager.Instance = this;
		this.Load();
	}

	// Token: 0x06000170 RID: 368 RVA: 0x000099F8 File Offset: 0x00007BF8
	public void Save()
	{
		PlayerPrefs.SetString("save", this.Serialize<PlayerSave>(this.state));
	}

	// Token: 0x06000171 RID: 369 RVA: 0x00009A10 File Offset: 0x00007C10
	public void Load()
	{
		if (PlayerPrefs.HasKey("save"))
		{
			this.state = this.Deserialize<PlayerSave>(PlayerPrefs.GetString("save"));
			return;
		}
		this.NewSave();
	}

	// Token: 0x06000172 RID: 370 RVA: 0x00009A3B File Offset: 0x00007C3B
	public void NewSave()
	{
		this.state = new PlayerSave();
		this.Save();
		MonoBehaviour.print("Creating new save file");
	}

	// Token: 0x06000173 RID: 371 RVA: 0x00009A58 File Offset: 0x00007C58
	public string Serialize<T>(T toSerialize)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		StringWriter stringWriter = new StringWriter();
		xmlSerializer.Serialize(stringWriter, toSerialize);
		return stringWriter.ToString();
	}

	// Token: 0x06000174 RID: 372 RVA: 0x00009A8C File Offset: 0x00007C8C
	public T Deserialize<T>(string toDeserialize)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		StringReader textReader = new StringReader(toDeserialize);
		return (T)((object)xmlSerializer.Deserialize(textReader));
	}

	// Token: 0x04000151 RID: 337
	public PlayerSave state;
}
