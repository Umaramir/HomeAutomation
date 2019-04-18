using BMC.Sarah.ChatBot.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BMC.Sarah.ChatBot.Dialogs
{
    public class AuthDialog : IDialog<object>
    {
        //private readonly IDialogTask task;

        const string PassCode = "Type Passcode";

        const string Yes = "Yes, I am Member";
        const string No = "No I am just a Guest";
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Type the SpellWord");

            context.Wait(this.MessageReceivedAsync);
        }
        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            string InputUser = message.ToString();
            string PassCode = "Champion";
            if (message.Text == PassCode)
            {
                await context.PostAsync($"Hello fellas (👍^-^👍)");
                //this.task.Reset();
                context.Call(new MenuDialog(), ResumeAfterOptionDialog);

                //var commonResponsesDialog = new MenuDialog();
                //var interruption = commonResponsesDialog.Void<object, IMessageActivity>();
                //this.task.Call(interruption, null);
            }
            else
            {
                await context.PostAsync($"Wrong SpellWords (¬‿¬)" +
                                        $"I am rebooting . . .");
                await context.PostAsync($"Hi there (づ｡◕‿‿◕｡)づ ");
                context.Done<object>(null);
            }
        }
        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            //var message = await result;
            //context.Done<object>(null);
            try
            {
                var message = await result;
                await context.PostAsync($"Thats it? Okey (づ￣ ³￣)づ ");
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Uhh hmm I'm trying to fix : {ex.Message}");
            }
            finally
            {
                context.Wait(this.MessageReceivedAsync);
                context.Done<object>(null);
            }

        }
    }
}