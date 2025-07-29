using System.Text.RegularExpressions;

namespace Craft;

/// <summary>
/// スキルID
/// </summary>
public partial record class SkillId
{
    #region Constructors

    /// <summary>
    /// スキルIDを初期化します。
    /// </summary>
    /// <param name="value">値</param>
    public SkillId(string value)
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
            message = "スキルIDは、半角英数字とハイフンで入力してください。";
        }

        return result;
    }

    /// <summary>
    /// 正規表現を取得します。
    /// </summary>
    /// <returns>正規表現を返します。</returns>
    [GeneratedRegex(@"^[\-a-z0-9]+$")]
    private static partial Regex GetRegex();

    #endregion
}
