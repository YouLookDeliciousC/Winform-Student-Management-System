using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace WinStudent
{
    class CommandInfo
    {
        public string CommandText; //sql或存储过程
        public DbParameter[] Parameters;
        public bool IsProc; //是否是存储过程

        public CommandInfo()
        {

        }
        public CommandInfo(string comText,bool isProc)
        {
            this.CommandText = comText;
            this.IsProc = isProc;
        }

        public CommandInfo(string sqlText,bool isProc, DbParameter[] para)
        {
            this.CommandText = sqlText;
            this.Parameters = para;
            this.IsProc = isProc;
        }



    }
}
