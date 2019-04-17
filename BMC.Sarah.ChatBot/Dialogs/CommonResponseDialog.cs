using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BMC.Sarah.ChatBot.Dialogs
{
    [Serializable]
    public class CommonResponseDialog : IDialog<object>
    {
        private readonly string _messageToSend;

        public CommonResponseDialog(string message)
        {
            _messageToSend = message;
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(_messageToSend);
            context.Done<object>(null);
        }
    }
}