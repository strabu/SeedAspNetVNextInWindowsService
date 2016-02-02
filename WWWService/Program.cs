using System;
using System.Diagnostics;
using System.Reflection;
using System.ServiceProcess;
using System.Linq;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;

namespace WWWServiceService
{    
    public class Program: ServiceBase
    {
        private readonly EventLog _log = new EventLog("Application") { Source = "Application" };         
        IConfigurationRoot Configuration;
        
        public Program(IHostingEnvironment env)
        {
            Log("WWWService .ctor");
        }
 
        public void ConfigureServices(IServiceCollection services)
        {
            Log("WWWService ConfigureServices()");
            services.AddMvc();                   
        }
               
        public void Configure(IApplicationBuilder app)
        {   
            Log("WWWService Configure()");           
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();            
        }            
              
        public void Main(string[] args)
        {            
            Log("started with args: "+String.Join(" ", args));
                       
            var configBuilder = new ConfigurationBuilder();             
                configBuilder.AddJsonFile("appsettings.json");
                                                  
            Configuration = configBuilder.Build();
                        
            if (args.Contains("--windows-service"))
            {
                try
                {                    
                    Log("WWWService Main()");
                    Run(this); 
                    return;   
                }
                catch(Exception x)
                {                    
                    Log("WWWService Main() Exception: "+ x.Message);
                }
            }
            
            OnStart(null);
            Console.ReadLine();
            OnStop();                      
        }
        
        protected override void OnStart(string[] args)
        {
            Log("WWWService started.");           
            try
            {                                       
                var builder = new WebHostBuilder(Configuration);                                                                                                
                builder.UseServer("Microsoft.AspNet.Server.WebListener");                
                builder.UseStartup<Program>();                                                
                var appBuilder = builder.Build();                                                                                       
                appBuilder.Start();
            }
            catch(Exception x)
            {
                Log("WWWService Exception: "+x.Message);
                if (x.InnerException != null)
                    Log("WWWService Exception: "+x.InnerException.Message);    
                    
                if (x is System.Reflection.ReflectionTypeLoadException)
                {
                    var typeLoadException = x as ReflectionTypeLoadException;
                    var loaderExceptions  = typeLoadException.LoaderExceptions;
                    foreach (var l in loaderExceptions)
                    {
                        Log("WWWService TypeLoadException: "+l.Message);                            
                    }
                }            
            }
            
        }
 
        protected override void OnStop()
        {
            Log("WWWService stopping...");            
            Log("WWWService stopped.");
        }
        
        private void Log(string msg)
        {
            Console.WriteLine(msg);
            _log.WriteEntry(msg);
        }
    }
}