namespace SimpleStore.Core.Services.Payments
{
    public interface IPaymentMethod
    {
        string Identificator { get; }

        string Name { get; }

        string DescriptionComponent { get; }

        string ImageUrl { get; }
    }
}
