using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator_DataAccess
{
    static class clsDataAccessSettings
    {
        public static string ConnectionString(string DatabaseName = "master")
        {
            return $"Data Source=.;Initial Catalog={DatabaseName};Integrated Security=True";
        }
    }
}
