using System;
using System.Net;
using System.Collections.Generic;

namespace Bone.Router
{
    public class Mux
    {
        public delegate void Handler(HttpListenerContext ctx);
        public struct Route
        {
            public string Method;
            public string Path;
            public Handler Handler;
            public Route(string method, string path, Handler handler)
            {
                this.Method = method;
                this.Path = path;
                this.Handler = handler;
            }
        }
        public List<Route> Routes = new List<Route>();
        public Mux() { }
        public void Add(string method, string path, Handler handler)
        {
            Routes.Add(new Route(method, path, handler));
        }
        public void Get(string path, Handler handler)
        {
            Routes.Add(new Route("GET", path, handler));
        }
        public void Post(string path, Handler handler)
        {
            Routes.Add(new Route("POST", path, handler));
        }
        public void Put(string path, Handler handler)
        {
            Routes.Add(new Route("PUT", path, handler));
        }
        public void Update(string path, Handler handler)
        {
            Routes.Add(new Route("UPDATE", path, handler));
        }
        public void Delete(string path, Handler handler)
        {
            Routes.Add(new Route("DELETE", path, handler));
        }
        public void Patch(string path, Handler handler)
        {
            Routes.Add(new Route("PATCH", path, handler));
        }
        public void Head(string path, Handler handler) {
            Routes.Add(new Route("HEAD", path, handler));
        }
        public void HandleRoute(HttpListenerContext ctx)
        {
            var uri = ctx.Request.Url.LocalPath;
            Console.WriteLine(string.Format("Handle [{0}] request from {1} at {2}",
                ctx.Request.HttpMethod,
                ctx.Request.UserHostAddress,
                ctx.Request.Url.LocalPath));
            foreach (var r in Routes)
            {
                if (r.Path == uri && r.Method == ctx.Request.HttpMethod)
                {
                    r.Handler(ctx);
                }
            }
        }
    }
}