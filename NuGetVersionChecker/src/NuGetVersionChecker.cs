using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace NuGetVersionChecker.src
{
    /// <summary>
    /// 
    /// </summary>
    public class NugetVersionChecker
    {
        /// <summary>
        /// 
        /// </summary>
        public class Package
        {
            /// <summary>
            /// 
            /// </summary>
            public string Name;

            /// <summary>
            /// 
            /// </summary>
            public Version Version;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="name"></param>
            /// <param name="version"></param>
            public Package(string name, Version version)
            {
                Name = name;
                Version = version;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">D:/repos/WhatColors/WhatColors/WhatColors.csproj</param>
        /// <returns>List of Packages.</returns>
        public static List<Package> GetPackages(string path)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(path);

            XmlNodeList packageReferences = xmldoc?.DocumentElement?.SelectNodes("/Project/ItemGroup/PackageReference");

            List<Package> packages = new List<Package>();

            if (packageReferences == null || packageReferences.Count == 0)
            {
                return packages;
            }

            foreach (XmlNode item in packageReferences)
            {
                if (item == null)
                {
                    continue;
                }

                if (item.Attributes == null)
                {
                    continue;
                }

                XmlAttribute package = item.Attributes["Include"];
                if (package == null)
                {
                    continue;
                }

                var version = item.Attributes["Version"];
                if (version == null)
                {
                    continue;
                }

                Version.TryParse(version.Value, out Version versionParseResult);

                if (versionParseResult == null)
                {
                    continue;
                }

                packages.Add(new Package(name: package.Value, version: versionParseResult));
            }

            return packages;
        }

        internal static readonly ILogger s_logger = NullLogger.Instance;
        internal static readonly CancellationToken s_cancellationToken = CancellationToken.None;

        //static readonly SourceCacheContext s_cache = new SourceCacheContext();
        internal static readonly SourceRepository s_repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packageName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<Package> GetPackageFromNuget(string packageName)
        {
            PackageSearchResource resource = await s_repository.GetResourceAsync<PackageSearchResource>();
            SearchFilter searchFilter = new SearchFilter(includePrerelease: true);

            //
            IEnumerable<IPackageSearchMetadata> results = await resource.SearchAsync(
            packageName,
            searchFilter,
            skip: 0,
            take: 1,
            s_logger,
            s_cancellationToken);

            IPackageSearchMetadata result = results.First();

            VersionInfo versionInfo = result.GetVersionsAsync().GetAwaiter().GetResult().OrderByDescending(p => p.Version).FirstOrDefault();

            if (versionInfo == null)
            {
                throw new Exception("Error");
            }

            return new Package(name: packageName, version: versionInfo.Version.Version);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packageNameList"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<List<Package>> GetPackagesFromNuget(List<string> packageNameList)
        {
            PackageSearchResource resource = await s_repository.GetResourceAsync<PackageSearchResource>();
            SearchFilter searchFilter = new SearchFilter(includePrerelease: true);

            List<Package> packageList = new List<Package>();

            foreach (string package in packageNameList)
            {
                //
                IEnumerable<IPackageSearchMetadata> results = await resource.SearchAsync(
                package,
                searchFilter,
                skip: 0,
                take: 1,
                s_logger,
                s_cancellationToken);

                IPackageSearchMetadata result = results.First();

                VersionInfo versionInfo = result.GetVersionsAsync().GetAwaiter().GetResult().OrderByDescending(p => p.Version).FirstOrDefault();

                if (versionInfo == null)
                {
                    throw new Exception("Error");
                }

                packageList.Add(new Package(name: package, version: versionInfo.Version.Version));
            }

            return packageList;
        }
    }
}