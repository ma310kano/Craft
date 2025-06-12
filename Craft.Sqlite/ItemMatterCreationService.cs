using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Craft.Sqlite;

/// <summary>
/// アイテム物質の作成サービス
/// </summary>
/// <param name="configuration">設定</param>
public class ItemMatterCreationService(IConfiguration configuration) : IItemMatterCreationService
{
	#region Fields

	/// <summary>
	/// 接続文字列
	/// </summary>
	private readonly string _connectionString = configuration.GetConnectionString("Craft") ?? throw new InvalidOperationException("接続文字列を取得できません。");

	/// <summary>
	/// 言語コード
	/// </summary>
	private readonly string _languageCode = configuration.GetValue<string>("LanguageCode") ?? throw new InvalidOperationException("言語コードを取得できません。");

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
		DefaultTypeMap.MatchNamesWithUnderscores = true;

		using SqliteConnection connection = new(_connectionString);
		connection.Open();

		ItemMatter product;
		{
			ItemMatterId resItemMatterId = ItemMatterId.Create();

			ItemRepository itemRepository = new(connection, _languageCode);
			Item resItem = itemRepository.Find(itemId);

			product = new ItemMatter(resItemMatterId, resItem, quantity);
		}

		return product;
	}

	#endregion

	#region Nested types

	/// <summary>
	/// アイテム物質のレコード
	/// </summary>
	/// <param name="ItemMatterId">アイテム物質ID</param>
	/// <param name="ItemId">アイテムID</param>
	/// <param name="Quantity">数量</param>
	private record class ItemMatterRecord(string ItemMatterId, string ItemId, long Quantity);

	#endregion
}
