using NuGet.Versioning;
using System;
using System.Collections.Generic;

namespace NuGetVersionChecker
{
    /// <summary>
    /// Package class.
    /// </summary>
    public partial class NuGetVersionChecker
    {
        /// <summary>
        /// Package class that hold Name and Version with constructor.
        /// </summary>
        public class Package
        {
            /// <summary>
            /// Name of the package.
            /// </summary>
            public string Name;

            /// <summary>
            /// Version of the package.
            /// </summary>
            public SemanticVersion Version;

            /// <summary>
            /// If new version is available to update.
            /// </summary>
            public bool UpdateAvailable;

            /// <summary>
            /// Empty Package constructor.
            /// </summary>
            public Package()
            {

            }

            /// <summary>
            /// Package constructor with name and version.
            /// </summary>
            /// <param name="name">Name of the package.</param>
            /// <param name="version">Version of the package on <seealso cref="SemanticVersion"/> format.</param>
            public Package(string name, SemanticVersion version)
            {
                Name = name;
                Version = version;
            }

            /// <summary>
            /// Package constructor with name, version and availability to update..
            /// </summary>
            /// <param name="name">Name of the package.</param>
            /// <param name="version">Version of the package on <seealso cref="SemanticVersion"/> format.</param>
            /// <param name="updateAvailable">Availability to update.</param>
            public Package(string name, SemanticVersion version, bool updateAvailable)
            {
                Name = name;
                Version = version;
                UpdateAvailable = updateAvailable;
            }

            /// <summary>
            /// Override method of ToString(), simply displays package name and its version.
            /// </summary>
            /// <returns>Concat of package name and version.</returns>
            public override string ToString()
            {
                return $"{Name} {Version}";
            }

            /// <summary>
            /// Package equality comparer derived from IEqualityComparer interface.
            /// </summary>
            public class PackageEqualityComparer : IEqualityComparer<Package>
            {
                /// <summary>
                /// Compares two Package based on their name and version.
                /// </summary>
                /// <param name="x">First package to compare.</param>
                /// <param name="y">Second package to compare.</param>
                /// <returns>Returns true if packages are same, returns false if packages are different or one of the given package is null.</returns>
                public bool Equals(Package x, Package y)
                {
                    // Checking if either of package is null.
                    if (x == null || y == null)
                    {
                        // Returning false to indicate at least one of packages is null.
                        return false;
                    }
                    // Checking if two given package's property values are same.
                    else if (x.Name == y.Name && x.Version == y.Version)
                    {
                        return true;
                    }
                    else
                    {
                        // Returning false to indicate packages are different.
                        return false;
                    }
                }

                /// <summary>
                /// Returning hashcode with creating tuple of Package's name and version.
                /// </summary>
                /// <param name="obj">A package.</param>
                /// <returns>int value of hashcode.</returns>
                public int GetHashCode(Package obj)
                {
                    // Returning hash code.
                    return Tuple.Create(obj.Name, obj.Version).GetHashCode();
                }
            }
        }
    }
}
