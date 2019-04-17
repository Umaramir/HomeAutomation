using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;


namespace BMC.Sarah.ChatBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public MessagesController()
        {
         
        }

        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.GetActivityType() == ActivityTypes.Message)
            {

                await Conversation.SendAsync(activity, () => new Dialogs.MainDialog());
            }
            else
            {
                await HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task HandleSystemMessage(Activity message)
        {
            string messageType = message.GetActivityType();
            if (messageType == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
                if (message.MembersAdded.Any(o => o.Id == message.Recipient.Id))
                {
                    ConnectorClient client = new ConnectorClient(new Uri(message.ServiceUrl));

                    //var NewItem = new BotUserData();

                    //NewItem.toId = message.From.Id;
                    //NewItem.toName = message.From.Name;
                    //NewItem.fromId = message.Recipient.Id;
                    //NewItem.fromName = message.Recipient.Name;
                    //NewItem.serviceUrl = message.ServiceUrl;
                    //NewItem.channelId = message.ChannelId;
                    //NewItem.conversationId = message.Conversation.Id;

                    //NewItem.AssignKey();
                    //await SaveData(NewItem);
                    var reply = message.CreateReply();

                    reply.Text = "HaiSarah is here (づ｡◕‿‿◕｡)づ";

                    await client.Conversations.ReplyToActivityAsync(reply);
                }


            }
            else if (messageType == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (messageType == ActivityTypes.Typing)
            {
                // Handle knowing that the user is typing
            }
            else if (messageType == ActivityTypes.Ping)
            {
               
            }

            //return null;
        }
        /*public static CloudTable cloudTable
        {
            get
            {
                var cloudStorageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=padidata;AccountKey=G0CSG/ndz66adthIyIicdqh6lTv0oHc7D5HrgiGMjoBZhYNGsknrz9bMNkT3w2cSssh2BFoPF8s7P3FZNDOZJQ==;EndpointSuffix=core.windows.net");
                CloudTableClient tableClient = cloudStorageAccount.CreateCloudTableClient();
                var cloudTable = tableClient.GetTableReference("BotUserData");
                return cloudTable;
            }
        }
        public async static Task<bool> SaveData(BotUserData datas)
        {

            // Create the TableOperation that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(datas);

            // Execute the insert operation.
            var res = await cloudTable.ExecuteAsync(insertOperation);
            return true;
        }

        public class BotUserData : TableEntity
        {
            public void AssignKey()
            {
                this.RowKey = shortid.ShortId.Generate(true, false, 10).ToUpper();
                this.PartitionKey = DateTime.Now.ToString("yyyy_MM_dd");
            }
            public string channelId { get; set; }
            public string conversationId { get; set; }
            public string fromId { get; set; }
            public string fromName { get; set; }
            public string serviceUrl { get; set; }
            public string toId { get; set; }
            public string toName { get; set; }
        } */
    }
}