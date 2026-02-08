using System;
using System.Reflection;

namespace CgmInfo.Commands
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class TextTokenAttribute : Attribute
    {
        public string Token { get; }
        public string? EndToken { get; set; }

        public TextTokenAttribute(string token)
        {
            Token = token;
        }

        public static string? GetToken(Command command)
        {
            var textTokenAttribute = command?.GetType().GetTypeInfo().GetCustomAttribute<TextTokenAttribute>();
            return textTokenAttribute?.Token;
        }
        public static string? GetEndToken(Command command)
        {
            var textTokenAttribute = command?.GetType().GetTypeInfo().GetCustomAttribute<TextTokenAttribute>();
            return textTokenAttribute?.EndToken;
        }
        // enums shouldn't really have an EndToken, so it doesn't make much sense to provide a GetEndToken method here.
        public static string? GetToken<TEnum>(TEnum enumValue) where TEnum : Enum
        {
            var textTokenAttribute = typeof(TEnum).GetRuntimeField(enumValue.ToString())?.GetCustomAttribute<TextTokenAttribute>();
            return textTokenAttribute?.Token;
        }
    }
}
