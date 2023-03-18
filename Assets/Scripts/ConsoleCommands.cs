using System;
using System.Reflection;
using System.Text;
using UnityEngine;

// Token: 0x0200031F RID: 799
public class ConsoleCommands : MonoBehaviour
{
	// Token: 0x06001D7D RID: 7549 RVA: 0x0001529A File Offset: 0x0001349A
	[Console(new string[]
	{
		"game_quit"
	})]
	private static void OnGameQuit()
	{
		if (PhotonNetwork.inRoom)
		{
			PhotonNetwork.LeaveRoom(true);
		}
		Application.Quit();
	}

	// Token: 0x06001D7E RID: 7550 RVA: 0x000B81C0 File Offset: 0x000B63C0
	[Console(new string[]
	{
		"set_player_ragdoll_force"
	})]
	private static void OnPlayerForceRagdoll(int value)
	{
		int num = Mathf.Clamp(value, 0, 2000);
		PlayerSkinRagdoll.force = (float)num;
	}

	// Token: 0x06001D7F RID: 7551 RVA: 0x000152B2 File Offset: 0x000134B2
	[Console(new string[]
	{
		"leave_room"
	})]
	private static void OnLeaveRoom()
	{
		if (PhotonNetwork.inRoom)
		{
			PhotonNetwork.LeaveRoom(true);
		}
	}

