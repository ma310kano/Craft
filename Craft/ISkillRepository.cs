namespace Craft;

/// <summary>
/// スキルのリポジトリー
/// </summary>
public interface ISkillRepository
{
	#region Methods

	/// <summary>
	/// スキルを検索します。
	/// </summary>
	/// <param name="skillId">スキルID</param>
	/// <returns>検索したスキルを返します。</returns>
	Skill Find(SkillId skillId);

	#endregion
}
