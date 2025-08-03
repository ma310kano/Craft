using Dapper;
using System.Data;

namespace Craft.Sqlite;

/// <summary>
/// スキルのリポジトリー
/// </summary>
/// <param name="connection">コネクション</param>
/// <param name="languageCode">言語コード</param>
public class SkillRepository(IDbConnection connection, string languageCode) : ISkillRepository
{
	#region Fields

	/// <summary>
	/// キャッシュ
	/// </summary>
	private static readonly Dictionary<SkillId, Skill> _cache = [];

	#endregion

	#region Methods

	/// <summary>
	/// スキルを検索します。
	/// </summary>
	/// <param name="skillId">スキルID</param>
	/// <returns>検索したスキルを返します。</returns>
	public Skill Find(SkillId skillId)
	{
		bool exists = _cache.TryGetValue(skillId, out Skill? result);

		if (!exists)
		{
			const string sql = @"SELECT
	  ski.skill_id
	, snm.skill_name
FROM
	skills ski
	INNER JOIN skill_names snm
		ON  ski.skill_id = snm.skill_id
		AND snm.language_code = :language_code
WHERE
	ski.skill_id = :skill_id";

			var param = new
			{
				skill_id = skillId.Value,
				language_code = languageCode,
			};

			SkillRecord record = connection.QuerySingle<SkillRecord>(sql, param);

			SkillId resSkillId = new(record.SkillId);
			SkillName resSkillName = new(record.SkillName);

			result = new Skill(resSkillId, resSkillName);

			_cache.Add(result.SkillId, result);
		}

		return result!;
	}

	#endregion

	#region Nested types

	/// <summary>
	/// スキルのレコード
	/// </summary>
	/// <param name="SkillId">スキルID</param>
	/// <param name="SkillName">スキル名</param>
	private record class SkillRecord(string SkillId, string SkillName);

	#endregion
}
