using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JsonCsProj.Poc
{
    [XmlRoot("Project")]
    public class CsProj
    {
        public CsProj()
        {
            PropertyGroups = new List<PropertyGroup>();
            ItemGroups  = new List<ItemGroup>();
            Targets = new List<Target>();
        }
        [XmlAttribute]
        public string Sdk { get; set; }
        [XmlAttribute]
        public string ToolsVersion { get; set; }

        [XmlElement("PropertyGroup")]
        public List<PropertyGroup> PropertyGroups { get; set; }
        [XmlElement("ItemGroup")]
        public List<ItemGroup> ItemGroups { get; set; }
        [XmlElement("Target")]
        public List<Target> Targets { get; set; }
    }
    public class ItemGroup
    {
        public ItemGroup()
        {
            PackageReferences = new List<PackageReference>();
            ProjectReferences = new List<ProjectReference>();
            DotNetCliToolReferences = new List<DotNetCliToolReference>();
        }
        [XmlElement("PackageReference")]
        public List<PackageReference> PackageReferences { get; set; }
        [XmlElement("ProjectReference")]
        public List<ProjectReference> ProjectReferences { get; set; }
        [XmlElement("DotNetCliToolReference")]
        public List<DotNetCliToolReference> DotNetCliToolReferences { get; set; }

        [XmlAttribute]
        public string Condition { get; set; }
        [XmlElement("Compile")]
        public FileItem CompileFiles { get; set; }
        [XmlElement("EmbeddedResource")]
        public FileItem EmbeddedFiles { get; set; }
        [XmlElement("Content")]
        public List<FileItem> ContentFiles { get; set; } = new List<FileItem>();
        [XmlElement("None")]
        public List<FileItem> NoneFiles { get; set; } = new List<FileItem>();
    }

    public class PackageReference
    {
        [XmlAttribute]
        public string Include { get; set; }

        [XmlAttribute]
        public string Version { get; set; }

        [XmlAttribute]
        public string PrivateAssets { get; set; }
    }

    public class DotNetCliToolReference
    {
        [XmlAttribute]
        public string Include { get; set; }

        [XmlAttribute]
        public string Version { get; set; }

        
    }

    public class ProjectReference
    {
        [XmlAttribute]
        public string Include { get; set; }

        
    }


    public class PublicSign
    {
        [XmlText]
        public bool Value { get; set; }

        [XmlAttribute]
        public string Condition { get; set; }
    }

    public class Target
    {
        public Target()
        {
            Execs = new List<Exec>();
        }
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string BeforeTargets { get; set; }
        [XmlAttribute]
        public string AfterTargets { get; set; }

        [XmlElement("Exec")]
        public List<Exec> Execs { get; set; }
    }

    public class Exec
    {
        [XmlAttribute]
        public string Command { get; set; }
    }

    public class FileItem
    {
        [XmlAttribute]
        public string Include { get; set; }
        [XmlAttribute]
        public string Exclude { get; set; }
        [XmlAttribute]
        public string PackagePath { get; set; }
        public string Pack { get; set; }
        public string CopyToOutputDirectory { get; set; }
        public string CopyToPublishDirectory { get; set; }
    }


    public class PropertyGroup
    {
        public string Version { get; set; }
        public string Authors { get; set; }
        public string Company { get; set; }
        public string NeutralLanguage { get; set; }
        public string AssemblyTitle { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }
        public string UserSecretsId { get; set; }
        public string TargetFramework { get; set; }
        public string PackageTargetFallback { get; set; }
        public string RuntimeIdentifiers { get; set; }
        public string OutputType { get; set; }
        public string AssemblyOriginatorKeyFile { get; set; }
        public string SignAssembly { get; set; }
        public string WarningsAsErrors { get; set; }
        public string NoWarn { get; set; }
        public string GenerateDocumentationFile { get; set; }
        public string PreserveCompliationContext { get; set; }
        public string AssemblyName { get; set; }
        public string DebugType { get; set; }
        public string AllowUnsafeBlocks { get; set; }
        public string DefineConstants { get; set; }
        public PublicSign PublicSign { get; set; }
        public string Summary { get; set; }
        public string PackageTags { get; set; }
        public string PackageReleaseNotes { get; set; }
        public string PackageProjectUrl { get; set; }
        public string PackageIconUrl { get; set; }
        public string PackageLicenseUrl { get; set; }
        public string PackageRequireLicenseAcceptance { get; set; }
        public string RepositoryType { get; set; }
        public string RepositoryUrl { get; set; }

        

    }


}
