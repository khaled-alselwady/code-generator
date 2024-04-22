namespace CodeGeneratorDataAccess
{
    static class clsDataAccessSettings
    {
        public static string ConnectionString(string DatabaseName = "master")
        {
            return $"Data Source=.;Initial Catalog={DatabaseName};Integrated Security=True";
        }
    }
}
