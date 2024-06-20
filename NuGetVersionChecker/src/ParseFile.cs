using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            // Creating instance of XmlDocument.
            XmlDocument xmldoc = new XmlDocument();

            // Creating list of empty packages.
            List<Package> packages = new List<Package>();

            try
            {
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
    }
}