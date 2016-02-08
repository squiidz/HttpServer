using System;
using System.Net;
using Bone.Generate;
using Bone.Router;

namespace Bone.HttpServer
{
    public class HttpServer
    {
        public Mux Mux = null;
        public string Host;
        private HttpListener Listener;

        public HttpServer(string host, Mux mux)
        {
            this.Mux = mux;
            this.Host = host;
            this.Listener = new HttpListener();
            Listener.Prefixes.Add(Host);
        }
        public void Listen()
        {
            Listener.Start();
            while (Listener.IsListening)
            {
                var ctx = Listener.GetContext();
                Mux.HandleRoute(ctx);
                ctx.Response.Close();
            }
        }
    }

    public class ResponseHandler
    {
        static public byte[] Build(string data)
        {
            return System.Text.Encoding.UTF8.GetBytes(data);
        }
        static public void Write(HttpListenerContext ctx, string data)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(data);
            ctx.Response.ContentLength64 = buffer.Length;
            System.IO.Stream output = ctx.Response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
        }
    }
}