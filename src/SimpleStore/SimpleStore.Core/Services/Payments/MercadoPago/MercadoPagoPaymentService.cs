namespace SimpleStore.Core.Services.Payments.BankTransfer
{
    public class MercadoPagoPaymentService : IPaymentMethod
    {
        public string Identificator => "MercadoPago";
        public string Name => "Mercado Pago";
        public string ImageUrl => "";

        public string DescriptionComponent => throw new NotImplementedException();
    }
}
