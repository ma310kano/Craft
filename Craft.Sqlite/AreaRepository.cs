using Dapper;
using System.Data;

namespace Craft.Sqlite;

/// <summary>
/// エリアのリポジトリー
/// </summary>
/// <param name="connection">コネクション</param>
/// <param name="languageCode">言語コード</param>
public class AreaRepository(IDbConnection connection, string languageCode) : IAreaRepository
{
	#region Fields

	/// <summary>
	/// エリアのコレクション
	/// </summary>
	private static readonly Dictionary<AreaId, Area> _areas = [];

	/// <summary>
	/// アイテムのリポジトリー
	/// </summary>
	private readonly ItemRepository _itemRepository = new(connection, languageCode);

	#endregion

	#region Methods

	/// <summary>
	/// エリアを検索します。
	/// </summary>
	/// <param name="areaId">エリアID</param>
	/// <returns>検索したエリアを返します。</returns>
	public Area Find(AreaId areaId)
	{
		bool got = _areas.TryGetValue(areaId, out Area? value);

		Area result;
		if (got && value is not null)
		{
			result = value;
		}
		else
		{
			AreaId resAreaId;
			AreaName resAreaName;
			{
				const string sql = @"SELECT
	  are.area_id
	, anm.area_name
FROM
	areas are
	INNER JOIN area_names anm
		ON  are.area_id = anm.area_id
		AND anm.language_code = :language_code
WHERE
	are.area_id = :area_id";

				var param = new
				{
					area_id = areaId.Value,
					language_code = languageCode,
				};

				AreaRecord source = connection.QuerySingle<AreaRecord>(sql, param);

				resAreaId = new(source.AreaId);
				resAreaName = new(source.AreaName);
			}

			List<Item> items = [];
			{
				const string sql = @"SELECT
	ait.item_id
FROM
	areas are
	INNER JOIN area_items ait
		ON are.area_id = ait.area_id
WHERE
	are.area_id = :area_id";

				var param = new
				{
					area_id = areaId.Value,
				};

				IEnumerable<ItemId> itemIds = connection.Query<string>(sql, param).Select(x => new ItemId(x));

				foreach (ItemId itemId in itemIds)
				{
					Item item = _itemRepository.Find(itemId);

					items.Add(item);
				}
			}

			List<Human> humans = [];

			result = new Area(resAreaId, resAreaName, items, humans);

			_areas.Add(result.AreaId, result);
		}

		return result;
	}

	#endregion

	#region Nested types

	/// <summary>
	/// エリアのレコード
	/// </summary>
	/// <param name="AreaId">エリアID</param>
	/// <param name="AreaName">エリア名</param>
	private record class AreaRecord(string AreaId, string AreaName);

	#endregion
}
