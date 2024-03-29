﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Connector;
using BMC.Sarah.ChatBot.Services;
using BMC.Sarah.ChatBot.Helpers;

namespace BMC.Sarah.ChatBot.Dialogs
{
    public class SearchScorable : ScorableBase<IActivity, string, double>
    {
        private const string TRIGGER = "cari tentang ";
        private readonly AzureSearchService searchService = new AzureSearchService();

        protected override Task DoneAsync(IActivity item, string state, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        protected override double GetScore(IActivity item, string state)
        {
            return 1.0;
        }

        protected override bool HasScore(IActivity item, string state)
        {
            return !string.IsNullOrWhiteSpace(state);
        }

        protected async override Task PostAsync(IActivity item, string state, CancellationToken token)
        {
            var searchResult = await this.searchService.Search(state);

            var replyActivity = ((Activity)item).CreateReply();
            await CardUtil.ShowSearchResults(replyActivity, searchResult, $"Maaf, saya tidak mengerti '{state}'.\nKetik 'tolong' untuk info yang membantu Anda :)");
        }

        protected async override Task<string> PrepareAsync(IActivity item, CancellationToken token)
        {
            var message = item.AsMessageActivity();
            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                if (message.Text.Trim().StartsWith(TRIGGER, StringComparison.InvariantCultureIgnoreCase))
                {
                    return message.Text.Substring(TRIGGER.Length);
                }
            }

            return null;
        }
    }
}