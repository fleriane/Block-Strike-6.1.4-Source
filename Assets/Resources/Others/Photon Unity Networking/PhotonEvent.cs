using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x0200053E RID: 1342
public static class PhotonEvent
{

	// Token: 0x06002BD2 RID: 11218 RVA: 0x0001DCD0 File Offset: 0x0001BED0
	public static void Clear()
	{
		PhotonEvent.eventList.Clear();
	}

	// Token: 0x06002BD3 RID: 11219 RVA: 0x0001DCDC File Offset: 0x0001BEDC
	public static void RPC(PhotonEventTag tag, PhotonPlayer player, params object[] parameters)
	{
		if (tag != PhotonEventTag.None)
		{
			PhotonEvent.RPC(tag, 0, false, PhotonTargets.Others, new PhotonPlayer[]
			{
				player
			}, parameters);
		}
	}

	// Token: 0x06002BD4 RID: 11220 RVA: 0x0001DCF5 File Offset: 0x0001BEF5
	public static void RPC(PhotonEventTag tag, PhotonPlayer[] players, params object[] parameters)
	{
		if (tag != PhotonEventTag.None)
		{
			PhotonEvent.RPC(tag, 0, false, PhotonTargets.Others, players, parameters);
		}
	}

	// Token: 0x06002BD5 RID: 11221 RVA: 0x0001DD05 File Offset: 0x0001BF05
	public static void RPC(PhotonEventTag tag, bool sendTimestamp, PhotonPlayer player, params object[] parameters)
	{
		if (tag != PhotonEventTag.None)
		{
			PhotonEvent.RPC(tag, 0, sendTimestamp, PhotonTargets.Others, new PhotonPlayer[]
			{
				player
			}, parameters);
		}
	}

	// Token: 0x06002BD6 RID: 11222 RVA: 0x0001DD1E File Offset: 0x0001BF1E
	public static void RPC(PhotonEventTag tag, bool sendTimestamp, PhotonPlayer[] players, params object[] parameters)
	{
		if (tag != PhotonEventTag.None)
		{
			PhotonEvent.RPC(tag, 0, sendTimestamp, PhotonTargets.Others, players, parameters);
		}
	}

	// Token: 0x06002BD7 RID: 11223 RVA: 0x0001DD2E File Offset: 0x0001BF2E
	public static void RPC(PhotonEventTag tag, PhotonTargets target, params object[] parameters)
	{
		if (tag != PhotonEventTag.None)
		{
			PhotonEvent.RPC(tag, 0, false, target, null, parameters);
		}
	}

	// Token: 0x06002BD8 RID: 11224 RVA: 0x0001DD3E File Offset: 0x0001BF3E
	public static void RPC(PhotonEventTag tag, bool sendTimestamp, PhotonTargets target, params object[] parameters)
	{
		if (tag != PhotonEventTag.None)
		{
			PhotonEvent.RPC(tag, 0, sendTimestamp, target, null, parameters);
		}
	}

	// Token: 0x06002BD9 RID: 11225 RVA: 0x0001DD4E File Offset: 0x0001BF4E
	public static void RPC(byte id, PhotonPlayer player, params object[] parameters)
	{
		if (id != 0)
		{
			PhotonEvent.RPC(PhotonEventTag.None, id, false, PhotonTargets.Others, new PhotonPlayer[]
			{
				player
			}, parameters);
		}
	}

	// Token: 0x06002BDA RID: 11226 RVA: 0x0001DD67 File Offset: 0x0001BF67
	public static void RPC(byte id, PhotonPlayer[] players, params object[] parameters)
	{
		if (id != 0)
		{
			PhotonEvent.RPC(PhotonEventTag.None, id, false, PhotonTargets.Others, players, parameters);
		}
	}

