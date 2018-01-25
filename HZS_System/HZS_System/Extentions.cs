using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZS_System
{
    public static class Extentions
    {

        public static string uploads_path = @"C:\Users\Huseyn\Desktop\New folder\HZS_System\HZS_System\Uploads\";

        public static string getHashCode(string password)
        {
            byte[] data = new System.Text.ASCIIEncoding().GetBytes(password);
            var dataEncrypted = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            string hashResult = new System.Text.ASCIIEncoding().GetString(dataEncrypted);
            return hashResult;
        }
    }
}
