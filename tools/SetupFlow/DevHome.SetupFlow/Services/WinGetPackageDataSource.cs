// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevHome.SetupFlow.Common.Helpers;
using DevHome.SetupFlow.Models;

namespace DevHome.SetupFlow.Services;

/// <summary>
/// Abstract class for a WinGet package data source
/// </summary>
public abstract class WinGetPackageDataSource
{
    /// <summary>
    /// Gets the total number of package catalogs available in this data source
    /// </summary>
    public abstract int CatalogCount { get; }

    protected IWindowsPackageManager WindowsPackageManager { get; }

    public WinGetPackageDataSource(IWindowsPackageManager wpm)
    {
        WindowsPackageManager = wpm;
    }

    /// <summary>
    /// Initialize the data source
    /// </summary>
    public abstract Task InitializeAsync();

    /// <summary>
    /// Load catalogs from the data source
    /// </summary>
    /// <returns>List of package catalogs</returns>
    public abstract Task<IList<PackageCatalog>> LoadCatalogsAsync();

    /// <summary>
    /// Get a list of packages from WinGet catalog
    /// </summary>
    /// <typeparam name="T">Input type</typeparam>
    /// <param name="packageUris">List of package URIs</param>
    /// <returns>List of packages</returns>
    protected async Task<IList<IWinGetPackage>> GetPackagesAsync(IList<WinGetPackageUri> packageUris)
    {
        List<IWinGetPackage> result = new();

        // Skip search if package data source is empty
        if (!packageUris.Any())
        {
            Log.Logger?.ReportWarn(Log.Component.AppManagement, $"{nameof(GetPackagesAsync)} received an empty list of items. Skipping search.");
            return result;
        }

        // Get packages from winget catalog
        return await WindowsPackageManager.GetPackagesAsync(packageUris);
    }
}