	// Token: 0x06002BDB RID: 11227 RVA: 0x0001DD77 File Offset: 0x0001BF77
	public static void RPC(byte id, bool sendTimestamp, PhotonPlayer player, params object[] parameters)
	{
		if (id != 0)
		{
			PhotonEvent.RPC(PhotonEventTag.None, id, sendTimestamp, PhotonTargets.Others, new PhotonPlayer[]
			{
				player
			}, parameters);
		}
	}

	// Token: 0x06002BDC RID: 11228 RVA: 0x0001DD90 File Offset: 0x0001BF90
	public static void RPC(byte id, bool sendTimestamp, PhotonPlayer[] players, params object[] parameters)
	{
		if (id != 0)
		{
			PhotonEvent.RPC(PhotonEventTag.None, id, sendTimestamp, PhotonTargets.Others, players, parameters);
		}
	}

	// Token: 0x06002BDD RID: 11229 RVA: 0x0001DDA0 File Offset: 0x0001BFA0
	public static void RPC(byte id, PhotonTargets target, params object[] parameters)
	{
		if (id != 0)
		{
			PhotonEvent.RPC(PhotonEventTag.None, id, false, target, null, parameters);
		}
	}

	// Token: 0x06002BDE RID: 11230 RVA: 0x0001DDB0 File Offset: 0x0001BFB0
	public static void RPC(byte id, bool sendTimestamp, PhotonTargets target, params object[] parameters)
	{
		if (id != 0)
		{
			PhotonEvent.RPC(PhotonEventTag.None, id, sendTimestamp, target, null, parameters);
		}
	}

	// Token: 0x06002BDF RID: 11231 RVA: 0x000DCB24 File Offset: 0x000DAD24
	private static void RPC(PhotonEventTag tag, byte id, bool sendTimestamp, PhotonTargets target, PhotonPlayer[] players, params object[] parameters)
	{
		Hashtable hashtable = new Hashtable();
		if (tag != PhotonEventTag.None)
		{
			hashtable.Add(1, (byte)tag);
		}
		if (id != 0)
		{
			hashtable.Add(2, id);
		}
		if (sendTimestamp)
		{
			hashtable.Add(3, PhotonNetwork.ServerTimestamp);
		}
		if (parameters != null && parameters.Length > 0)
		{
			hashtable.Add(4, parameters);
		}
		if (players != null)
		{
			if (players.Length == 1)
			{
				if (PhotonNetwork.player.ID == players[0].ID)
				{
					PhotonEvent.Invoke(hashtable, players[0].ID);
				}
				else
				{
					RaiseEventOptions options = new RaiseEventOptions
					{
						TargetActors = new int[]
						{
							players[0].ID
						}
					};
					PhotonNetwork.RaiseEvent(PhotonEvent.code, hashtable, true, options);
				}
			}
			else if (players.Length > 1)
			{
				int[] array = new int[players.Length];
				for (int i = 0; i < players.Length; i++)
				{
					array[i] = players[i].ID;
				}
				RaiseEventOptions options2 = new RaiseEventOptions
				{
					TargetActors = array
				};
				PhotonNetwork.RaiseEvent(PhotonEvent.code, hashtable, true, options2);
			}
			return;
		}
		if (target == PhotonTargets.All)
		{
			PhotonNetwork.RaiseEvent(PhotonEvent.code, hashtable, true, null);
			PhotonEvent.Invoke(hashtable, PhotonNetwork.player.ID);
		}
		else if (target == PhotonTargets.Others)
		{
			PhotonNetwork.RaiseEvent(PhotonEvent.code, hashtable, true, null);
		}
		else if (target == PhotonTargets.AllBuffered)
		{
			RaiseEventOptions options3 = new RaiseEventOptions
			{
				CachingOption = EventCaching.AddToRoomCache
			};
			PhotonNetwork.RaiseEvent(PhotonEvent.code, hashtable, true, options3);
			PhotonEvent.Invoke(hashtable, PhotonNetwork.player.ID);
		}
		else if (target == PhotonTargets.OthersBuffered)
		{
			RaiseEventOptions options4 = new RaiseEventOptions
			{
				CachingOption = EventCaching.AddToRoomCache
			};
			PhotonNetwork.RaiseEvent(PhotonEvent.code, hashtable, true, options4);
		}
		else if (target == PhotonTargets.MasterClient)
		{
			if (PhotonNetwork.isMasterClient)
			{
				PhotonEvent.Invoke(hashtable, PhotonNetwork.player.ID);
			}
			else
			{
				RaiseEventOptions options5 = new RaiseEventOptions
				{
					Receivers = ReceiverGroup.MasterClient
				};
				PhotonNetwork.RaiseEvent(PhotonEvent.code, hashtable, true, options5);
			}
		}
		else if (target == PhotonTargets.AllViaServer)
		{
			RaiseEventOptions options6 = new RaiseEventOptions
			{
				Receivers = ReceiverGroup.All
			};
			PhotonNetwork.RaiseEvent(PhotonEvent.code, hashtable, true, options6);
		}
		else if (target == PhotonTargets.AllBufferedViaServer)
		{
			RaiseEventOptions options7 = new RaiseEventOptions
			{
				Receivers = ReceiverGroup.All,
				CachingOption = EventCaching.AddToRoomCache
			};
			PhotonNetwork.RaiseEvent(PhotonEvent.code, hashtable, true, options7);
		}
		else
		{
			Debug.LogError("Unsupported target enum: " + target);
		}
	}

