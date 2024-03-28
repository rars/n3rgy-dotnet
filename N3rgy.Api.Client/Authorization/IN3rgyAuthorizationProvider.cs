namespace N3rgy.Api.Client.Authorization;

public interface IN3rgyAuthorizationProvider
{
    public Task<string> GetAuthorization(CancellationToken cancellationToken);
}
