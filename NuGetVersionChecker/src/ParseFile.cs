using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;

namespace NuGetVersionChecker
{
    /// <summary>
    /// Parsing files to get package informations.
    /// </summary>
    public partial class NuGetVersionChecker
    {
        /// <summary>
        /// Returns list of package names and their versions by provided .csprof file. Get nodes named "PackageReference". Uses <seealso cref="XmlDocument"/> to parse data.
        /// </summary>
        /// <param name="path">Path of .csproj file.</param>
        /// <returns>List of Packages.</returns>
        public static List<Package> GetPackages(string path)
        {
            // Creating list of empty packages.
            List<Package> packages = new List<Package>();

            // Checking if path is null or empty.
            if (string.IsNullOrEmpty(path))
            {
                return packages;
            }

            try
            {
                // Creating instance of XmlDocument.
                XmlDocument xmldoc = new XmlDocument();

                // Loading file with specified path.
                xmldoc.Load(path);

                // Getting nodes with names as "PackageReference".
                XmlNodeList packageReferences = xmldoc?.DocumentElement?.SelectNodes("/Project/ItemGroup/PackageReference");

                // Checking package if packageRefenrences is null or empty.
                if (packageReferences == null || packageReferences.Count == 0)
                {
                    // Returning empty list of packages.
                    return packages;
                }

                // Loop for every node.
                foreach (XmlNode xmlNode in packageReferences)
                {
                    // Checking if node is null.
                    if (xmlNode == null)
                    {
                        continue;
                    }

                    // Checking if attribute is null.
                    if (xmlNode.Attributes == null)
                    {
                        continue;
                    }

                    // Checking if attribute with "Include" exists.
                    XmlAttribute xmlAttribute = xmlNode.Attributes["Include"];
                    if (xmlAttribute == null)
                    {
                        continue;
                    }

                    // Checking if attrbute with "Version" exists.
                    var version = xmlNode.Attributes["Version"];
                    if (version == null)
                    {
                        continue;
                    }

                    // Parsion string version into SemanticVersion.
                    bool parseResult = SemanticVersion.TryParse(version.Value, out SemanticVersion versionParseResult);
                    if (parseResult == false)
                    {
                        Debug.WriteLine($"Version couldn't parsed for {version.Value}");

                        continue;
                    }

                    // Checking if version parsing result is null.
                    if (versionParseResult == null)
                    {
                        Debug.WriteLine($"Version couldn't parsed for {version.Value}");

                        continue;
                    }

                    // Adding package into the returning list.
                    packages.Add(new Package(name: xmlAttribute.Value, version: versionParseResult));
                }
            }
            catch (Exception)
            {
                Debug.WriteLine($"NuGetVersionChecker: Path is not found: {path} or nodes could not load.");
            }

            // Returning list of packages.
            return packages;
        }

       /// <summary>
       /// Gets the full path of the current working directory of the application.
       /// </summary>
       /// <remarks>The current directory is the directory from which the process is started, unless it is
       /// changed by the application. The value may differ between threads or after calling methods that change the
       /// current directory, such as System.IO.Directory.SetCurrentDirectory. The returned path does not end with a
       /// directory separator character.</remarks>
        public static string CurrentDirectory => System.IO.Directory.GetCurrentDirectory();

        /// <summary>
        /// Gets the full path of the startup directory of the application, which is the parent directory of the current working directory. It assumes that the .csproj file is located three levels up from the current directory.   
        /// </summary>
        public static string StartupPath => Directory.GetParent(CurrentDirectory).Parent.Parent.FullName;

        /// <summary>
        /// Gets the name of the application, which is derived from the entry assembly's name. It assumes that the .csproj file has the same name as the application.
        /// </summary>
        public static string AppName => Assembly.GetEntryAssembly().GetName().Name;

        /// <summary>
        /// Constructs the full file path to the .csproj file based on the startup directory and application name.
        /// </summary>
        /// <returns>The full file path to the .csproj file.</returns>
        /// <returns>Exact path of .csproj file.</returns>
        public static string GetFilePath()
        {
            // Returning path of .csproj file.
            return $"{StartupPath}\\{AppName}.csproj";
        }
    }
}