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
            public string[] Tokens;
            public int TokenLength;
            public int VarPos;
            public Handler Handler;
            
            public string variables;
            public Route(string method, string path, Handler handler)
            {
                Method = method;
                Path = path;
                Handler = handler;
                Tokens = Path.Split('/');
                TokenLength = Tokens.Length;
                variables = "";
                VarPos = 0;
                for (var i = 0; i < TokenLength; i++) {
                    if (Tokens[i].Contains(":")) {
                        VarPos = i;
                    }
                }
                Console.WriteLine(string.Format("{0} Route Saved.", Path));
            }
            public void Parse() {

            }
        }
        public List<Route> Routes = new List<Route>();
        public Mux() { }
        public void Add(string method, string path, Handler handler)
        {
            var route = new Route(method, path, handler);
            route.Parse();
            Routes.Add(route);
        }
        public void Get(string path, Handler handler)
        {
            Add("GET", path, handler);
        }
        public void Post(string path, Handler handler)
        {
            Add("POST", path, handler);
        }
        public void Put(string path, Handler handler)
        {
            Add("PUT", path, handler);
        }
        public void Update(string path, Handler handler)
        {
            Add("UPDATE", path, handler);
        }
        public void Delete(string path, Handler handler)
        {
            Add("DELETE", path, handler);
        }
        public void Patch(string path, Handler handler)
        {
            Add("PATCH", path, handler);
        }
        public void Head(string path, Handler handler) {
            Add("HEAD", path, handler);
        }
        public void HandleRoute(HttpListenerContext ctx)
        {
            var uri = ctx.Request.Url.LocalPath;
            Console.WriteLine(string.Format("Handle [{0}] request from {1} at {2}",
                ctx.Request.HttpMethod,
                ctx.Request.UserHostAddress,
                ctx.Request.Url.LocalPath));
                
            var uriTokens = uri.Split('/');
            
            foreach (var r in Routes)
            {
                if (r.TokenLength == uriTokens.Length && r.Method == ctx.Request.HttpMethod)
                {
                    if (r.VarPos != 0) {
                        var id = Routes.IndexOf(r);
                        var sr = Routes[id];
                        sr.variables = uriTokens[r.VarPos];
                        ctx.Request.QueryString.Add(r.Tokens[r.VarPos], sr.variables);
                    }
                    r.Handler(ctx);
                }
            }
        }
    }
}