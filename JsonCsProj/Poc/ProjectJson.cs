using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonCsProj.Poc
{
    public class FrameworkDetail
    {
        public Dictionary<string, object> Dependencies { get; set; }
        public string[] Imports { get; set; }
    }


    public class DependencyDetail
    {
        public string Version { get; set; }
        public string Type { get; set; }
    }

    public class BuildDetail
    {
        public bool? EmitEntryPoint { get; set; }
        public string KeyFile { get; set; }
        public bool? WarningsAsErrors { get; set; }
        public string[] Nowarn { get; set; }
        public bool? XmlDoc { get; set; }
        public bool? PreserveCompilationContext { get; set; }
        public string OutputName { get; set; }
        public string DebugType { get; set; }
        public bool? AllowUnsafe { get; set; }
        public string[] Define { get; set; }
        public FileDetail Compile { get; set; }
        public FileDetail Embed { get; set; }
    }


    public class FileDetail
    {
        
        public object Include { get; set; }
       
        public object Exclude { get; set; }
        
        public string CopyToOutput { get; set; }
       
        
    }


    public class PackOptions
    {
        public string Summary { get; set; }
        public string[] Tags { get; set; }
        public string[] Owners { get; set; }
        public string ReleaseNotes { get; set; }
        public string IconUrl { get; set; }
        public string ProjectUrl { get; set; }
        public string LicenseUrl { get; set; }
        public bool? RequireLicenseAcceptance { get; set; }
        public PackRepository Repository { get; set; }
        public object Include { get; set; }
        public Dictionary<string,string> Mappings { get; set; }
    }

    public class PackRepository
    {
        public string Type { get; set; }
        public string Url { get; set; }
    }

    public class PublishOptions
    {
        public string[] Include { get; set; }
    }

    public class Scripts
    {
        public object Precompile { get; set; }
        public object Postpublish { get; set; }
    }


    public class ProjectJson
    {
        public string Name { get; set; }
        public string Version { get; set; }

        public string[] Authors { get; set; }
        public string Company { get; set; }
        public string Language { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }
        public string UserSecretsId { get; set; }
        public Dictionary<string, FrameworkDetail> Frameworks { get; set; }
        public Dictionary<string, object> Dependencies { get; set; }
        public Dictionary<string, object> Runtimes { get; set; }
        public Dictionary<string, string> Tools { get; set; }
        public BuildDetail BuildOptions { get; set; }
        public PackOptions PackOptions { get; set; }
        public PublishOptions PublishOptions { get; set; }
        public Scripts Scripts { get; set; }
        public string TestRunner { get; set; }
    }
}
