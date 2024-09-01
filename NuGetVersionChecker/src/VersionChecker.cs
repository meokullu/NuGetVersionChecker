using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NuGetVersionChecker.NuGetVersionChecker.Package;

namespace NuGetVersionChecker
{
    public partial class NuGetVersionChecker
    {
        /// <summary>
        /// Returns list of packages with their names, versions and availability to update.
        /// </summary>
        /// <param name="path">Path of .csproj file.</param>
        /// <param name="includePrerelease">Include Prerelease version to compare versions.</param>
        /// <returns>List of Packages.</returns>
        public static async Task<List<Package>> CheckVersionAsync(string path, bool includePrerelease = false)
        {
            // Returning data which consisting packages.
            List<Package> packages = new List<Package>();

            // Checking if path is null or empty.
            if (string.IsNullOrEmpty(path))
            {
                return packages;
            }

            // Get currently used packages.
            List<Package> usedPackages = GetPackages(path);

            // Get used packages info from NuGet.
            List<Package> nugetPackages = await GetPackagesFromNuGetAsync(
                packageNameList: usedPackages.Select(p => p.Name).ToList(),
                includePrerelease: includePrerelease);

            // Loop for packages
            foreach (Package package in usedPackages)
            {
                // Check if newer version is available on NuGet.
                if (nugetPackages.Where(p => p.Name == package.Name).Select(q => q.Version).FirstOrDefault() > package.Version)
                {
                    // Package is available to update.
                    packages.Add(new Package(package.Name, package.Version, updateAvailable: true));
                }
                else
                {
                    // Package is up to date.
                    packages.Add(new Package(package.Name, package.Version, updateAvailable: false));
                }
            }

            // Returning list of packages.
            return packages;
        }

        /// <summary>
        /// Returns list of packages with their names, versions and availability to update.
        /// </summary>
        /// <param name="pathList">List of package names.</param>
        /// <param name="includePrerelease">Include Prerelease version to compare versions.</param>
        /// <returns>List of Packages.</returns>
        public static async Task<List<Package>> CheckVersionAsync(List<string> pathList, bool includePrerelease = false)
        {
            // Returning data which consisting packages.
            List<Package> packages = new List<Package>();

            if (pathList.Any(p => p == null))
            {
                return packages;
            }

            // Get currently used packages.
            List<Package> usedPackages = new List<Package>();

            // Loop for pathlist to fill used packages list.
            foreach (string path in pathList)
            {
                usedPackages.AddRange(GetPackages(path));
            }

            // Instance of comparer.
            PackageEqualityComparer pec = new PackageEqualityComparer();

            // Distinction of packages.
            IEnumerable<Package> disctinctUsedPackage = usedPackages.Distinct(comparer: pec);

            // Get used packages info from NuGet.
            List<Package> nugetPackages = await GetPackagesFromNuGetAsync(
                packageNameList: disctinctUsedPackage.Select(p => p.Name).ToList(),
                includePrerelease: includePrerelease);

            // Loop for packages
            foreach (Package package in usedPackages)
            {
                // Check if newer version is available on NuGet.
                if (disctinctUsedPackage.Where(p => p.Name == package.Name).Select(q => q.Version).FirstOrDefault() > package.Version)
                {
                    // Package is available to update.
                    packages.Add(new Package(package.Name, package.Version, updateAvailable: true));
                }
                else
                {
                    // Package is up to date.
                    packages.Add(new Package(package.Name, package.Version, updateAvailable: false));
                }
            }

            // Returning list of packages.
            return disctinctUsedPackage.ToList();
        }
    }
}