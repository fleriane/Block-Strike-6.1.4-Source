using System;

// Token: 0x02000521 RID: 1313
public class ConsoleAttribute : Attribute
{
	// Token: 0x06002D18 RID: 11544 RVA: 0x0001F61C File Offset: 0x0001D81C
	public ConsoleAttribute(params string[] value)
	{
		this.commands = value;
	}

	// Token: 0x04001CAC RID: 7340
	public string[] commands;
}
