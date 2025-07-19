using Drudoca.Blog.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Drudoca.Blog.Domain.Tests;

[TestClass]
public class CommentBuilderTests
{
    const string _fileName = "test.md";

    private readonly List<CommentData> _comments = [];
    private BlogComment[]? _results;

    private CommentTreeFluent Tree => new CommentTreeFluent(this, null);
    private CommentAssertions ThenRootComment(int i) => new CommentAssertions(_results![i]);

    private int _nextCommentId = 1;

    [TestMethod]
    public void No_Comments()
    {
        WhenBuilt();

        ThereAreRootComments(0);
    }

    [TestMethod]
    public void One_Root_Comment()
    {
        Tree.GivenAComment("andrew", "hello");

        WhenBuilt();

        ThereAreRootComments(1);

        ThenRootComment(0)
            .HasAuthor("andrew")
            .HasHtml("hello")
            .HasChildren(0);
    }

    [TestMethod]
    public void Multiple_Root_Comments_Unsorted()
    {
        Tree.GivenAComment("andrew", "hello1")
            .GivenAComment("bob", "hello2");

        GivenCommentsAreOutOfOrder();

        WhenBuilt();

        ThereAreRootComments(2);

        ThenRootComment(0)
            .HasAuthor("andrew")
            .HasHtml("hello1")
            .HasChildren(0);

        ThenRootComment(1)
            .HasAuthor("bob")
            .HasHtml("hello2")
            .HasChildren(0);
    }

    [TestMethod]
    public void Chain_Of_Replies()
    {
        Tree.GivenAComment("andrew", "1", a =>
            a.GivenAComment("bob", "1.1", b =>
                    b.GivenAComment("charlie", "1.1.1")
                        .GivenAComment("cherry", "1.1.2"))
                .GivenAComment("barry", "1.2", b =>
                    b.GivenAComment("clippy", "1.2.1")));

        GivenCommentsAreOutOfOrder();

        WhenBuilt();

        ThereAreRootComments(1);
        ThenRootComment(0)
            .HasAuthor("andrew")
            .HasChildren(2)
            .AndChild(0, b =>
                b.HasAuthor("bob")
                    .HasChildren(2)
                    .AndChild(0, c => c.HasAuthor("charlie").HasChildren(0))
                    .AndChild(1, c => c.HasAuthor("cherry").HasChildren(0)))
            .AndChild(1, b =>
                b.HasAuthor("barry")
                    .HasChildren(1)
                    .AndChild(0, c => c.HasAuthor("clippy").HasChildren(0)));
    }

    private void GivenCommentsAreOutOfOrder()
    {
        _comments.Sort((a, b) => b.PostedOnUtc.CompareTo(a.PostedOnUtc));
    }

    private void WhenBuilt()
    {
        var sut = new CommentBuilder(new MockMarkdownParser());
        _results = sut.BuildTree(_comments.ToArray());
    }

    private void ThereAreRootComments(int n)
    {
        var results = _results!;
        Assert.IsNotNull(results);
        Assert.AreEqual(n, results.Length);
    }

    private class CommentAssertions(BlogComment result)
    {
        public CommentAssertions HasAuthor(string name)
        {
            Assert.AreEqual(name, result.Author);
            return this;
        }

        public CommentAssertions HasHtml(string html)
        {
            Assert.AreEqual(html.Trim(), result.Html.Trim());
            return this;
        }

        public CommentAssertions HasChildren(int n)
        {
            Assert.AreEqual(n, (result.Replies?.Length).GetValueOrDefault());
            return this;
        }

        public CommentAssertions AndChild(int n, Action<CommentAssertions> f)
        {
            f(new CommentAssertions(result.Replies![n]));
            return this;
        }
    }

    private class CommentTreeFluent(
        CommentBuilderTests testClass,
        long? parent)
    {
        public CommentTreeFluent GivenAComment(string author, string markdown, Action<CommentTreeFluent>? setupChildren = null)
        {
            var comment = new CommentData
            {
                Id = testClass._nextCommentId++,
                PostFileName = _fileName,
                ParentId = parent,
                Author = author,
                Email = $"{author}@drudoca.com",
                Markdown = markdown,
                PostedOnUtc = DateTime.UtcNow.AddMinutes(testClass._comments.Count), // Otherwise they all have the same timestamp
                IsDeleted = false
        };

            testClass._comments.Add(comment);

            setupChildren?.Invoke(new CommentTreeFluent(testClass, comment.Id));
            return this;
        }
    }
}