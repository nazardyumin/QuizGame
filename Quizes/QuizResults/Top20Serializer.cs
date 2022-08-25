using System.Text.Json;
using QuizGame.Configs;

namespace QuizGame.Quizes.QuizResults
{
    public class Top20Serializer
    {
        protected Top20Serializer()
        {
        }
        public static void CreateTop20File(string theme)
        {
            var path = PathInit();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var file = new FileStream(path + theme + "Top20.json", FileMode.Create, FileAccess.Write);
            Serialize(ref file, new List<RatingPosition>());
            file.Close();
        }
        private static List<RatingPosition> LoadFromFile(string path, string theme)
        {
            if (theme != "Mixed Quiz (Mixed)")
            {
                var file = new FileStream(path + theme + "Top20.json", FileMode.Open, FileAccess.Read);
                var list = Deserialize(ref file);
                file.Close();
                return list;
            }
            else return new List<RatingPosition>();
        }
        private static void SaveToFile(string path, string theme, List<RatingPosition> list)
        {
            File.Delete(path + theme + "Top20.json");
            var file = new FileStream(path + theme + "Top20.json", FileMode.Create, FileAccess.Write);
            Serialize(ref file, list);
            file.Close();
        }
        public static void Add(string theme, RatingPosition position)
        {
            if (position.Scores > 0)
            {
                var path = PathInit();
                var list = LoadFromFile(path, theme);
                if (list.Count > 0)
                {
                    if (list.Find((i) => i.Login == position.Login) is not null)
                    {
                        var item = list.Find((i) => i.Name == position.Name);
                        if (item!.Scores < position.Scores) item.Scores = position.Scores;
                    }
                    else
                    {
                        list.Add(position);
                        var ordered = list.OrderByDescending((p) => p.Scores);
                        var i = 0;
                        foreach (var item in ordered)
                        {
                            list[i] = item;
                            i++;
                        }
                    }
                }
                else
                {
                    list.Add(position);
                }
                SaveToFile(path, theme, list);
            }
        }
        public static List<RatingPosition> GetTop20(string theme)
        {
            var path = PathInit();
            var list = LoadFromFile(path, theme);
            return list;
        }
        public static void DeleteTop20File(string theme)
        {
            var path = PathInit();
            File.Delete(path + theme + "Top20.json");
        }
        private static string PathInit()
        {
            return PathsConfig.Init().PathToTop20!;
        }
        private static void Serialize(ref FileStream file, List<RatingPosition> results)
        {
            JsonSerializer.SerializeAsync(file, results);
        }
        private static List<RatingPosition> Deserialize(ref FileStream file)
        {
            return JsonSerializer.DeserializeAsync<List<RatingPosition>>(file).AsTask().Result!;
        }
    }
}
