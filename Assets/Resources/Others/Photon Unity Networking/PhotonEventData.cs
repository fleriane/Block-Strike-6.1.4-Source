using System;
using ExitGames.Client.Photon;

// Token: 0x0200053C RID: 1340
public struct PhotonEventData
{
	// Token: 0x06002BCD RID: 11213 RVA: 0x000DC9E8 File Offset: 0x000DABE8
	public PhotonEventData(Hashtable rpcEvent, int sender)
	{
		if (rpcEvent.ContainsKey(1))
		{
			this.tag = (PhotonEventTag)((byte)rpcEvent[1]);
		}
		else
		{
			this.tag = PhotonEventTag.None;
		}
		if (rpcEvent.ContainsKey(2))
		{
			this.id = (byte)rpcEvent[2];
		}
		else
		{
			this.id = 0;
		}
		if (rpcEvent.ContainsKey(3))
		{
			uint num = (uint)((int)rpcEvent[3]);
			double num2 = num;
			this.timestamp = num2 / 1000.0;
		}
		else
		{
			this.timestamp = -1.0;
		}
		if (rpcEvent.ContainsKey(4))
		{
			this.parameters = (object[])rpcEvent[4];
		}
		else
		{
			this.parameters = new object[0];
		}
		this.senderID = sender;
	}

	// Token: 0x04001BE9 RID: 7145
	public readonly byte id;

	// Token: 0x04001BEA RID: 7146
	public readonly PhotonEventTag tag;

	// Token: 0x04001BEB RID: 7147
	public readonly object[] parameters;

	// Token: 0x04001BEC RID: 7148
	public readonly int senderID;

	// Token: 0x04001BED RID: 7149
	public readonly double timestamp;
}
