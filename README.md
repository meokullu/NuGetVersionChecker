## NuGetVersionChecker
[![NuGetVersionChecker](https://img.shields.io/nuget/v/NuGetVersionChecker.svg)](https://www.nuget.org/packages/NuGetVersionChecker/) [![NuGetVersionChecker](https://img.shields.io/nuget/dt/NuGetVersionChecker.svg)](https://www.nuget.org/packages/NuGetVersionChecker/) [![License](https://img.shields.io/github/license/meokullu/NuGetVersionChecker.svg)](https://github.com/meokullu/NuGetVersionChecker/blob/master/LICENSE)

NuGet Version Checker is a project aims recency of NuGet packges on projects.

### Description
Since there is no tracking mechanism that notifies users of package without manually checking them, NuGetVersionChecker will allow a way to track packages certain ways on upcoming versions.

### Example Usage

```
// Path for .csproj file. Returns list of `Package` that contains package name and it version.
GetPackages(string path);
```

```
// packageName of the NuGet package. Returns `Package` that contains package name and its version.
GetPackageFromNuGetAsync(string packageName);
```

```
// packageNameList of the list of NuGet package names. Returns List of `Package` that contains packages name and their versions.
GetPackagesFromNuGetAsync(List<string> packageNameList);
```

### Version History
See [Changelog](https://github.com/meokullu/NuGetVersionChecker/blob/master/CHANGELOG.md)
  
### Task list
* Create an issue or check task list: [Issues](https://github.com/meokullu/NuGetVersionChecker/issues)

### Licence
This repository is licensed under the "MIT" license. See [MIT license](https://github.com/meokullu/NuGetVersionChecker/blob/master/LICENSE).

### Authors & Contributing
If you'd like to contribute, then contribute. [contributing guide](https://github.com/meokullu/NuGetVersionChecker/blob/master/CONTRIBUTING.md).

[![Contributors](https://contrib.rocks/image?repo=meokullu/NuGetVersionChecker)](https://github.com/meokullu/NuGetVersionChecker/graphs/contributors)

### Help
Twitter: Enes Okullu [@enesokullu](https://twitter.com/EnesOkullu)