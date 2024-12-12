using openprocurement.api.client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openprocurement_agent.MessagePipeline
{
    public enum MessageTenderStatus 
    { 
        NextTarget,
        NullTarget,
        SendTarget
    }

    public class MessageTender
    {
        public MessageTenderStatus Status { get; set; } = MessageTenderStatus.NextTarget;
        public Tender? Item {  get; set; }
    }
}
