using RestSharpServices;
using System.Net;
using System.Reflection.Emit;
using System.Text.Json;
using RestSharp;
using RestSharp.Authenticators;
using NUnit.Framework.Internal;
using RestSharpServices.Models;
using System;

namespace TestGitHubApi
{
    public class TestGitHubApi
    {
        private GitHubApiClient client;
        private static string repo;
        private static int lastCreatedIssueNumber;
        private static int lastCreatedCommentId;
        

        [SetUp]
        public void Setup()
        {            
            client = new GitHubApiClient("https://api.github.com/repos/testnakov/", "ChavdarDimov", "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");
            repo = "test-nakov-repo";
        }


        [Test, Order (1)]
        public void Test_GetAllIssuesFromARepo()
        {
            // Arrange

            // Act
            var issues = client.GetAllIssues(repo);

            // Assert
            Assert.That(issues.Count, Is.GreaterThan(1));

            foreach (var issue in issues)
            {
                Assert.That(issue.Id, Is.GreaterThan(0));
                Assert.That(issue.Number, Is.GreaterThan(0));
                Assert.That(issue.Title, Is.Not.Empty);
            }
        }

        [Test, Order (2)]
        public void Test_GetIssueByValidNumber()
        {
            // Arrange
            int issueNumber = 1;

            // Act
            var issue = client.GetIssueByNumber(repo, issueNumber);

            // Assert
            Assert.That(issue, Is.Not.Null);
            Assert.That(issue.Id, Is.GreaterThan(0));
            Assert.That(issue.Number, Is.EqualTo(issueNumber));
            Assert.That(issue.Title, Is.Not.Empty);
        }
        
        [Test, Order (3)]
        public void Test_GetAllLabelsForIssue()
        {
            // Arrange
            int issueNumber = 6;

            // Act
            var labels = client.GetAllLabelsForIssue(repo, issueNumber);

            // Assert
            Assert.That(labels.Count, Is.GreaterThan(0));

            foreach (var label in labels)
            {
                Assert.That(label.Id, Is.GreaterThan(0));
                Assert.That(label.Name, Is.Not.Null);

                Console.WriteLine($"Label: {label.Id} - Name: {label.Name}");
            }
        }

        [Test, Order (4)]
        public void Test_GetAllCommentsForIssue()
        {
            // Arrange
            int issueNumber = 6;

            // Act
            var comments = client.GetAllCommentsForIssue(repo, issueNumber);

            // Assert
            Assert.That(comments.Count, Is.GreaterThan(0));

            foreach (var comment in comments)
            {
                Assert.That(comment.Id, Is.GreaterThan(0));
                Assert.That(comment.Body, Is.Not.Null);

                Console.WriteLine($"Comment: {comment.Id} - Body: {comment.Body}");
            }
        }

        [Test, Order(5)]
        public void Test_CreateGitHubIssue()
        {
            // Arrange
            string title = "New Issue Title";
            string body = "Body of the New Issue";

            // Act
            var issue = client.CreateIssue(repo, title, body);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(issue.Id, Is.GreaterThan(0));
                Assert.That(issue.Number, Is.GreaterThan(0));
                Assert.That(issue.Title, Is.Not.Empty);
                Assert.That(issue.Title, Is.EqualTo(title));
            });

            Console.WriteLine(issue.Number);
            lastCreatedIssueNumber = issue.Number;
        }

        [Test, Order (6)]
        public void Test_CreateCommentOnGitHubIssue()
        {
            // Arrange
            string body = "Body of the New Comment";
            int issueNumber = 6917;

            // Act
            var comment = client.CreateCommentOnGitHubIssue(repo, issueNumber, body);

            // Assert
            Assert.That(comment.Body, Is.EqualTo(body));

            Console.WriteLine(comment.Id);
            lastCreatedCommentId = comment.Id;
        }

        [Test, Order (7)]
        public void Test_GetCommentById()
        {
            // Arrange
            int commentId = 2002492659;

            // Act
            var comment = client.GetCommentById(repo, commentId);

            // Assert
            Assert.That(comment.Id, Is.EqualTo(commentId));
            Assert.That(comment, Is.Not.Null);
        }


        [Test, Order (8)]
        public void Test_EditCommentOnGitHubIssue()
        {
            // Arrange
            int commentId = 2002492659;
            string newBody = "Edited Body on Comment";

            // Act
            var comment = client.EditCommentOnGitHubIssue(repo, commentId, newBody);

            // Assert
            Assert.That(comment.Id, Is.EqualTo(commentId));
            Assert.That(comment, Is.Not.Null);
            Assert.That(comment.Body, Is.EqualTo(newBody));
        }

        [Test, Order (9)]
        public void Test_DeleteCommentOnGitHubIssue()
        {
            // Arrange
            int commentId = 2002492659;

            // Act
            var response = client.DeleteCommentOnGitHubIssue(repo, commentId);

            // Assert
            Assert.That(response, Is.True);
        }


    }
}

