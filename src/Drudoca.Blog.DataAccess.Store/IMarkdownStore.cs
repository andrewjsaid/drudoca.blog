﻿namespace Drudoca.Blog.DataAccess.Store;

internal interface IMarkdownStore<T>
{
    ValueTask<T[]> GetAllAsync();
}