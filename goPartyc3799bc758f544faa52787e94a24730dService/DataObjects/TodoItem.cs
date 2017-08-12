using Microsoft.Azure.Mobile.Server;

namespace goPartyc3799bc758f544faa52787e94a24730dService.DataObjects
{
    public class TodoItem : EntityData
    {
        public string Text { get; set; }

        public bool Complete { get; set; }
    }
}