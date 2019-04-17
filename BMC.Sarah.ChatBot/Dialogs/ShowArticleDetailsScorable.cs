using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Connector;
using BMC.Sarah.ChatBot.Services;

namespace BMC.Sarah.ChatBot.Dialogs
{
    public class ShowArticleDetailsScorable : ScorableBase<IActivity, string, double>
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
            ConnectorClient connector = new ConnectorClient(new Uri(item.ServiceUrl));
            var reply = "Maaf, tidak dapat menemukan artikel tentang hal tersebut.";

            var searchResult = await this.searchService.SearchByTitle(state.ToString());
            if (searchResult != null && searchResult.Value.Length != 0)
            {
                reply = searchResult.Value[0].Description;
            }

            var replyActivity = ((Activity)item).CreateReply(reply);
            await connector.Conversations.ReplyToActivityAsync(replyActivity);
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