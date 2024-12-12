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
        public TransformSettings_Identifier Identifier { get; set; }
        public TransformSettings_TendersHistory TendersHistory { get; set; }
        public TransformSettings_Classification Classification { get; set; }

        public TransformSettings()
        {
            this.Status = new TransformSettings_Status();
            this.Identifier = new TransformSettings_Identifier();
            this.TendersHistory = new TransformSettings_TendersHistory();
            this.Classification = new TransformSettings_Classification();
        }
    }

    public class TransformSettings_Identifier
    {
        public bool Enabled { get; set; } = false;
    }

    public class TransformSettings_TendersHistory
    {
        public bool Enabled { get; set; } = false;
    }

    public class TransformSettings_Status
    {
        public bool Enabled { get; set; } = false;
        public HashSet<string> Allow { get; set; } = new HashSet<string>();
    }

    public class TransformSettings_Classification
    {
        public bool Enabled { get; set; } = false;
        public HashSet<string> Bypass { get; set; } = new HashSet<string>();
        public HashSet<string> Block { get; set; } = new HashSet<string>();
    }

    public class ActionSettings
    {
        public ActionSetting_SendMail SendMail { get; set; }
        public ActionSetting_TendersHistory TendersHistory { get; set; }

        public ActionSettings()
        {
            this.SendMail = new ActionSetting_SendMail();
            this.TendersHistory = new ActionSetting_TendersHistory();
        }
    }

    public class ActionSetting_TendersHistory
    {
        public bool Enabled { get; set; } = false;
    }

    public class ActionSetting_SendMail
    {
        public bool Enabled { get; set; } = false;
        public string From { get; set; } = "Tenders Agent";
        public string Username { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public int Port { get; set; } = 25;
        public bool EnableSsl { get; set; } = false;
        public string Subject { get; set; } = @"{Title} - ({ProcuringEntity.Name})";
        public List<string> MailTo { get; set; }
        public string MessageTemplateFile { get; set; }
    }
}
