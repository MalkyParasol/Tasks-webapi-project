using Microsoft.AspNetCore.Mvc.Controllers;
using System.Diagnostics;

public class logMiddleware{
    private RequestDelegate next;

    private ILogger<logMiddleware> logger;

    private string fileName;

    

    public logMiddleware(RequestDelegate next,ILogger<logMiddleware> logger)
    {
        this.next=next;
        this.logger=logger;
        fileName = Path.Combine("Data", "Loggers/log.txt");
    }
    public async Task Invoke(HttpContext c)
    {
        var sw = new Stopwatch();
        sw.Start();
        await next(c);
        sw.Stop();

        var endpoint = c.GetEndpoint();
        string controllerName = endpoint?.Metadata?.GetMetadata<ControllerActionDescriptor>()?.ControllerName ?? "Unknown";

        var requestTime = DateTime.Now;
        string message =$"{requestTime} - url: {c.Request.Path} | controller: {controllerName} | method: {c.Request.Method} | duration time:  {sw.ElapsedMilliseconds}ms | User Name : {c.User?.FindFirst("userName")?.Value ?? "unknown"}";
        //using (StreamWriter writer = new StreamWriter("Loggers/log.txt", true))
        //{
        //    writer.WriteLine(message);
        //    writer.Close();
        //}

        File.WriteAllText(fileName, message);
       
    }        

}
public static partial class OurMiddleExtensions
{
    public static IApplicationBuilder UselogMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<logMiddleware>();
    }
}