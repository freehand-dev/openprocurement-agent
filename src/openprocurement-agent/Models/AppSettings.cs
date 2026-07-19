using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace openprocurement_agent.Models
{
    /// <summary>
    /// Root configuration object bound from the .conf file.
    /// Pipeline (Global/Action/Transform) and Mail (Action:SendMail) settings have
    /// been moved to <see cref="PipelineSettingsDbContext"/> and are managed via
    /// the Settings page.
    /// </summary>
    public class AppSettings
    {
    }
}

