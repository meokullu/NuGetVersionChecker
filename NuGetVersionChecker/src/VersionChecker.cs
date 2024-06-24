using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NuGetVersionChecker
{
    public partial class NuGetVersionChecker
    {
        /// <summary>
        /// Returns list of packages with their names, versions and availability to update.
        /// </summary>
        /// <param name="path">Path of .csproj file.</param>
        /// <returns>List of Packages.</returns>
        public static async Task<List<Package>> CheckVersionAsync(string path)
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
            List<Package> nugetPackages = await GetPackagesFromNuGetAsync(usedPackages.Select(p => p.Name).ToList());

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
    }
}