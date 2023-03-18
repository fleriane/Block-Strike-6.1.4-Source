using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000320 RID: 800
public class ConsoleManager : MonoBehaviour
{
	// Token: 0x06001D91 RID: 7569 RVA: 0x0001532A File Offset: 0x0001352A
	private void Start()
	{
		ConsoleManager.autoErrorShow = GameConsole.Load<bool>("auto_error_show", false);
		Application.RegisterLogCallback(new Application.LogCallback(this.OnLogCallback));
	}

	// Token: 0x06001D92 RID: 7570 RVA: 0x0001534D File Offset: 0x0001354D
	private void OnEnable()
	{
		ConsoleManager.isCreated = true;
	}

	// Token: 0x06001D93 RID: 7571 RVA: 0x00015355 File Offset: 0x00013555
	private void OnDisable()
	{
		ConsoleManager.isCreated = false;
	}

	// Token: 0x06001D94 RID: 7572 RVA: 0x000B8584 File Offset: 0x000B6784
	public static void Log(string message)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("<color=grey>Log:</color> " + message);
		ConsoleManager.list.Add(stringBuilder.ToString());
	}

	// Token: 0x06001D95 RID: 7573 RVA: 0x000B85BC File Offset: 0x000B67BC
	public static void LogWarning(string message)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("<color=yellow>Warning:</color> " + message);
		ConsoleManager.list.Add(stringBuilder.ToString());
	}

	// Token: 0x06001D96 RID: 7574 RVA: 0x000B85F4 File Offset: 0x000B67F4
	public static void LogError(string message)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("<color=red>Error:</color> " + message);
		ConsoleManager.list.Add(stringBuilder.ToString());
	}

	// Token: 0x06001D97 RID: 7575 RVA: 0x000B862C File Offset: 0x000B682C
	private void OnLogCallback(string message, string stackTrace, LogType type)
	{
		StringBuilder stringBuilder = new StringBuilder();
		switch (type)
		{
		case LogType.Error:
			stringBuilder.AppendLine("<color=red>Error:</color> " + message);
			if (ConsoleManager.autoErrorShow)
			{
				UIToast.Show(Localization.Get("Error", true));
			}
			break;
		case LogType.Assert:
			stringBuilder.AppendLine("<color=red>Assert:</color> " + message);
			if (ConsoleManager.autoErrorShow)
			{
				UIToast.Show(Localization.Get("Error", true));
			}
			break;
		case LogType.Warning:
			stringBuilder.AppendLine("<color=yellow>Warning:</color> " + message);
			break;
		case LogType.Log:
			stringBuilder.AppendLine("<color=grey>Log:</color> " + message);
			break;
		case LogType.Exception:
			stringBuilder.AppendLine("<color=red>Exception:</color> " + message);
			if (ConsoleManager.autoErrorShow)
			{
				UIToast.Show(Localization.Get("Error", true));
			}
			break;
		}
		stringBuilder.Append("<color=grey>" + stackTrace + "</color>");
		ConsoleManager.list.Add(stringBuilder.ToString());
		if (ConsoleManager.LogCallback != null)
		{
			ConsoleManager.LogCallback(message, stackTrace, type);
		}
	}

	// Token: 0x04001192 RID: 4498
	private static bool autoErrorShow = false;

	// Token: 0x04001193 RID: 4499
	public static bool isCreated;

	// Token: 0x04001194 RID: 4500
	public static Action<string, string, LogType> LogCallback;

	// Token: 0x04001195 RID: 4501
	public static List<string> list = new List<string>();
}
