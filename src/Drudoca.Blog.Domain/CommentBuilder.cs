using System;
using System.Collections.Generic;
using Drudoca.Blog.Data;

namespace Drudoca.Blog.Domain
{
    internal class CommentBuilder : ICommentBuilder
    {
        private readonly IMarkdownParser _markdownParser;

        public CommentBuilder(IMarkdownParser markdownParser)
        {
            _markdownParser = markdownParser;
        }

        public BlogComment[] BuildTree(CommentData[] commentData)
        {
            var edges = GetEdges(commentData);

            var internalNodes = new Dictionary<long, List<BlogComment>>(); // internal nodes which have already been converted
            var roots = new List<BlogComment>(); // root nodes which have already been converted

            var postedOnUtcComparer = new PostedOnUtcComparer();

            int lastEdgesCount = int.MaxValue;

            while(edges.Count > 0 && edges.Count < lastEdgesCount)
            {
                lastEdgesCount = edges.Count;

                // Performance: Go through the array in reverse order since children will more likely come after parents
                for (int i = commentData.Length - 1; i >= 0; i--)
                {
                    var data = commentData[i];

                    if (!edges.TryGetValue(data.Id, out var dataChildren))
                    {
                        // Already converted this node.
                        continue;
                    }

                    if (dataChildren.Count > 0)
                    {
                        // Not all children have been converted.
                        continue;
                    }

                    // At least 1 improvement will be made this iteration

                    // No more children left to be converted.

                    var children = GetLeaves(data.Id, internalNodes);
                    if (children != null)
                    {
                        Array.Sort(children, postedOnUtcComparer);
                    }
                    var domainObject = BuildComment(data, children);

                    // Remove it from "pending" list
                    edges.Remove(data.Id);

                    // Add it to the list of pending nodes.

                    if (data.ParentId == null)
                    {
                        roots.Add(domainObject);
                    }
                    else
                    {
                        // Remove from parent edge
                        var parentEdge = edges[data.ParentId.Value];
                        parentEdge.Remove(data);

                        // Add to internal nodes
                        AddToInternalNodes(internalNodes, data.ParentId.Value, domainObject);
                    }
                }
            }

            roots.Sort(postedOnUtcComparer);
            return roots.ToArray();
        }

        private void AddToInternalNodes(
            Dictionary<long, List<BlogComment>> nodes,
            long parentId,
            BlogComment domainObject)
        {
            if (!nodes.TryGetValue(parentId, out var parentDomainList))
            {
                parentDomainList = new List<BlogComment>();
                nodes.Add(parentId, parentDomainList);
            }
            parentDomainList.Add(domainObject);
        }

        private static BlogComment[]? GetLeaves(long id, Dictionary<long, List<BlogComment>> leaves)
        {
            if (!leaves.TryGetValue(id, out List<BlogComment> domainChildren))
                return null;

            if (domainChildren.Count == 0)
                return null;

            return domainChildren.ToArray();
        }

        /// <summary>
        /// Gets all edges from the data.
        /// All comments have a key in the result.
        /// </summary>
        private static Dictionary<long, List<CommentData>> GetEdges(CommentData[] commentData)
        {
            var edges = new Dictionary<long, List<CommentData>>();
            foreach (var cd in commentData)
            {
                if (!edges.TryGetValue(cd.Id, out var _))
                {
                    edges.Add(cd.Id, new List<CommentData>());
                }

                if (cd.ParentId != null)
                {
                    if (!edges.TryGetValue(cd.ParentId.Value, out var list))
                    {
                        list = new List<CommentData>();
                        edges.Add(cd.ParentId.Value, list);
                    }
                    list.Add(cd);
                }
            }

            return edges;
        }

        private BlogComment BuildComment(CommentData data, BlogComment[]? children)
        {
            var html = _markdownParser.ToCommentHtml(data.Markdown);

            var result = new BlogComment(
                data.Id,
                data.Author,
                data.Email,
                data.PostedOnUtc,
                html,
                data.IsDeleted,
                children);

            return result;
        }

        private class PostedOnUtcComparer : IComparer<BlogComment>
        {
            public int Compare(BlogComment x, BlogComment y) => x.PostedOnUtc.CompareTo(y.PostedOnUtc);
        }

    }
}
