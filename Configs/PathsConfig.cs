using System.Text.Json;

namespace QuizGame.Configs
{
    public class PathsConfig
    {
        public string? PathToUserList { get; set; }
        public string? PathToHighscores { get; set; }
        public string? PathToQuizes { get; set; }
        public static PathsConfig Init()
        {
            using var file = new FileStream("PathsConfig.json", FileMode.Open, FileAccess.Read);
            var config = JsonSerializer.DeserializeAsync<PathsConfig>(file).AsTask().Result;
            if (config!.PathToUserList == "" || config.PathToHighscores == "" || config.PathToQuizes == "")
            {
                config.PathToUserList = config.PathToHighscores = config.PathToQuizes = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                config.PathToUserList += @"\\MyQuizGame\\Users\\";
                config.PathToHighscores += @"\\MyQuizGame\\Highscores\\";
                config.PathToQuizes += @"\\MyQuizGame\\Quizes\\";
            }
            return config;
        }
    }
}
