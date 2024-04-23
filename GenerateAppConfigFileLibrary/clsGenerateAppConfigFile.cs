using System.IO;
using System.Text;

namespace GenerateAppConfigFileLibrary
{
    public static class clsGenerateAppConfigFile
    {
        private static StringBuilder _tempText;

        static clsGenerateAppConfigFile()
        {
            _tempText = new StringBuilder();
        }

        internal static void WriteToFile(string path, string value)
        {
            using (StreamWriter writer = new StreamWriter(path.Trim()))
            {
                writer.Write(value);
            }
        }

        public static void CreateAppConfigFile(string path, string databaseName)
        {
            _tempText.Clear();

            _tempText.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            _tempText.AppendLine("<configuration>");
            _tempText.AppendLine();
            _tempText.AppendLine("\t<startup>");
            _tempText.AppendLine("\t\t<supportedRuntime version=\"v4.0\" sku=\".NETFramework,Version=v4.8\" />");
            _tempText.AppendLine("\t</startup>");
            _tempText.AppendLine();
            _tempText.AppendLine("\t<appSettings>");
            _tempText.AppendLine($"\t\t<add key=\"ProjectName\" value=\"{databaseName}\" />");
            _tempText.AppendLine("\t</appSettings>");
            _tempText.AppendLine();
            _tempText.AppendLine("\t<connectionStrings>");
            _tempText.AppendLine($"\t\t<add name=\"ConnectionString\" connectionString=\"Server=.;Database={databaseName};Integrated Security=True;\" providerName=\"System.Data.SqlClient\" />");
            _tempText.AppendLine("\t</connectionStrings>");
            _tempText.AppendLine();
            _tempText.AppendLine("</configuration>");

            WriteToFile(path, _tempText.ToString());
        }
    }
}