	// Token: 0x06001D80 RID: 7552 RVA: 0x000B81E4 File Offset: 0x000B63E4
	[Console(new string[]
	{
		"game_info"
	})]
	private static void OnGameInfo()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(VersionManager.bundleVersion);
		stringBuilder.AppendLine(VersionManager.bundleVersionCode.ToString());
		Debug.Log(stringBuilder.ToString());
	}

	// Token: 0x06001D81 RID: 7553 RVA: 0x000152C5 File Offset: 0x000134C5
	[Console(new string[]
	{
		"texture_quality"
	})]
	private static void OnTextureQuality(int value)
	{
		QualitySettings.masterTextureLimit = Mathf.Clamp(value, 0, 3);
		Debug.Log("Texture Quality: " + value + " [0-3]");
	}

	// Token: 0x06001D82 RID: 7554 RVA: 0x000B8224 File Offset: 0x000B6424
	[Console(new string[]
	{
		"show_all_stats"
	})]
	private static void OnShowAllStats(bool value)
	{
		FieldInfo field = typeof(UIFramesPerSecond).GetField("allStats", BindingFlags.Static | BindingFlags.NonPublic);
		field.SetValue(null, value);
		GameConsole.Save("show_all_stats", value);
	}

	// Token: 0x06001D83 RID: 7555 RVA: 0x000B8260 File Offset: 0x000B6460
	[Console(new string[]
	{
		"auto_show_error"
	})]
	private static void OnAutoShowError(bool value)
	{
		FieldInfo field = typeof(ConsoleManager).GetField("autoErrorShow", BindingFlags.Static | BindingFlags.NonPublic);
		field.SetValue(null, value);
		GameConsole.Save("auto_error_show", value);
	}

	// Token: 0x06001D84 RID: 7556 RVA: 0x000B829C File Offset: 0x000B649C
	[Console(new string[]
	{
		"set_time_day"
	})]
	private static void OnSetTimeDay(int value)
	{
		if (LevelManager.GetSceneName() != "Shooting Range")
		{
			Debug.Log("Only level: Shooting Range");
			return;
		}
		SkyboxManager skyboxManager = UnityEngine.Object.FindObjectOfType<SkyboxManager>();
		skyboxManager.TimeDay = 100f / (float)Mathf.Clamp(value, 0, 100);
		skyboxManager.SkyboxMaterial.mainTextureOffset = new Vector2(skyboxManager.TimeDay, 0f);
	}

	// Token: 0x06001D85 RID: 7557 RVA: 0x000B8300 File Offset: 0x000B6500
	[Console(new string[]
	{
		"show_player_model"
	})]
	private static void OnCustomShowPlayerModel(bool value)
	{
		if (LevelManager.GetSceneName() != "Shooting Range")
		{
			Debug.Log("Only level: Shooting Range");
			return;
		}
		ShootingRangeManager shootingRangeManager = UnityEngine.Object.FindObjectOfType<ShootingRangeManager>();
		shootingRangeManager.SendMessage("ShowPlayerModel", value);
	}

	// Token: 0x06001D86 RID: 7558 RVA: 0x000B8344 File Offset: 0x000B6544
	[Console(new string[]
	{
		"custom_player_head_skin",
		"custom_player_body_skin",
		"custom_player_legs_skin"
	})]
	private static void OnSetCustomPlayerHeadSkins(string value)
	{
		if (LevelManager.GetSceneName() != "Shooting Range")
		{
			Debug.Log("Only level: Shooting Range");
			return;
		}
		ShootingRangeManager shootingRangeManager = UnityEngine.Object.FindObjectOfType<ShootingRangeManager>();
		shootingRangeManager.SendMessage("SetCustomPlayerSkins", value);
	}

	// Token: 0x06001D87 RID: 7559 RVA: 0x000B8384 File Offset: 0x000B6584
	[Console(new string[]
	{
		"custom_weapon_skin"
	})]
	private static void OnCustomWeaponSkin(string value)
	{
		if (LevelManager.GetSceneName() != "Shooting Range")
		{
			Debug.Log("Only level: Shooting Range");
			return;
		}
		if (GameManager.player == null)
		{
			return;
		}
		FPWeaponShooter script = GameManager.player.PlayerWeapon.GetSelectedWeaponData().Script;
		if (script == null)
		{
			return;
		}
		script.SendMessage("UpdateCustomSkin", value);
	}

	// Token: 0x06001D88 RID: 7560 RVA: 0x000B83F0 File Offset: 0x000B65F0
	[Console(new string[]
	{
		"chat_show_duration"
	})]
	private static void OnChatShowDuration(float value)
	{
		value = Mathf.Clamp(value, 1f, 20f);
		FieldInfo field = typeof(UIChat).GetField("textShowDuration", BindingFlags.Static | BindingFlags.NonPublic);
		field.SetValue(null, value);
		GameConsole.Save("chat_show_duration", value);
	}

	// Token: 0x06001D89 RID: 7561 RVA: 0x000B8440 File Offset: 0x000B6640
	[Console(new string[]
	{
		"show_player_name"
	})]
	private static void OnShowPlayerName(bool value)
	{
		FieldInfo field = typeof(UINameManager).GetField("showPlayerName", BindingFlags.Static | BindingFlags.NonPublic);
		field.SetValue(null, value);
		GameConsole.Save("show_player_name", value);
	}

	// Token: 0x06001D8A RID: 7562 RVA: 0x000B847C File Offset: 0x000B667C
	[Console(new string[]
	{
		"weapon_fov"
	})]
	private static void OnWeaponFOV(float value)
	{
		value = Mathf.Clamp(value, -15f, 15f);
		FieldInfo field = typeof(vp_FPWeapon).GetField("customRenderingFieldOfView", BindingFlags.Static | BindingFlags.NonPublic);
		field.SetValue(null, value);
		if (GameManager.player == null)
		{
			return;
		}
		FPWeaponShooter script = GameManager.player.PlayerWeapon.GetSelectedWeaponData().Script;
		if (script == null)
		{
			return;
		}
		script.FPWeapon.SnapZoom();
		GameConsole.Save("weapon_fov", value);
	}

	// Token: 0x06001D8B RID: 7563 RVA: 0x000B850C File Offset: 0x000B670C
	[Console(new string[]
	{
		"fix_touch_look"
	})]
	private static void OnFixTouchLook(bool value)
	{
		FieldInfo field = typeof(InputTouchLook).GetField("fixTouch", BindingFlags.Static | BindingFlags.NonPublic);
		field.SetValue(null, value);
		GameConsole.Save("fix_touch_look", value);
	}

	// Token: 0x06001D8C RID: 7564 RVA: 0x000B8548 File Offset: 0x000B6748
	[Console(new string[]
	{
		"weapon_bob"
	})]
	private static void OnWeaponBob(bool value)
	{
		FieldInfo field = typeof(vp_FPWeapon).GetField("customBob", BindingFlags.Static | BindingFlags.NonPublic);
		field.SetValue(null, value);
		GameConsole.Save("weapon_bob", value);
	}

	// Token: 0x06001D8D RID: 7565 RVA: 0x000152EE File Offset: 0x000134EE
	[Console(new string[]
	{
		"weapon_left_hand"
	})]
	private static void OnWeaponLeftHand(bool value)
	{
		WeaponCameraFlip.OnFlip(value);
		GameConsole.Save("weapon_left_hand", value);
	}

	// Token: 0x06001D8E RID: 7566 RVA: 0x00015301 File Offset: 0x00013501
	[Console(new string[]
	{
		"thread_enable"
	})]
	private static void OnThreadEnable(bool value)
	{
		Debug.Log("Restart the game to make the settings work");
		GameConsole.Save("thread_enable", value);
	}
}
