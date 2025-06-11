namespace Craft;

/// <summary>
/// アイテムの問い合わせサービス
/// </summary>
public interface IItemQueryService
{
	#region Methods

	/// <summary>
	/// アイテムを問い合わせします。
	/// </summary>
	/// <param name="itemId">アイテムID</param>
	/// <returns>問い合わせたアイテムを返します。</returns>
	Item QuerySingle(ItemId itemId);

	#endregion
}
