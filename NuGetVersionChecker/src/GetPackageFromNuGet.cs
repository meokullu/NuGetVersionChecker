using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NuGetVersionChecker
{
    /// <summary>
    /// Get package/s information from NuGet.
    /// </summary>
    public partial class NuGetVersionChecker
    {
        #region Static

        private static readonly ILogger s_logger = NullLogger.Instance;
        private static readonly CancellationToken s_cancellationToken = CancellationToken.None;
        private static readonly SourceRepository s_repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");

        #endregion Static

        #region Temporary obsolote method due to naming mistake on async method.

        /// <summary>
        /// Obsolote method due to naming mistake.
        /// </summary>
        /// <param name="packageName"></param>
        /// <returns></returns>
        [Obsolete("Use GetPackageFromNugetAsync(), sorry for naming mistake.", error: false)]
        public static async Task<Package> GetPackageFromNuget(string packageName)
        {
            return await GetPackageFromNuGetAsync(packageName: packageName);
        }

        /// <summary>
        /// Obsolote method due to naming mistake.
        /// </summary>
        /// <param name="packageNameList"></param>
        /// <returns></returns>
        [Obsolete("Use GetPackagesFromNugetAsync(), sorry for naming mistake.", error: false)]
        public static async Task<List<Package>> GetPackagesFromNuget(List<string> packageNameList)
        {
            return await GetPackagesFromNuGetAsync(packageNameList: packageNameList);
        }

        #endregion Temporary obsolote method due to naming mistake on async method.

        /// <summary>
        /// Returns a package by making search on NuGet.
        /// </summary>
        /// <param name="packageName">Name of package.</param>
        /// <returns>Returns a package.</returns>
        /// <exception cref="NullReferenceException">Throws NullReferenceException when package is not found via search.</exception>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException when package version is not found via GetVersionsAsync().</exception>
        public static async Task<Package> GetPackageFromNuGetAsync(string packageName)
        {
            PackageSearchResource resource = await s_repository.GetResourceAsync<PackageSearchResource>();
            SearchFilter searchFilter = new SearchFilter(includePrerelease: true);

            // Making a search with taking one package.
            IEnumerable<IPackageSearchMetadata> searchResults = await resource.SearchAsync(
            packageName,
            searchFilter,
            skip: 0,
            take: 1,
            s_logger,
            s_cancellationToken);

            // Getting only one value from the list. Since it takes only 1, it is "Single" instead of "First".
            IPackageSearchMetadata package = searchResults.SingleOrDefault();

            // Checking if package is null.
            if (package == null)
            {
                throw new NullReferenceException();
            }

            // Get latest version info of newest version of the package.
            // TODO: Create better approach to use this individually instead of getting only latest one.
            VersionInfo versionInfo = package.GetVersionsAsync().GetAwaiter().GetResult().OrderByDescending(p => p.Version).FirstOrDefault();

            // Checking if versionInfo is null.
            if (versionInfo == null)
            {
                throw new ArgumentNullException();
            }

            // Returning a Package via creating with its constructor.
            return new Package(name: packageName, version: versionInfo.Version.Version);
        }

        /// <summary>
        /// Returns list of packages by making search on NuGet.
        /// </summary>
        /// <param name="packageNameList">List of package names.</param>
        /// <returns>List of packages.</returns>
        /// <exception cref="NullReferenceException">Throws NullReferenceException when package is not found via search.</exception>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException when package version is not found via GetVersionsAsync().</exception>
        public static async Task<List<Package>> GetPackagesFromNuGetAsync(List<string> packageNameList)
        {
            PackageSearchResource resource = await s_repository.GetResourceAsync<PackageSearchResource>();
            SearchFilter searchFilter = new SearchFilter(includePrerelease: true);

            // Return value.
            List<Package> packageList = new List<Package>();

            // Loop for given list of package name.
            foreach (string packageName in packageNameList)
            {
                //
                IEnumerable<IPackageSearchMetadata> searchResults = await resource.SearchAsync(
                packageName,
                searchFilter,
                skip: 0,
                take: 1,
                s_logger,
                s_cancellationToken);

                // Getting only one value from the list. Since it takes only 1, it is "Single" instead of "First".
                IPackageSearchMetadata package = searchResults.SingleOrDefault();

                // Checking if package is null.
                if (package == null)
                {
                    throw new NullReferenceException();
                }

                // Get latest version info of newest version of the package.
                // TODO: Create better approach to use this individually instead of getting only latest one.
                VersionInfo versionInfo = package.GetVersionsAsync().GetAwaiter().GetResult().OrderByDescending(p => p.Version).FirstOrDefault();

                // Checking if versionInfo is null.
                if (versionInfo == null)
                {
                    throw new ArgumentNullException();
                }

                // Adding the Package via creating with its constructor.
                packageList.Add(new Package(name: packageName, version: versionInfo.Version.Version));
            }

            // Retuning list of packages as return data.
            return packageList;
        }
    }
}