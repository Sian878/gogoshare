using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace 第二組期末專題.Models
{
    public static class Teamdb2
    {
        public static string 連線字串 = "Server=tcp:datateam2.database.windows.net,1433;" +
                                        "Initial Catalog=team2;" +
                                        "Persist Security Info=False;" +
                                        "User ID=team2;" +
                                        "Password=XYZtwo22;" +
                                        "MultipleActiveResultSets=False;" +
                                        "Encrypt=True;" +
                                        "TrustServerCertificate=False;" +
                                        "Connection Timeout=30;";
    }
}