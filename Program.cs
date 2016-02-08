using System;
using System.Net;
using Bone.HttpServer;
using Bone.Router;

namespace HttpMux
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Instance of Bone.Mux()
            var router = new Mux();
            // Route /index
            router.Get("/index", (HttpListenerContext ctx) => {
               ResponseHandler.Write(ctx, "Hello " + ctx.Request.UserHostName.ToString());
            });
            // Route /index/test
            router.Post("/index/test", (HttpListenerContext ctx) => {
                byte[] body = new byte[]{};
                ctx.Request.InputStream.Read(body, 0, (int)ctx.Request.Length);
                Console.WriteLine(body);
            });
            // Route /home
            router.Get("/home", (HttpListenerContext ctx) => {
                ResponseHandler.Write(ctx, "Hello from the other side");
            });
            router.Get("/home/:var", (HttpListenerContext ctx) => {
               ResponseHandler.Write(ctx, string.Format("You Send {0}", ctx.Request.QueryString.Get(":var"))); 
            });
            // Start Bone.HttpServer()
            var server = new HttpServer("http://localhost:8080/", router);
            server.Listen();
        }
    }
}
