namespace Craft;

/// <summary>
/// アイテム物質除去イベントの引数
/// </summary>
public class ItemMatterRemovedEventArgs(ItemMatter itemMatter) : EventArgs
{
	#region Properties

	/// <summary>
	/// アイテム物質を取得します。
	/// </summary>
	public ItemMatter ItemMatter { get; } = itemMatter;

	#endregion
}
