using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DataAccess
{
    internal class Ultilities
    {
        static string StrName = "ConnectionStringName";
        public static string ConnectionString = ConfigurationManager.ConnectionStrings[StrName].ConnectionString;
        //FOOD
        public static string Food_GetAll = "Food_GetAll";
        public static string Food_InsertUpdateDelete = "Food_InsertUpdate";
        //CATEGORY
        public static string Category_InsertUpdateDelete = "Category_InsertUpdateDelete";
        public static string Category_GetAll = "Category_GetAll";
    }
}
