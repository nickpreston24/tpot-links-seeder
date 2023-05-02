using System.IO;
using System.Text.RegularExpressions;
using CodeMechanic.Extensions;
using CodeMechanic.Advanced.Extensions;

public static class DotEnv 
{
    public static bool Load(string file_path = ".env") 
    {
        if(!File.Exists(file_path))
            return false;

        string raw_text = File.ReadAllText(file_path);

        var regex_options = 
            RegexOptions.Compiled
            | RegexOptions.IgnoreCase
            | RegexOptions.ExplicitCapture
            | RegexOptions.Multiline
            | RegexOptions.IgnorePatternWhitespace;

        var raw_settings = raw_text.Extract<DotEnvSetting>(
            DotEnvSetting.regex_pattern,
            options: regex_options
        );

        raw_settings.Dump();

        foreach(var setting in raw_settings){
            Environment.SetEnvironmentVariable(setting.Left, setting.Right);
        }

        return true;
    }
}

public class DotEnvSetting 
{

    public string Left { get; set; } 
    public string Right { get; set; } 

    public const string regex_pattern = $"""
        (?<Left>\w+) # Match alphas and underscore
        =?                  # Match only the first equals sign
        (?<Right>.*)        # Match anything
    """;
}

public static class DotEnvSettingExtensions 
{
    public static void Deconstruct<T>(
        this T? setting,
        out string left,
        out string right) where T: DotEnvSetting
    {
        left = setting.Left;
        right = setting.Right;
    }
}

