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

        public TransformSettings Transform { get; set; }

        public AppSettings()
        {
            this.Global = new GlobalSettings();
            this.Action = new ActionSettings();
            this.Transform = new TransformSettings();
        }
    }


    public class GlobalSettings
    {
        public int Subtract { get; set; } = 12;
    }


    public class TransformSettings
    {
        public TransformSettings_Status Status { get; set; }

        public TransformSettings()
        {
            this.Status = new TransformSettings_Status();
        }

    }


    public class TransformSettings_Status
    {
        public List<string> Allow { get; set; } = new List<string>();

    }

    public class ActionSettings
    {
        public ActionSetting_SendMail SendMail { get; set; }

        public ActionSettings()
        {
            this.SendMail = new ActionSetting_SendMail();
        }
    }


    public class ActionSetting_SendMail
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Server { get; set; }

        public int Port { get; set; } = 25;

        public bool EnableSsl { get; set; } = false;

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
