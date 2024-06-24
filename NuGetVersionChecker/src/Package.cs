using NuGet.Versioning;
using System;

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
        }
    }    
}
