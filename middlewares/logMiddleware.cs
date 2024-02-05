using System.Diagnostics;

public class logMiddleware{
    private RequestDelegate next;

    private ILogger<logMiddleware> logger;

    public logMiddleware(RequestDelegate next,ILogger<logMiddleware> logger)
    {
        this.next=next;
        this.logger=logger;
    }
    public async Task Invoke(HttpContext c)
    {
        var sw = new Stopwatch();
        sw.Start();
        await next(c);
        sw.Stop();

        var requestTime = DateTime.Now;
        string message =$"{requestTime} - {c.Request.Path}.{c.Request.Method} took {sw.ElapsedMilliseconds}ms.User: {c.User?.FindFirst("userId")?.Value ?? "unknown"}"; 
             using (var writer = new StreamWriter("log.txt", true))
            {
                writer.WriteLine(message);
            }    
    }        

}
public static partial class OurMiddleExtensions
{
    public static IApplicationBuilder UselogMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<logMiddleware>();
    }
}