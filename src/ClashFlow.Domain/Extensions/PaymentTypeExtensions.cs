using ClashFlow.Domain.Enums;

namespace ClashFlow.Domain.Extensions
{
    public static class PaymentTypeExtensions
    {
        public static string PaymentTypeToString(this PaymentType paymentType)
        {
            return paymentType switch
            {
                PaymentType.Cash => "Dinheiro",
                PaymentType.CreditCard => "Cartão de Crédito",
                PaymentType.DebitCard => "Cartão de Débito",
                PaymentType.EletronicTransfer => "Transferência Eletrônica",
                _ => string.Empty
            };
        }
    }
}
