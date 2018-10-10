using System;
using WebApi.EF.Models;
using Xunit;

namespace WebApi.Test
{
    public class EFModelTest : IDisposable
    {
        private readonly string articleText;
        private readonly string articleTitle;
        private readonly string tagText;
        private readonly string configName;
        private readonly string providerName;
        private readonly string userLogin;
        private readonly string userEmail;
        private readonly string userINN;
        private readonly string queryText;
        private readonly DateTime dateProviderEnd;

        public EFModelTest()
        {
            userLogin = "123321";
            userEmail = "superStrong@mail.com";
            userINN = "123456789";
            articleText = "Blah blah balh blah la la la la";
            articleTitle = "1233";
            tagText = "Blah";
            configName = "Ypp";
            providerName = "OOO MMM";
            dateProviderEnd = DateTime.Now.AddDays(10);
            queryText = "Мне сложно, помогите";
        }

        public void Dispose()
        {
        }

        [Fact]
        public void Can_Initial_Article()
        {
            var article = new Article(articleTitle, articleText);

            Assert.NotNull(article);
            Assert.Equal(articleTitle, article.Title);
            Assert.Equal(articleText, article.Source);
        }

        [Fact]
        public void Can_Initial_Tag()
        {
            var tag = new Tag(tagText);

            Assert.NotNull(tag);
            Assert.Equal(tagText, tag.Value);
        }

        [Fact]
        public void Can_Initial_ArticleTag()
        {
            var tag = new Tag(tagText);
            var article = new Article(articleTitle, articleText);

            var articleTag = new ArticleTag(article, tag);

            Assert.NotNull(articleTag);
            Assert.Equal(tag, articleTag.Tag);
            Assert.Equal(article, articleTag.Article);
        }

        [Fact]
        public void Can_Initial_Configuration1c()
        {
            var config = new Configuration1C(configName);

            Assert.NotNull(config);
            Assert.Equal(configName, config.Name);
        }

        [Fact]
        public void Can_Initial_DependesArticle()
        {
            var config = new Configuration1C(configName);

            var depenedes = new ArticleDependencies(config);

            Assert.NotNull(depenedes);
            Assert.Equal(config, depenedes.Configuration1C);
        }

        [Fact]
        public void Can_Initial_Provider()
        {
            var provider = new Provider(providerName, dateProviderEnd);

            Assert.NotNull(provider);
            Assert.Equal(providerName, provider.Name);
            Assert.Equal(dateProviderEnd, provider.ContractEndTime);
        }

        [Fact]
        public void Can_Initial_User()
        {
            var provider = new Provider(providerName, dateProviderEnd);

            var user = new User(userLogin, userEmail, userINN, provider);

            Assert.NotNull(user);
            Assert.Equal(userEmail, user.Email);
            Assert.Equal(userINN, user.INN);
            Assert.Equal(userLogin, user.Login);
            Assert.Equal(user.Provider, provider);
        }

        [Fact]
        public void Can_Initial_Session()
        {
            var provider = new Provider(providerName, dateProviderEnd);
            var user = new User(userLogin, userEmail, userINN, provider);

            var session = new Session(user);

            Assert.NotNull(session);
            Assert.Equal(session.User, user);
        }

        [Fact]
        public void Can_Initial_Query()
        {
            var provider = new Provider(providerName, dateProviderEnd);
            var user = new User(userLogin, userEmail, userINN, provider);
            var session = new Session(user);

            var query = new SearchingQuery(queryText, session);

            Assert.NotNull(query);
            Assert.Equal(query.Text, queryText);
            Assert.Equal(query.Session, session);
        }

        [Fact]
        public void Can_Initial_OpenedArticle()
        {
            var provider = new Provider(providerName, dateProviderEnd);
            var user = new User(userLogin, userEmail, userINN, provider);
            var session = new Session(user);
            var query = new SearchingQuery(queryText, session);
            var article = new Article(articleTitle, articleText);
            var date = DateTime.Now;

            var openedArticle = new OpenedArticle(date, article, query);

            Assert.NotNull(openedArticle);
            Assert.Equal(openedArticle.Article, article);
            Assert.Equal(openedArticle.SearchingQuery, query);
            Assert.Equal(openedArticle.Time, date);
        }
    }
}