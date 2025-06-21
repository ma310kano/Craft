namespace Craft;

/// <summary>
/// アイテム拾得イベントの引数
/// </summary>
public class ItemPickedUpEventArgs(ItemId itemId, Quantity quantity) : EventArgs
{
	#region Properties

	/// <summary>
	/// アイテムIDを取得します。
	/// </summary>
	public ItemId ItemId { get; } = itemId;

	/// <summary>
	/// 数量を取得します。
	/// </summary>
	public Quantity Quantity { get; } = quantity;

	#endregion
}
