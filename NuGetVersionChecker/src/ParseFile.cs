using System;
using System.Collections.Generic;
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
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(path);

            XmlNodeList packageReferences = xmldoc?.DocumentElement?.SelectNodes("/Project/ItemGroup/PackageReference");

            List<Package> packages = new List<Package>();

            // Checking package if packageRefenrences is null or empty.
            if (packageReferences == null || packageReferences.Count == 0)
            {
                return packages;
            }

            // Loop for every node.
            foreach (XmlNode xmlNode in packageReferences)
            {
                if (xmlNode == null)
                {
                    continue;
                }

                if (xmlNode.Attributes == null)
                {
                    continue;
                }

                XmlAttribute package = xmlNode.Attributes["Include"];
                if (package == null)
                {
                    continue;
                }

                var version = xmlNode.Attributes["Version"];
                if (version == null)
                {
                    continue;
                }

                bool parseResult = Version.TryParse(version.Value, out Version versionParseResult);
                if (parseResult == false)
                {
                    continue;
                }

                if (versionParseResult == null)
                {
                    continue;
                }

                packages.Add(new Package(name: package.Value, version: versionParseResult));
            }

            return packages;
        }
    }
}