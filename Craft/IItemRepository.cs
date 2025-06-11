namespace Craft;

/// <summary>
/// アイテムのリポジトリー
/// </summary>
public interface IItemRepository
{
	#region Methods

	/// <summary>
	/// アイテムを検索します。
	/// </summary>
	/// <param name="itemId">アイテムID</param>
	/// <returns>検索したアイテムを返します。</returns>
	Item Find(ItemId itemId);

	#endregion
}
