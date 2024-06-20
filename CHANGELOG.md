## NugetVersionChecker Changelog

<!--
### [Unreleased] (YYYY-MM-DD)

#### Added

#### Changed

#### Deprecated

#### Fixed

#### Removed

#### Security
-->

### [1.0.0-rc] (2024-06-20)

#### Added
* `CheckVersionAsync(string path)` method added. This method return packages with their names, versions and available to update as a list.

#### Changed
* Minor performance improvements.
* `Package` now has `SemanticVersion` instead of `System.Version`.
* `Package` now has UpdateAvailable value to indicate newer version is available.
* `Package` now has its third constructor. `Package(string name, SemanticVersion version, bool updateAvailable)`.
* `Package(string name, Version versioon)` is replaced with `Package(string name, SemanticVersion version)` due to change on `Package`. 
* `GetPackages(string path)` now try-catch mechanism when loading and parsing .csporj file with `XmlDocument`.
* `GetPackages(string path)` method now has inner explanations line by line.

#### Deleted
* `GetPackageFromNuget(string packageName)` method was marked as obsolote due to naming mistake and on this it is deleted.
* `GetPackagesFromNuget(List<string> packageNameList)` method was marked as obsolote due to naming mistake and on this it is deleted.

### [1.0.0-beta.2] (2024-06-14)
#### Added
* `Package` now has `Package()` constructor among `Package(string name, Version version)`

#### Changed
* `GetPackageFromNugetAsync(string packageName)` no longer throws exception when package or its versions are not found. It simple returns empty package.
* `GetPackagesFromNuGetAsync(List<string> packageNameList)` no longer throws exception when one of package or its versions are not found. It simple returns a list of package.

### [1.0.0-beta.1] (2024-06-09)
### Added
* `GetPackageFromNuGetAsync(string packageName)` method is added instead of now obsolote `GetPackageFromNuget(string packageName)` method.
* `GetPackagesFromNuGetAsync(List<string> packageNameList)` method is added instead of now obsolote `GetPackagesFromNuget(List<string> packageNameList)` method.

### Changed
* After making a search among results, `SingleOrDefault()` is using instead of `First()`. Since search parameter has one as "take:", result can be only one. That's why `Single()`is better. `SingleOrDefault()` is used now due to wrong or not existing "packageName".

### Deprecated
* `GetPackageFromNuget(string packageName)` is obsolote now due to naming mistake. You can use `GetPackageFromNuGetAsync(string packageName)` instead.
* `GetPackagesFromNuget(List<string> pakcageNameList)` is obsolote now due to naming mistake. You can use `GetPackagesFromNuGetAsync(List<string> packageNameList)` instead.

### [1.0.0-beta] (2024-06-07)
Initial release.