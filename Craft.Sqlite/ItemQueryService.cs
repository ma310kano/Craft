using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Craft.Sqlite;

/// <summary>
/// アイテムの問い合わせサービス
/// </summary>
/// <param name="configuration">設定</param>
public class ItemQueryService(IConfiguration configuration) : IItemQueryService
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
	/// アイテムを問い合わせします。
	/// </summary>
	/// <param name="itemId">アイテムID</param>
	/// <returns>問い合わせたアイテムを返します。</returns>
	public Item QuerySingle(ItemId itemId)
	{
		DefaultTypeMap.MatchNamesWithUnderscores = true;

		using SqliteConnection connection = new(_connectionString);
		connection.Open();

		ItemRepository repository = new(connection, _languageCode);

		Item result = repository.Find(itemId);

		return result;
	}

	#endregion
}
