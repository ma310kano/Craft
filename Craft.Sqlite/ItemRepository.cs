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
	#region Fields

	/// <summary>
	/// アイテムのコレクション
	/// </summary>
	private static readonly Dictionary<ItemId, Item> _cache = [];

	/// <summary>
	/// スキルのリポジトリー
	/// </summary>
	private readonly SkillRepository _skillRepository = new(connection, languageCode);

	#endregion

	#region Methods

	/// <summary>
	/// アイテムを検索します。
	/// </summary>
	/// <param name="itemId">アイテムID</param>
	/// <returns>検索したアイテムを返します。</returns>
	public Item Find(ItemId itemId)
	{
		Item result;

		if (_cache.ContainsKey(itemId))
		{
			result = _cache[itemId];
		}
		else
		{
			ItemId resItemId;
			ItemName resItemName;
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

				resItemId = new(record.ItemId);
				resItemName = new(record.ItemName);
			}

			List<Skill> skillsActivatedByEquipping = [];
			{
				const string sql = @"SELECT
	skill_id
FROM
	item_skills_activated_by_equipping
WHERE
	item_id = :item_id
ORDER BY
	  item_id
	, skill_id";

				var param = new
				{
					item_id = itemId.Value,
				};

				IEnumerable<string> sources = connection.Query<string>(sql, param);

				foreach (string source in sources)
				{
					Skill skill;
					{
						SkillId skillId = new(source);

						skill = _skillRepository.Find(skillId);
					}

					skillsActivatedByEquipping.Add(skill);
				}
			}

			List<Skill> skillsNeededToPickup = [];
			{
				const string sql = @"SELECT
	skill_id
FROM
	item_skills_needed_to_pickup
WHERE
	item_id = :item_id
ORDER BY
	  item_id
	, skill_id";

				var param = new
				{
					item_id = itemId.Value,
				};

				IEnumerable<string> sources = connection.Query<string>(sql, param);

				foreach (string source in sources)
				{
					Skill skill;
					{
						SkillId skillId = new(source);

						skill = _skillRepository.Find(skillId);
					}

					skillsNeededToPickup.Add(skill);
				}
			}

			result = new Item(resItemId, resItemName, skillsActivatedByEquipping, skillsNeededToPickup);

			_cache.Add(result.ItemId, result);
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
