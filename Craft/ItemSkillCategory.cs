using System.Text.RegularExpressions;

namespace Craft;

/// <summary>
/// アイテムスキルカテゴリー
/// </summary>
public partial record class ItemSkillCategory
{
    #region Fields

    /// <summary>
    /// 値のコレクション
    /// </summary>
    private static readonly IReadOnlyCollection<ItemSkillCategory> _constants;

    /// <summary>
    /// 拾得
    /// </summary>
    public static readonly ItemSkillCategory PickUp;

    /// <summary>
    /// 装備
    /// </summary>
    public static readonly ItemSkillCategory Equipment;

	#endregion

	#region Constructors

    /// <summary>
    /// アイテムスキルカテゴリーを初期化します。
    /// </summary>
    static ItemSkillCategory()
    {
		PickUp = new("pick-up");
        Equipment = new("equipment");

		_constants = [PickUp, Equipment];
    }

	/// <summary>
	/// アイテムスキルカテゴリーを初期化します。
	/// </summary>
	/// <param name="value">値</param>
	public ItemSkillCategory(string value)
    {
        bool succeeded = Validate(value, out string message);
        if (!succeeded) throw new ArgumentException(message, nameof(value));

        Value = value;
    }

    #endregion

    #region Properties

    /// <summary>
    /// 値を取得します。
    /// </summary>
    public string Value { get; }

    #endregion

    #region Methods

    /// <summary>
    /// 検索します。
    /// </summary>
    /// <param name="value">値</param>
    /// <returns>検索したアイテムスキルカテゴリーを返します。</returns>
    public static ItemSkillCategory Find(string value)
    {
        ItemSkillCategory result = _constants.Single(x => x.Value == value);

        return result;
    }

    /// <summary>
    /// 値を検証します。
    /// </summary>
    /// <param name="value">値</param>
    /// <param name="message">メッセージ</param>
    public static bool Validate(string value, out string message)
    {
        bool result = GetRegex().IsMatch(value);

        if (result)
        {
            message = string.Empty;
        }
        else
        {
            message = "アイテムスキルカテゴリーは、半角英数字とハイフンで入力してください。";
        }

        return result;
    }

    /// <summary>
    /// 正規表現を取得します。
    /// </summary>
    /// <returns>正規表現を返します。</returns>
    [GeneratedRegex(@"^[\-0-9a-z]+$")]
    private static partial Regex GetRegex();

    #endregion
}
