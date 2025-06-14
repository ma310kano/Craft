namespace Craft;

/// <summary>
/// インベントリー
/// </summary>
public interface IInventory
{
	#region Methods

	/// <summary>
	/// アイテムを追加します。
	/// </summary>
	/// <param name="itemId">アイテムID</param>
	/// <param name="quantity">数量</param>
	void AddItem(ItemId itemId, Quantity quantity);

	/// <summary>
	/// アイテムを除去します。
	/// </summary>
	/// <param name="itemId">アイテムID</param>
	/// <param name="quantity">数量</param>
	ItemMatter RemoveItem(ItemId itemId, Quantity quantity);

	#endregion
}
