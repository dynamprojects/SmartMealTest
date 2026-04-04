using System.Windows.Input;

namespace SmartMealWPF.Helpers;

public static class ValidationInputHelper
{
    public static bool Validate(string text)
    {
        return text.All(c => 
            char.IsLetterOrDigit(c) || 
            c == '_' || 
            char.IsWhiteSpace(c) || 
            c == '/' || c == '\\' || c == '.');
    }
}