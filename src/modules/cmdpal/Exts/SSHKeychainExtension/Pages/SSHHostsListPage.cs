﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.CmdPal.Extensions;
using Microsoft.CmdPal.Extensions.Helpers;
using Microsoft.UI.Windowing;
using SSHKeychainExtension.Commands;
using SSHKeychainExtension.Data;

namespace SSHKeychainExtension;

internal sealed class SSHHostsListPage : ListPage
{
    private static readonly string _defaultConfigFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.ssh\\config";

    private static readonly Regex _hostRegex = new(@"^Host\s+(?:(\S*) ?)*?\s*$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

    public SSHHostsListPage()
    {
        Icon = new(string.Empty);
        Name = "SSH Keychain";
    }

    private static async Task<List<SSHKeychainItem>> GetSSHHosts()
    {
        var hosts = new List<SSHKeychainItem>();
        var configFile = _defaultConfigFile;

        if (!File.Exists(configFile))
        {
            return hosts;
        }

        var options = new FileStreamOptions()
        {
            Access = FileAccess.Read,
        };

        using var reader = new StreamReader(configFile, options);
        var fileContent = await reader.ReadToEndAsync();

        if (!string.IsNullOrEmpty(fileContent))
        {
            var matches = _hostRegex.Matches(fileContent);
            hosts = matches.Select(match => new SSHKeychainItem { HostName = match.Groups[1].Value }).ToList();
        }

        return hosts;
    }

    public override ISection[] GetItems()
    {
        var t = DoGetItems();
        t.ConfigureAwait(false);
        return t.Result;
    }

    private async Task<ISection[]> DoGetItems()
    {
        List<SSHKeychainItem> items = await GetSSHHosts();
        var s = new ListSection()
        {
            Title = "SSH Hosts",
            Items = items.Select((host) => new ListItem(new LaunchSSHHostCommand(host))
            {
                Title = host.HostName,
                Subtitle = host.EscapedHost,
            }).ToArray(),
        };
        return [s];
    }
}
