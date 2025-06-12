namespace Craft;

/// <summary>
/// アイテム物質の作成サービス
/// </summary>
public interface IItemMatterCreationService
{
	#region Methods

	/// <summary>
	/// アイテム物質を作成します。
	/// </summary>
	/// <param name="itemId">アイテムID</param>
	/// <param name="quantity">数量</param>
	/// <returns>作成したアイテム物質を返します。</returns>
	ItemMatter Create(ItemId itemId, Quantity quantity);

	#endregion
}
