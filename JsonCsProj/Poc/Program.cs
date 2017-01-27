using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JsonCsProj.Poc
{
    class Program
    {
         
        static void Main(string[] args)
        {
            var jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var json = File.ReadAllText("_project.json");//

            var source = JsonConvert.DeserializeObject<ProjectJson>(json,jsonSerializerSettings);

            var destination = new CsProj();

            destination.Sdk = "Microsoft.NET.Sdk";
            destination.ToolsVersion = "15.0";
            destination.PropertyGroups.Add(new PropertyGroup { Version = source.Version.Replace("-*","") });
            destination.PropertyGroups.Add(new PropertyGroup {
                Authors = string.Join(";", source.Authors),
                Company = source.Company,
                NeutralLanguage = source.Language,
                AssemblyTitle = source.Title,
                Description = source.Description,
                Copyright = source.Copyright,
                UserSecretsId = source.UserSecretsId
            });
            destination.PropertyGroups.Add(new PropertyGroup
            {
                TargetFrameworks = string.Join(";", source.Frameworks.Select(f=>f.Key).ToArray())
            });

           
            
            

            destination.ItemGroups.Add(parseDependencies(source.Dependencies));

            var frameworkDependencies = new ItemGroup();
            foreach (var framework in source.Frameworks)
            {
                var depGroup = parseDependencies(framework.Value.Dependencies);
                depGroup.Condition = $"'$(TargetFramework)'=='{framework.Key}'";
                destination.ItemGroups.Add(depGroup);
            }

            destination.PropertyGroups.Add(new PropertyGroup
            {
                RuntimeIdentifiers = string.Join(";", source.Runtimes.Keys.ToArray())
            });

            destination.ItemGroups.Add(parseTools(source.Tools));

            if (source.BuildOptions != null)
            {
                var buildGroup = new PropertyGroup();
                if (source.BuildOptions.EmitEntryPoint.HasValue)
                {
                    buildGroup.OutputType = source.BuildOptions.EmitEntryPoint.Value ? "Exe" : "Library";
                }
                if (!String.IsNullOrEmpty(source.BuildOptions.KeyFile))
                {
                    buildGroup.AssemblyOriginatorKeyFile = source.BuildOptions.KeyFile;
                    buildGroup.SignAssembly = "true";
                    buildGroup.PublicSign = new PublicSign
                    {
                        Condition = "'$(OS)' != 'Windows_NT'",
                        Value = true
                    };
                    
                }
                buildGroup.WarningsAsErrors = source.BuildOptions.WarningsAsErrors?.ToString().ToLower();
                buildGroup.NoWarn = string.Join(";", source.BuildOptions.Nowarn);
                buildGroup.GenerateDocumentationFile = source.BuildOptions.XmlDoc?.ToString().ToLower();
                buildGroup.PreserveCompliationContext = source.BuildOptions.PreserveCompilationContext?.ToString().ToLower();
                buildGroup.AssemblyName = source.BuildOptions.OutputName;
                buildGroup.OutputType = source.BuildOptions.DebugType;
                buildGroup.AllowUnsafeBlocks = source.BuildOptions.AllowUnsafe?.ToString().ToLower();
                buildGroup.DefineConstants = string.Join(";", source.BuildOptions.Define);

                destination.PropertyGroups.Add(buildGroup);
            }

            if (source.PackOptions != null)
            {
                var packGroup = new PropertyGroup();

                packGroup.Summary = source.PackOptions.Summary;
                packGroup.PackageTags = string.Join(";", source.PackOptions.Tags);
                packGroup.PackageReleaseNotes = source.PackOptions.ReleaseNotes;
                packGroup.PackageIconUrl = source.PackOptions.IconUrl;
                packGroup.PackageProjectUrl = source.PackOptions.ProjectUrl;
                packGroup.PackageLicenseUrl = source.PackOptions.LicenseUrl;
                packGroup.PackageRequireLicenseAcceptance = source.PackOptions.RequireLicenseAcceptance?.ToString().ToLower();
                packGroup.RepositoryType = source.PackOptions.Repository?.Type;
                packGroup.RepositoryUrl = source.PackOptions.Repository?.Url;
                
                destination.PropertyGroups.Add(packGroup);
            }


            if (source.Scripts != null)
            {
                if (source.Scripts.Precompile != null)
                {
                    var preCompTarget = new Target
                    {
                        Name = source.Name + "PreCompileTarget",
                        BeforeTargets = "Build"

                    };

                    if (source.Scripts.Precompile is string)
                    {
                        preCompTarget.Execs.Add(new Exec { Command = source.Scripts.Precompile.ToString() });
                    }
                    else
                    {
                        var commands = JsonConvert.DeserializeObject<string[]>(source.Scripts.Precompile.ToString());
                        preCompTarget.Execs.AddRange(commands.Select(e => new Exec { Command = e }));
                    }

                    destination.Targets.Add(preCompTarget);
                }
                if (source.Scripts.Postpublish != null)
                {
                    var target = new Target
                    {
                        Name = source.Name + "PostPublishTarget",
                        BeforeTargets = "Publish"

                    };

                    if (source.Scripts.Postpublish is string)
                    {
                        target.Execs.Add(new Exec { Command = source.Scripts.Precompile.ToString() });
                    }
                    else
                    {
                        var commands = JsonConvert.DeserializeObject<string[]>(source.Scripts.Postpublish.ToString());
                        target.Execs.AddRange(commands.Select(e => new Exec { Command = e }));
                    }

                    destination.Targets.Add(target);
                }

            }

            //files: work in progress
            //var fileGroup = new ItemGroup();

            //if (source.BuildOptions?.Compile?.CopyToOutput != null)
            //{
            //    fileGroup.NoneFiles.Add(new FileItem
            //    {
            //        Include = source.BuildOptions.Compile.CopyToOutput
            //     //   CopyToOutputDirectory = "Always",
            //    };
            //}



            //

           //testRunner: todo










            var serializer = new XmlSerializer(destination.GetType());
            var xns = new XmlSerializerNamespaces();
            xns.Add(string.Empty, string.Empty);
            var xmlName = $@"{Environment.CurrentDirectory}\..\..\{source.Name}.csproj";
            File.Delete(xmlName);
            using (var fs = new FileStream(xmlName, FileMode.CreateNew))
            {
                serializer.Serialize(fs, destination,xns);
            }
        }

        private static ItemGroup parseDependencies(Dictionary<string, object> dependencies)
        {
            var depGroup = new ItemGroup();
            foreach (var dep in dependencies)
            {



                if (dep.Value is string)
                {

                    depGroup.PackageReferences.Add(new PackageReference { Include = dep.Key, Version = dep.Value.ToString().Replace("-*", "") });

                }
                else
                {
                    var detailDep = JsonConvert.DeserializeObject<DependencyDetail>(dep.Value.ToString());
                    if (detailDep.Type == "project")
                    {
                        depGroup.ProjectReferences.Add(new ProjectReference { Include = dep.Key });

                    }
                    else if (detailDep.Type == "build")
                    {
                        depGroup.PackageReferences.Add(new PackageReference
                        {
                            Include = dep.Key,
                            Version = detailDep.Version.Replace("-*", ""),
                            PrivateAssets = "All"
                        });

                    }

                }
            }
            return depGroup;
        }

        private static ItemGroup parseTools(Dictionary<string, string> tools)
        {
            var toolGroup = new ItemGroup();
            foreach (var tool in tools)
            {
                
                    toolGroup.DotNetCliToolReferences.Add(new DotNetCliToolReference { Include = tool.Key, Version = tool.Value.ToString().Replace("-*", "") });

            }
            return toolGroup;
        }
    }
}
