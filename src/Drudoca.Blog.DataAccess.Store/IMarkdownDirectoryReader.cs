﻿namespace Drudoca.Blog.DataAccess.Store;

internal interface IMarkdownDirectoryReader
{
    ValueTask<MarkdownFile[]> ReadAsync(string path);
}