using System.Data;

namespace Craft.Sqlite;

/// <summary>
/// アイテム物質のファクトリー
/// </summary>
/// <param name="connection">コネクション</param>
/// <param name="languageCode">言語コード</param>
public class ItemMatterFactory(IDbConnection connection, string languageCode) : IItemMatterFactory
{
	#region Fields

	/// <summary>
	/// アイテムのリポジトリー
	/// </summary>
	private readonly ItemRepository _itemRepository = new(connection, languageCode);

	#endregion

	#region Methods

	/// <summary>
	/// アイテム物質を作成します。
	/// </summary>
	/// <param name="itemId">アイテムID</param>
	/// <param name="quantity">数量</param>
	/// <returns>作成したアイテム物質を返します。</returns>
	public ItemMatter Create(ItemId itemId, Quantity quantity)
	{
		ItemMatter product;
		{
			ItemMatterId itemMatterId = ItemMatterId.Create();
			Item item = _itemRepository.Find(itemId);

			product = new ItemMatter(itemMatterId, item, quantity);
		}

		return product;
	}

	#endregion
}
