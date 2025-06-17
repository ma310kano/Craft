
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Craft.Sqlite;

/// <summary>
/// エリアの問い合わせサービス
/// </summary>
/// <param name="configuration">設定</param>
public class AreaQueryService(IConfiguration configuration) : IAreaQueryService
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
	/// エリアを問い合わせします。
	/// </summary>
	/// <param name="areaId">エリアID</param>
	/// <returns>問い合わせしたエリアを返します。</returns>
	public Area QuerySingle(AreaId areaId)
	{
		DefaultTypeMap.MatchNamesWithUnderscores = true;

		using SqliteConnection connection = new(_connectionString);
		connection.Open();

		AreaRepository areaRepository = new(connection, _languageCode);

		Area result = areaRepository.Find(areaId);

		return result;
	}

	/// <summary>
	/// エリアを問い合わせします。
	/// </summary>
	/// <param name="areaId">エリアID</param>
	/// <returns>問い合わせしたエリアを返します。</returns>
	public async Task<Area> QuerySingleAsync(AreaId areaId)
	{
		return await Task.Run(() => QuerySingle(areaId));
	}

	#endregion
}
