﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace Microsoft.CmdPal.Ext.Bookmarks;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(BookmarkData))]
internal sealed partial class SourceGenerationContext : JsonSerializerContext
{
}
