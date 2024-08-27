// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Windows.Management.Deployment;
// using Wox.Plugin.Logger;
using Package = Windows.ApplicationModel.Package;

namespace AllApps.Programs;

public class PackageManagerWrapper : IPackageManager
{
    private readonly PackageManager _packageManager;

    public PackageManagerWrapper()
    {
        _packageManager = new PackageManager();
    }

    public IEnumerable<IPackage> FindPackagesForCurrentUser()
    {
        var user = WindowsIdentity.GetCurrent().User;

        var pkgs = _packageManager.FindPackagesForUser(user.Value);

        return user != null
            ? pkgs
                .Select(TryGetWrapperFromPackage).Where(package => package != null)
            : Enumerable.Empty<IPackage>();
    }

    private static PackageWrapper TryGetWrapperFromPackage(Package package)
    {
        try
        {
            return PackageWrapper.GetWrapperFromPackage(package);
        }
        catch (Exception )
        {
            // Log.Error(e.Message, typeof(PackageManagerWrapper));
        }

        return null;
    }
}