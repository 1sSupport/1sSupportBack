// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EFModelTest.cs" company="">
//
// </copyright>
// <summary>
//   The ef model test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Test
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using WebApi.EF.Models;
    using Xunit;

    /// <summary>
    /// The ef model test.
    /// </summary>
    public class EfModelTest : IDisposable
    {
        /// <summary>
        /// The article text.
        /// </summary>
        private readonly string articleText;

        /// <summary>
        /// The article title.
        /// </summary>
        private readonly string articleTitle;

        /// <summary>
        /// The config name.
        /// </summary>
        private readonly string configName;

        /// <summary>
        /// The context options.
        /// </summary>
        private readonly DbContextOptions<EFContext> contextOptions;

        /// <summary>
        /// The date provider end.
        /// </summary>
        private readonly DateTime dateProviderEnd;

        /// <summary>
        /// The provider name.
        /// </summary>
        private readonly string providerName;

        /// <summary>
        /// The query text.
        /// </summary>
        private readonly string queryText;

        /// <summary>
        /// The tag text.
        /// </summary>
        private readonly string tagText;

        /// <summary>
        /// The user email.
        /// </summary>
        private readonly string userEmail;

        /// <summary>
        /// The user inn.
        /// </summary>
        private readonly string userInn;

        /// <summary>
        /// The user login.
        /// </summary>
        private readonly string userLogin;

        /// <summary>
        /// Initializes a new instance of the <see cref="EfModelTest"/> class.
        /// </summary>
        public EfModelTest()
        {
            contextOptions = new DbContextOptionsBuilder<EFContext>().UseInMemoryDatabase(databaseName: "Test_BD")
                .Options;
            userLogin = "123321";
            userEmail = "superStrong@mail.com";
            userInn = "123456789";
            articleText = "Blah blah balh blah la la la la";
            articleTitle = "1233";
            tagText = "Blah";
            configName = "Ypp";
            providerName = "OOO MMM";
            dateProviderEnd = DateTime.Now.AddDays(10);
            queryText = "Мне сложно, помогите";
        }

        /// <summary>
        /// The can initial article.
        /// </summary>
        [Fact]
        public void CanInitialArticle()
        {
            var article = new Article(articleTitle, articleText);

            Assert.NotNull(article);
            Assert.Equal(articleTitle, article.Title);
            Assert.Equal(articleText, article.Text);
        }

        /// <summary>
        /// The can initial article tag.
        /// </summary>
        [Fact]
        public void CanInitialArticleTag()
        {
            var tag = new Tag(tagText);
            var article = new Article(articleTitle, articleText);

            var articleTag = new ArticleTag(5, article, tag);

            Assert.NotNull(articleTag);
            Assert.Equal(tag, articleTag.Tag);
            Assert.Equal(article, articleTag.Article);
            Assert.Equal(5, articleTag.Weight);
        }

        /// <summary>
        /// The can initial configuration 1 c.
        /// </summary>
        [Fact]
        public void CanInitialConfiguration1C()
        {
            var config = new Configuration1C(configName);

            Assert.NotNull(config);
            Assert.Equal(configName, config.Name);
        }

        /// <summary>
        /// The can initial context.
        /// </summary>
        [Fact]
        public void CanInitialContext()
        {
            var context = new EFContext(contextOptions);

            Assert.NotNull(context);
        }

        /// <summary>
        /// The can_ initial_ dependes article.
        /// </summary>
        [Fact]
        public void CanInitialDependesArticle()
        {
            var config = new Configuration1C(configName);

            var depenedes = new ArticleDependencies(config);

            Assert.NotNull(depenedes);
            Assert.Equal(config, depenedes.Configuration1C);
        }

        /// <summary>
        /// The can initial opened article.
        /// </summary>
        [Fact]
        public void CanInitialOpenedArticle()
        {
            var provider = new Provider(providerName, dateProviderEnd);
            var user = new User(userLogin, userEmail, userInn) { Provider = provider };
            var session = new Session(DateTime.Now, user);
            var query = new SearchingQuery(queryText, session);
            var article = new Article(articleTitle, articleText);
            var date = DateTime.Now;

            var openedArticle = new OpenedArticle(date, article, query);

            Assert.NotNull(openedArticle);
            Assert.Equal(openedArticle.Article, article);
            Assert.Equal(openedArticle.SearchingQuery, query);
            Assert.Equal(openedArticle.Time, date);
        }

        /// <summary>
        /// The can initial provider.
        /// </summary>
        [Fact]
        public void CanInitialProvider()
        {
            var provider = new Provider(providerName, dateProviderEnd);

            Assert.NotNull(provider);
            Assert.Equal(providerName, provider.Name);
            Assert.Equal(dateProviderEnd, provider.ContractEndTime);
        }

        /// <summary>
        /// The can initial query.
        /// </summary>
        [Fact]
        public void CanInitialQuery()
        {
            var provider = new Provider(providerName, dateProviderEnd);
            var user = new User(userLogin, userEmail, userInn) { Provider = provider };
            var session = new Session(DateTime.Now,user);

            var query = new SearchingQuery(queryText, session);

            Assert.NotNull(query);
            Assert.Equal(query.Text, queryText);
            Assert.Equal(query.Session, session);
        }

        /// <summary>
        /// The can initial session.
        /// </summary>
        [Fact]
        public void CanInitialSession()
        {
            var provider = new Provider(providerName, dateProviderEnd);
            var user = new User(userLogin, userEmail, userInn) { Provider = provider };

            var session = new Session(DateTime.Now,user);

            Assert.NotNull(session);
            Assert.Equal(session.User, user);
        }

        /// <summary>
        /// The can initial tag.
        /// </summary>
        [Fact]
        public void CanInitialTag()
        {
            var tag = new Tag(tagText);

            Assert.NotNull(tag);
            Assert.Equal(tagText, tag.Value);
        }

        /// <summary>
        /// The can initial user.
        /// </summary>
        [Fact]
        public void CanInitialUser()
        {
            var provider = new Provider(providerName, dateProviderEnd);

            var user = new User(userLogin, userEmail, userInn) { Provider = provider };

            Assert.NotNull(user);
            Assert.Equal(userEmail, user.Email);
            Assert.Equal(userInn, user.INN);
            Assert.Equal(userLogin, user.Login);
            Assert.Equal(user.Provider, provider);
        }

        /// <inheritdoc />
        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
        }
    }
}