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

    [Serializable]
    public class PizzaOrder
    {
        public enum PizzaOptions
        {
            Margherita,
            Calabresa,
            Frango,
            Portuguesa,
            Palmito
        }

        public enum SizeOptions
        {
            Brotinho,
            Media,
            Grande,
            Familia
        }

        [Prompt("Qual o tipo de pizza você gostaria pedir? {||}")]
        public PizzaOptions? Pizza { get; set; }

        [Prompt("Escolha qual o tamanho da sua pizza? {||}")]
        public SizeOptions? Size { get; set; }

        [Prompt("Para qual o endereço nós iremos entregar a sua pizza?")]
        public String Address { get; set; }

        public static IForm<PizzaOrder> BuildForm()
        {
            CompletionDelegate<PizzaOrder> processOrder = async (context, state) =>
            {
                await context.PostAsync("Seu pedido foi guardado e logo logo estará na sua casa. Aproveite!");
            };

            return new FormBuilder<PizzaOrder>()
                    .Message("Seja bem vindo a Pizzaria do Bairro!")
                    .Field(nameof(PizzaOrder.Pizza))
                    .Field(nameof(PizzaOrder.Size))
                    .Field(nameof(PizzaOrder.Address), validate: async (state, response) =>
                    {
                        var result = new ValidateResult { IsValid = true };
                        var address = (response as string).Trim();

                        if (String.IsNullOrEmpty(address))
                        {
                            result.Feedback = "Por favor, para qual endereço devo entregar a sua pizza?";
                            result.IsValid = false;
                        }

                        return result;
                    })
                    .Confirm("O seu pedido foi registrado assim {Pizza} {Size} que será entregue em {Address}. Está correto ?")
                    .Message("Obrigado por pedir com a pizzaria do bairro")
                    .OnCompletionAsync(processOrder)
                    .Build();
        }
    }
}