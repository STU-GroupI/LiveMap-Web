namespace LiveMapDashboard.Web.Extensions;

public static class HttpContextExtensions
{
    private const string COOKIE_NAME = "SelectedMapId";
    public static Guid? GetCurrentMapId(this HttpContext context)
    {
        /*I have switched these statements as the second statement could only be applicable in the next request, explaining why the mapId
         * would never update instantly and refreshes were needed*/
        if (context.Items.TryGetValue(COOKIE_NAME, out object? obj) && Guid.TryParse(obj as string, out var itemsId))
        {
            return itemsId;
        }
        else if (context.Request.Cookies.TryGetValue(COOKIE_NAME, out var value) && Guid.TryParse(value, out var cookieId))
        {
            return cookieId;
        }

        return null;
    }

    public static void SetSelectedMapId(this HttpContext context, Guid mapId)
    {
        context.Items.TryAdd(COOKIE_NAME, mapId.ToString());
        context.Response.Cookies.Append(COOKIE_NAME, mapId.ToString(), new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
            //IsEssential = true // important for GDPR compliance
        });
    }

    public static bool ContainsSelectedMapId(this HttpContext context)
    {
        return context.Request.Cookies.ContainsKey(COOKIE_NAME);
    }
}