	// Token: 0x06002BE0 RID: 11232 RVA: 0x0001DDC0 File Offset: 0x0001BFC0
	private static void OnEventHandler(byte eventCode, object content, int senderID)
	{
		if (PhotonEvent.code != eventCode)
		{
			return;
		}
		PhotonEvent.Invoke((Hashtable)content, senderID);
	}

	// Token: 0x06002BE1 RID: 11233 RVA: 0x000DCDA4 File Offset: 0x000DAFA4
	private static void Invoke(Hashtable rpcEvent, int senderID)
	{
		PhotonEventData data = new PhotonEventData(rpcEvent, senderID);
		PhotonEvent.eventList.RemoveAll((PhotonEvent.EventData x) => x.callback == null);
		PhotonEvent.count = PhotonEvent.eventList.Count;
		for (int i = 0; i < PhotonEvent.count; i++)
		{
			if ((PhotonEvent.eventList[i].tag == data.tag && data.tag != PhotonEventTag.None) || (PhotonEvent.eventList[i].id == data.id && data.id > 0))
			{
				PhotonEvent.EventCallback eventCallback = PhotonEvent.eventList[i].callback as PhotonEvent.EventCallback;
				eventCallback(data);
			}
		}
	}

	// Token: 0x04001C01 RID: 7169
	private static byte code = 20;

	// Token: 0x04001C02 RID: 7170
	public static List<PhotonEvent.EventData> eventList = new List<PhotonEvent.EventData>();

	// Token: 0x04001C03 RID: 7171
	private static int count = 0;

	// Token: 0x04001C04 RID: 7172
	private static bool init = false;

	// Token: 0x0200053F RID: 1343
	public struct EventData
	{
		// Token: 0x06002BE3 RID: 11235 RVA: 0x0001DDE3 File Offset: 0x0001BFE3
		public EventData(byte i, PhotonEventTag t, PhotonEvent.EventCallback c)
		{
			this.id = i;
			this.tag = t;
			this.callback = c;
		}

		// Token: 0x04001C06 RID: 7174
		public byte id;

		// Token: 0x04001C07 RID: 7175
		public PhotonEventTag tag;

		// Token: 0x04001C08 RID: 7176
		public Delegate callback;
	}

	// Token: 0x02000540 RID: 1344
	// (Invoke) Token: 0x06002BE5 RID: 11237
	public delegate void EventCallback(PhotonEventData data);
}
