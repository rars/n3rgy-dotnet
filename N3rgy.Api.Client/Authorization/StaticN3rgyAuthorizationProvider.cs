namespace N3rgy.Api.Client.Authorization;

public class StaticN3rgyAuthorizationProvider : IN3rgyAuthorizationProvider
{
    private readonly string _authorization;

    public StaticN3rgyAuthorizationProvider(string authorization)
    {
        _authorization = authorization;
    }

    public Task<string> GetAuthorization(CancellationToken cancellationToken = default) => Task.FromResult(_authorization);
}
