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

			Dictionary<ItemSkillCategory, List<Skill>> itemSkills = [];
			{
				const string sql = @"SELECT
	  isk.item_skill_category
	, isk.skill_id
FROM
	item_skills isk
	INNER JOIN skills ski
		ON isk.skill_id = ski.skill_id
WHERE
	isk.item_id = :item_id
ORDER BY
	isk.skill_id";

				var param = new
				{
					item_id = itemId.Value,
					language_code = languageCode,
				};

				IEnumerable<SkillRecord> records = connection.Query<SkillRecord>(sql, param);

				foreach (SkillRecord record in records)
				{
					ItemSkillCategory category = ItemSkillCategory.Find(record.ItemSkillCategory);

					List<Skill> skills;
					{
						bool hasSkills = itemSkills.TryGetValue(category, out List<Skill>? value);
						if (hasSkills)
						{
							skills = value!;
						}
						else
						{
							skills = [];
							itemSkills.Add(category, skills);
						}
					}

					Skill skill;
					{
						SkillId skillId = new(record.SkillId);

						skill = _skillRepository.Find(skillId);
					}

					skills.Add(skill);
				}
			}

			IReadOnlyDictionary<ItemSkillCategory, IReadOnlyCollection<Skill>> categorySkills = itemSkills.Select(x => new KeyValuePair<ItemSkillCategory, IReadOnlyCollection<Skill>>(x.Key, x.Value)).ToDictionary();

			result = new Item(resItemId, resItemName, categorySkills);

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

	/// <summary>
	/// スキルのレコード
	/// </summary>
	/// <param name="ItemSkillCategory">アイテムスキルカテゴリー</param>
	/// <param name="SkillId">スキルID</param>
	private record class SkillRecord(string ItemSkillCategory, string SkillId);

	#endregion
}
