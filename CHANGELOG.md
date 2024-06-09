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