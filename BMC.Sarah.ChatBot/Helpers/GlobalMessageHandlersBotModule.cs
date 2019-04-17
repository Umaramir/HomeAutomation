using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using BMC.Sarah.ChatBot.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Scorables;
using Microsoft.Bot.Connector;

namespace BMC.Sarah.ChatBot.Helpers
{
    public class GlobalMessageHandlersBotModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterModule(new DefaultExceptionMessageOverrideModule());
            //builder.Update(Conversation.Container);

            builder
                .Register(c => new CancelScorable(c.Resolve<IDialogTask>()))
                .As<IScorable<IActivity, double>>()
                .InstancePerLifetimeScope();


        }
    }
}