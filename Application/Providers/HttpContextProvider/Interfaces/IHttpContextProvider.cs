namespace Application.Providers.HttpContextProvider.Interfaces
{
    public interface IHttpContextProvider
    {
        Guid GetUserId();

        string GetUserLogin();
    }
}
