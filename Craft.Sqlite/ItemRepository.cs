using Dapper;
using System.Data;

namespace Craft.Sqlite;

/// <summary>
/// アイテムのリポジトリー
/// </summary>
/// <param name="connection">コネクション</param>
/// <param name="languageCode">言語コード</param>
public class ItemRepository(IDbConnection connection, string languageCode) : IItemRepository
{
	#region Methods

	/// <summary>
	/// アイテムを検索します。
	/// </summary>
	/// <param name="itemId">アイテムID</param>
	/// <returns>検索したアイテムを返します。</returns>
	public Item Find(ItemId itemId)
	{
		Item result;
		{
			const string sql = @"SELECT
	  ite.item_id
	, inm.item_name
FROM
	items ite
	INNER JOIN item_names inm
		ON  ite.item_id = inm.item_id
		AND inm.language_code = :language_code
WHERE
	ite.item_id = :item_id";

			var param = new
			{
				item_id = itemId.Value,
				language_code = languageCode,
			};

			ItemRecord record = connection.QuerySingle<ItemRecord>(sql, param);

			ItemId resItemId = new(record.ItemId);
			ItemName resItemName = new(record.ItemName);

			result = new Item(resItemId, resItemName);
		}

		return result;
	}

	#endregion

	#region Nested types

	/// <summary>
	/// アイテムのレコード
	/// </summary>
	/// <param name="ItemId">アイテムID</param>
	/// <param name="ItemName">アイテム名</param>
	private record class ItemRecord(string ItemId, string ItemName);

	#endregion
}
