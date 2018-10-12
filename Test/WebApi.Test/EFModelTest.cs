using Microsoft.EntityFrameworkCore;
using System;
using WebApi.EF.Models;
using Xunit;

namespace WebApi.Test
{
    public class EFModelTest : IDisposable
    {
        private readonly string _articleText;
        private readonly string _articleTitle;
        private readonly string _tagText;
        private readonly string _configName;
        private readonly string _providerName;
        private readonly string _userLogin;
        private readonly string _userEmail;
        private readonly string _userInn;
        private readonly string _queryText;
        private readonly DateTime _dateProviderEnd;
        private readonly DbContextOptions<EFContext> _contextOptions;

        public EFModelTest()
        {
            _contextOptions = new DbContextOptionsBuilder<EFContext>().UseInMemoryDatabase(databaseName: "Test_BD").Options;
            _userLogin = "123321";
            _userEmail = "superStrong@mail.com";
            _userInn = "123456789";
            _articleText = "Blah blah balh blah la la la la";
            _articleTitle = "1233";
            _tagText = "Blah";
            _configName = "Ypp";
            _providerName = "OOO MMM";
            _dateProviderEnd = DateTime.Now.AddDays(10);
            _queryText = "Мне сложно, помогите";
        }

        public void Dispose()
        {
        }

        [Fact]
        public void Can_Initial_Context()
        {
            var context = new EFContext(_contextOptions);

            Assert.NotNull(context);
        }

        [Fact]
        public void Can_Initial_Article()
        {
            var article = new Article(_articleTitle, _articleText);

            Assert.NotNull(article);
            Assert.Equal(_articleTitle, article.Title);
            Assert.Equal(_articleText, article.Text);
        }

        [Fact]
        public void Can_Initial_Tag()
        {
            var tag = new Tag(_tagText);

            Assert.NotNull(tag);
            Assert.Equal(_tagText, tag.Value);
        }

        [Fact]
        public void Can_Initial_ArticleTag()
        {
            var tag = new Tag(_tagText);
            var article = new Article(_articleTitle, _articleText);

            var articleTag = new ArticleTag(article, tag);

            Assert.NotNull(articleTag);
            Assert.Equal(tag, articleTag.Tag);
            Assert.Equal(article, articleTag.Article);
        }

        [Fact]
        public void Can_Initial_Configuration1c()
        {
            var config = new Configuration1C(_configName);

            Assert.NotNull(config);
            Assert.Equal(_configName, config.Name);
        }

        [Fact]
        public void Can_Initial_DependesArticle()
        {
            var config = new Configuration1C(_configName);

            var depenedes = new ArticleDependencies(config);

            Assert.NotNull(depenedes);
            Assert.Equal(config, depenedes.Configuration1C);
        }

        [Fact]
        public void Can_Initial_Provider()
        {
            var provider = new Provider(_providerName, _dateProviderEnd);

            Assert.NotNull(provider);
            Assert.Equal(_providerName, provider.Name);
            Assert.Equal(_dateProviderEnd, provider.ContractEndTime);
        }

        [Fact]
        public void Can_Initial_User()
        {
            var provider = new Provider(_providerName, _dateProviderEnd);

            var user = new User(_userLogin, _userEmail, _userInn, provider);

            Assert.NotNull(user);
            Assert.Equal(_userEmail, user.Email);
            Assert.Equal(_userInn, user.INN);
            Assert.Equal(_userLogin, user.Login);
            Assert.Equal(user.Provider, provider);
        }

        [Fact]
        public void Can_Initial_Session()
        {
            var provider = new Provider(_providerName, _dateProviderEnd);
            var user = new User(_userLogin, _userEmail, _userInn, provider);

            var session = new Session(user);

            Assert.NotNull(session);
            Assert.Equal(session.User, user);
        }

        [Fact]
        public void Can_Initial_Query()
        {
            var provider = new Provider(_providerName, _dateProviderEnd);
            var user = new User(_userLogin, _userEmail, _userInn, provider);
            var session = new Session(user);

            var query = new SearchingQuery(_queryText, session);

            Assert.NotNull(query);
            Assert.Equal(query.Text, _queryText);
            Assert.Equal(query.Session, session);
        }

        [Fact]
        public void Can_Initial_OpenedArticle()
        {
            var provider = new Provider(_providerName, _dateProviderEnd);
            var user = new User(_userLogin, _userEmail, _userInn, provider);
            var session = new Session(user);
            var query = new SearchingQuery(_queryText, session);
            var article = new Article(_articleTitle, _articleText);
            var date = DateTime.Now;

            var openedArticle = new OpenedArticle(date, article, query);

            Assert.NotNull(openedArticle);
            Assert.Equal(openedArticle.Article, article);
            Assert.Equal(openedArticle.SearchingQuery, query);
            Assert.Equal(openedArticle.Time, date);
        }
    }
}