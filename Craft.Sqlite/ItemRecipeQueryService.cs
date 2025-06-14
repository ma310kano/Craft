using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Craft.Sqlite;

/// <summary>
/// アイテムレシピの問い合わせサービス
/// </summary>
/// <param name="configuration">設定</param>
public class ItemRecipeQueryService(IConfiguration configuration) : IItemRecipeQueryService
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
	/// アイテムレシピを問い合わせします。
	/// </summary>
	/// <param name="itemRecipeId">アイテムレシピID</param>
	/// <returns>問い合わせしたアイテムレシピを返します。</returns>
	public ItemRecipe QuerySingle(ItemRecipeId itemRecipeId)
	{
		DefaultTypeMap.MatchNamesWithUnderscores = true;

		using SqliteConnection connection = new(_connectionString);
		connection.Open();

		ItemRecipeRepository repository = new(connection, _languageCode);
		
		ItemRecipe result = repository.Find(itemRecipeId);

		return result;
	}

	#endregion
}
