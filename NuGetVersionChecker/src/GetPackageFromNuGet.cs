using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

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

        /// <summary>
        /// Returns a package by making search on NuGet.
        /// </summary>
        /// <param name="packageName">Name of package.</param>
        /// <param name="includePrerelease">Include Prerelease version to compare versions.</param>
        /// <returns>Returns a package.</returns>
        public static async Task<Package> GetPackageFromNuGetAsync(string packageName, bool includePrerelease = true)
        {
            // Quick check for network availability to avoid unnecessary network calls when offline.
            if (NetworkInterface.GetIsNetworkAvailable() == false)
            {
                return new Package();
            }

            try
            {
                PackageSearchResource resource = await s_repository.GetResourceAsync<PackageSearchResource>();
                SearchFilter searchFilter = new SearchFilter(includePrerelease: includePrerelease);

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
                return new Package();
                }

                // Get version info of list of the package.
                IEnumerable<VersionInfo> versionInfoList = await package.GetVersionsAsync();

                // Checking if versionInfoList is null or empty.
                if (versionInfoList == null || versionInfoList.Any() == false)
                {
                return new Package();
                }

                // Get latets version of the list of versions.
                VersionInfo versionInfo = versionInfoList.OrderByDescending(p => p.Version).First();

                // Returning a Package via creating with its constructor.
                return new Package(name: package.Identity.Id, version: versionInfo.Version);
            }
            catch
            {
                // If any network or runtime error occurs, return empty package.
                return new Package();
            }
        }

        /// <summary>
        /// Returns list of packages by making search on NuGet.
        /// </summary>
        /// <param name="packageNameList">List of package names.</param>
        /// <param name="includePrerelease">Include Prerelease version to compare versions.</param>
        /// <returns>List of packages.</returns>
        public static async Task<List<Package>> GetPackagesFromNuGetAsync(List<string> packageNameList, bool includePrerelease = true)
        {
            // Return value.
            List<Package> packageList = new List<Package>();

            // Quick check for network availability to avoid unnecessary network calls when offline.
            if (NetworkInterface.GetIsNetworkAvailable() == false)
            {
                return packageList;
            }

            try
            {
                PackageSearchResource resource = await s_repository.GetResourceAsync<PackageSearchResource>();
                SearchFilter searchFilter = new SearchFilter(includePrerelease: includePrerelease);

                
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
                        continue;
                    }

                    // Get latest version info of newest version of the package.
                    IEnumerable<VersionInfo> versionInfoList = await package.GetVersionsAsync();

                    // Checking if versionInfoList is null or empty.
                    if (versionInfoList == null || versionInfoList.Any() == false)
                    {
                        continue;
                    }

                    // Get latest version of the list of versions.
                    VersionInfo versionInfo = versionInfoList.OrderByDescending(p => p.Version).First();

                    // Checking if versionInfo is null.
                    if (versionInfo == null)
                    {
                        continue;
                    }

                    // Adding the Package via creating with its constructor.
                    packageList.Add(new Package(name: package.Identity.Id, version: versionInfo.Version));
                }

                // Retuning list of packages as return data.
                return packageList;
            }
            catch
            {
                // If any network or runtime error occurs, return list of empty packages.
                return packageList;
            }
        }
    }
}