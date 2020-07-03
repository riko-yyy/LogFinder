using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace LogFinder
{
    class Program
    {
        private static string InputDir;
        private static string OutputDir;
        private static string Keyword;

        /// <summary>
        /// メイン処理
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)

        {
            Console.WriteLine("走査したいファイルを入力フォルダにセットしてください");
            Console.ReadLine();
            Console.WriteLine("検索したい文字列を入力してください");
            Keyword = Console.ReadLine();

            Initialize();

            ScanInDirectory();

            Console.WriteLine("処理が完了しました");
            Console.WriteLine("出力フォルダを確認してください");
            Console.ReadLine();
        }

        /// <summary>
        /// 初期処理
        /// </summary>
        private static void Initialize()
        {
            InputDir = ConfigurationManager.AppSettings["INPUT-DIRECTORY"];
            OutputDir = ConfigurationManager.AppSettings["OUTPUT-DIRECTORY"];
        }

        /// <summary>
        /// ディレクトリを走査
        /// </summary>
        private static void ScanInDirectory()
        {
            var files = Directory.EnumerateFiles(InputDir);

            foreach (var file in files)
            {
                var result = ScanInFile(file);
                OutputFile(file, result);
            }

        }

        /// <summary>
        /// ファイルを行単位に操作
        /// </summary>
        /// <param name="file">走査対象</param>
        /// <returns></returns>
        private static StringBuilder ScanInFile(string file)
        {
            StringBuilder holder = new StringBuilder();

            using (StreamReader sr = new StreamReader(file))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains(Keyword))
                    {
                        holder.AppendLine(line);
                    }
                }
            }
            return holder;
        }

        /// <summary>
        /// 走査結果を出力
        /// </summary>
        /// <param name="inputFile">走査対象ファイル</param>
        /// <param name="result">走査結果</param>
        private static void OutputFile(string inputFile, StringBuilder result)
        {
            using (StreamWriter sw = new StreamWriter(GetOutputFile(inputFile)))
            {
                sw.Write(result.ToString());
            }
        }

        /// <summary>
        /// 出力するファイル名を生成
        /// </summary>
        /// <param name="inputFile">走査対象ファイル</param>
        /// <returns></returns>
        private static string GetOutputFile(string inputFile)
        {
            return @$"{OutputDir}\{DateTime.Now:yyyyMMddHHmmss}_{Path.GetFileNameWithoutExtension(inputFile)}.txt";
        }
    }
}
