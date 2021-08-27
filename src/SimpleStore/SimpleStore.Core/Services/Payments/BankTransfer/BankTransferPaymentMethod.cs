namespace SimpleStore.Core.Services.Payments.BankTransfer
{
    public class BankTransferPaymentMethod : IPaymentMethod
    {
        public string Identificator => "BankTransfer";
        public string Name => "Transferência Bancária";
        public string DescriptionComponent => throw new NotImplementedException();

        public string ImageUrl => "";

    }
}
