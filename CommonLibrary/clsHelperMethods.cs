using CodeGeneratorBusiness;
using CommonLibrary.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace CommonLibrary
{
    public static class clsHelperMethods
    {
        public static bool DoesTableHaveColumn(List<List<clsColumnInfo>> columnsInfo, string columnName)
        {
            if (columnName == null)
            {
                return false;
            }

            foreach (List<clsColumnInfo> column in columnsInfo)
            {
                if (column.Count > 0)
                {
                    if (column[0].ColumnName.ToLower() == columnName.ToLower())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool DoesTableHaveUsernameAndPassword(List<List<clsColumnInfo>> columnsInfo)
        {
            return (DoesTableHaveColumn(columnsInfo, "username") && DoesTableHaveColumn(columnsInfo, "password"));
        }

        public static void WriteToFile(string path, string value)
        {
            using (StreamWriter writer = new StreamWriter(path.Trim()))
            {
                writer.Write(value);
            }
        }

        public static List<List<clsColumnInfo>> LoadColumnsInfo(string tableName, string databaseName)
        {
            List<List<clsColumnInfo>> columnsInfo = new List<List<clsColumnInfo>>();

            DataTable dt = clsCodeGenerator.GetColumnsNameWithInfo(tableName, databaseName);

            foreach (DataRow row in dt.Rows)
            {
                var columnInfo = new List<clsColumnInfo>
                {
                    new clsColumnInfo
                    {
                        ColumnName = row["Column Name"].ToString(),
                        DataType = row["Data Type"].ToString().ToSqlDbType(),
                        IsNullable = row["Is Nullable"].ToString().ToLower() == "yes",
                        MaxLength = string.IsNullOrWhiteSpace(row["Max Length"].ToString()) ? null : (int?)Convert.ToInt32(row["Max Length"].ToString())
                    }
                };

                columnsInfo.Add(columnInfo);
            }

            return columnsInfo;
        }

        public static string GetSingleColumnName(List<List<clsColumnInfo>> columnsInfo)
        {
            if (columnsInfo == null)
            {
                return "";
            }

            if (columnsInfo.Count > 0)
            {
                string firstValue = columnsInfo[0][0].ColumnName;
                return firstValue.Remove(firstValue.Length - 2);
            }

            return "";
        }
    }
}
