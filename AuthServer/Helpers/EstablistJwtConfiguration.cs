using Microsoft.IdentityModel.Tokens;

namespace AuthServer.Helpers;

public class EstablishJwtConfiguration
{
    private static readonly string DefaultFile = Path.Combine("oidc-assets", ".private", "jwk.json");
    private static JsonWebKey? defaultJwk = null;
    private static SpinLock spinLock = new SpinLock();

    private static JsonWebKey LoadFromFile(string file)
    {
        var fi = new FileInfo(file);
        if (!fi.Exists) throw new FileNotFoundException(file);
        using var reader = fi.OpenText();
        var json = reader.ReadToEnd();

        return new JsonWebKey(json);
    }

    public static JsonWebKey LoadFromDefault()
    {
        bool lockTaken = false;

        spinLock.Enter(ref lockTaken);
        if (lockTaken)
        {
            defaultJwk ??= LoadFromFile(DefaultFile);
            spinLock.Exit();
        }
        else
        {
            throw new InvalidOperationException(); // this will never happen I hope
        }

        return defaultJwk;
    }
}