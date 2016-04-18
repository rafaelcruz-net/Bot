using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Dialogs;


namespace Bot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        internal static IDialog<PizzaOrder> MakeRootDialog()
        {
            return Chain.From(() => FormDialog.FromForm(PizzaOrder.BuildForm));
        }

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<Message> Post([FromBody]Message message)
        {
            if (message.Type == "Message")
            {
                return await Conversation.SendAsync(message, MakeRootDialog);
            }

            return HandleSystemMessage(message);
        }

        private Message HandleSystemMessage(Message message)
        {
            return null;
        }
    }
}