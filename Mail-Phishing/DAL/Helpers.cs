﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mail_Phishing.DAL
{
    public static class Helpers
    {
        //
        // DATA CASTING HELPERS
        public static object ReturnZeroIfNull(object value)
        {
            if (value == System.DBNull.Value)
                return 0;
            else if (value == null)
                return 0;
            else
                return value;
        }

        public static object ReturnEmptyIfNull(object value)
        {
            if (value == System.DBNull.Value)
                return string.Empty;
            else if (value == null)
                return string.Empty;
            else
                return value;
        }

        public static object ReturnFalseIfNull(object value)
        {
            if (value == System.DBNull.Value)
                return false;
            else if (value == null)
                return false;
            else
                return value;
        }
    }
}
