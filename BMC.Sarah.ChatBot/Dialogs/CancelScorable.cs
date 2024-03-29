﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using BMC.Security.Web.Helpers;

namespace BMC.Sarah.ChatBot.Dialogs
{
    public class CancelScorable : ScorableBase<IActivity, string, double>
    {
        private readonly IDialogTask task;
        //static MqttService iot;
        public CancelScorable(IDialogTask task)
        {
            SetField.NotNull(out this.task, nameof(task), task);
        }

        protected override async Task<string> PrepareAsync(IActivity activity, CancellationToken token)
        {
            var message = activity as IMessageActivity;

            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                if (message.Text.Equals("kembali", StringComparison.InvariantCultureIgnoreCase))
                {
                    return message.Text;
                }
                else if (message.Text.Equals("menu", StringComparison.InvariantCultureIgnoreCase))
                {
                    return message.Text;
                }
                else if (message.Text.Equals("mulai", StringComparison.InvariantCultureIgnoreCase))
                {
                    return message.Text;
                }
                else if (message.Text.Equals("start", StringComparison.InvariantCultureIgnoreCase))
                {
                    return message.Text;
                }
            }

            return null;
        }

        protected override bool HasScore(IActivity item, string state)
        {
            return state != null;
        }

        protected override double GetScore(IActivity item, string state)
        {
            return 1.0;
        }

        protected override async Task PostAsync(IActivity item, string state, CancellationToken token)
        {
            var message = item as IMessageActivity;
            var messageToSend = "keluar dari percakapan, dan kembali ke awal, silakan ketik 'menu'";
            var pesan = message.Text.ToLower();
            var pw = "123qweasd";
            var reply = message;
            if (message != null)
            {
                //var incomingMessage = message.Text.ToLowerInvariant();
                this.task.Reset();
                //var commonResponsesDialog = new CommonResponseDialog(messageToSend);
                //if (pesan == pw)
                //{
                //    var commonResponsesDialog = new MenuDialog();
                //    var interruption = commonResponsesDialog.Void<object, IMessageActivity>();
                //    this.task.Call(interruption, null);
                //    await this.task.PollAsync(token);
                //}
                //else
                //{
                //    reply.Text = "salah pw";
                //}
                
                var commonResponsesDialog = new MenuDialog();//AuthDialog();
                var interruption = commonResponsesDialog.Void<object, IMessageActivity>();
                this.task.Call(interruption, null);
                await this.task.PollAsync(token);
               

            }

        }

        protected override Task DoneAsync(IActivity item, string state, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}