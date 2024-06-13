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
            public Version Version;

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
            /// <param name="version">Version of the package on <seealso cref="System.Version"/> format.</param>
            public Package(string name, Version version)
            {
                Name = name;
                Version = version;
            }
        }
    }    
}
