using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace openprocurement_agent.Models
{

    public class AppSettings
    {
        public GlobalSettings Global { get; set; }

        public ActionSettings Action { get; set; }
    }


    public class GlobalSettings
    {
        public int Subtract { get; set; } = 12;
    }


    public class ActionSettings
    {
        public ActionSetting_SendMail SendMail { get; set; }
    }


    public class ActionSetting_SendMail
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public Uri ServerUrl { get; set; }
        public List<string> MailTo { get; set; }

        static String SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}
