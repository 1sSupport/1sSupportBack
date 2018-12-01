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
    using System;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using WebApi.EF.Models;

    using Xunit;

    /// <summary>
    ///     The ef model test.
    /// </summary>
    public class EfModelTest : IDisposable
    {
        /// <summary>
        ///     The article text.
        /// </summary>
        private readonly string articleText;

        /// <summary>
        ///     The article title.
        /// </summary>
        private readonly string articleTitle;

        /// <summary>
        ///     The config name.
        /// </summary>
        private readonly string configName;

        /// <summary>
        ///     The context options.
        /// </summary>
        private readonly DbContextOptions<EFContext> contextOptions;

        /// <summary>
        ///     The date provider end.
        /// </summary>
        private readonly DateTime dateProviderEnd;

        /// <summary>
        ///     The provider name.
        /// </summary>
        private readonly string providerName;

        /// <summary>
        ///     The query text.
        /// </summary>
        private readonly string queryText;

        /// <summary>
        ///     The tag text.
        /// </summary>
        private readonly string tagText;

        /// <summary>
        ///     The user email.
        /// </summary>
        private readonly string userEmail;

        /// <summary>
        ///     The user inn.
        /// </summary>
        private readonly string userInn;

        /// <summary>
        ///     The user login.
        /// </summary>
        private readonly string userLogin;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EfModelTest" /> class.
        /// </summary>
        public EfModelTest()
        {
            this.contextOptions = new DbContextOptionsBuilder<EFContext>().UseInMemoryDatabase("Test_BD").Options;
            this.userLogin = "123321";
            this.userEmail = "superStrong@mail.com";
            this.userInn = "123456789";
            this.articleText = "Blah blah balh blah la la la la";
            this.articleTitle = "1233";
            this.tagText = "Blah";
            this.configName = "Ypp";
            this.providerName = "OOO MMM";
            this.dateProviderEnd = DateTime.Now.AddDays(10);
            this.queryText = "Мне сложно, помогите";
        }

        /// <summary>
        ///     The can initial article.
        /// </summary>
        [Fact]
        public void CanInitialArticle()
        {
            var article = new Article(this.articleTitle, this.articleText);

            Assert.NotNull(article);
            Assert.Equal(this.articleTitle, article.Title);
            Assert.Equal(this.articleText, article.FileName);
        }

        /// <summary>
        ///     The can initial article tag.
        /// </summary>
        [Fact]
        public void CanInitialArticleTag()
        {
            var tag = new Tag(this.tagText);
            var article = new Article(this.articleTitle, this.articleText);

            var articleTag = new ArticleTag(5, article, tag);

            Assert.NotNull(articleTag);
            Assert.Equal(tag, articleTag.Tag);
            Assert.Equal(article, articleTag.Article);
            Assert.Equal(5, articleTag.Weight);
        }

        /// <summary>
        ///     The can initial configuration 1 c.
        /// </summary>
        [Fact]
        public void CanInitialConfiguration1C()
        {
            var config = new Configuration1C(this.configName);

            Assert.NotNull(config);
            Assert.Equal(this.configName, config.Name);
        }

        /// <summary>
        ///     The can initial context.
        /// </summary>
        [Fact]
        public void CanInitialContext()
        {
            var context = new EFContext(this.contextOptions);

            Assert.NotNull(context);
        }

        /// <summary>
        ///     The can_ initial_ dependes article.
        /// </summary>
        [Fact]
        public void CanInitialDependesArticle()
        {
            var config = new Configuration1C(this.configName);

            var depenedes = new ArticleDependencies(config);

            Assert.NotNull(depenedes);
            Assert.Equal(config, depenedes.Configuration1C);
        }

        /// <summary>
        ///     The can initial opened article.
        /// </summary>
        [Fact]
        public void CanInitialOpenedArticle()
        {
            var provider = new Provider(this.providerName, this.dateProviderEnd, "d");
            var user = new User(this.userLogin, this.userEmail, this.userInn, provider) { Provider = provider };
            var session = new Session(DateTime.Now, user);
            var query = new SearchingQuery(this.queryText);
            var sessionQuary = new SessionQuery(DateTime.Now, session, query);
            var article = new Article(this.articleTitle, this.articleText);
            var date = DateTime.Now;

            var openedArticle = new OpenedArticle(date, article, sessionQuary);

            Assert.NotNull(openedArticle);
            Assert.Equal(openedArticle.Article, article);
            Assert.Equal(openedArticle.SearchingQuery, sessionQuary);
            Assert.Equal(openedArticle.Time, date);
        }

        /// <summary>
        ///     The can initial provider.
        /// </summary>
        [Fact]
        public void CanInitialProvider()
        {
            var provider = new Provider(this.providerName, this.dateProviderEnd, "d");

            Assert.NotNull(provider);
            Assert.Equal(this.providerName, provider.Name);
            Assert.Equal(this.dateProviderEnd, provider.ContractEndTime);
        }

        /// <summary>
        ///     The can initial query.
        /// </summary>
        [Fact]
        public void CanInitialQuery()
        {
            var provider = new Provider(this.providerName, this.dateProviderEnd, "d");
            var user = new User(this.userLogin, this.userEmail, this.userInn, provider) { Provider = provider };
            var session = new Session(DateTime.Now, user);


            var sessionQuary = new SessionQuery(DateTime.Now, session, new SearchingQuery("dd"));

            Assert.NotNull(sessionQuary);
        }

        /// <summary>
        ///     The can initial session.
        /// </summary>
        [Fact]
        public void CanInitialSession()
        {
            var provider = new Provider(this.providerName, this.dateProviderEnd, "d");
            var user = new User(this.userLogin, this.userEmail, this.userInn, provider) { Provider = provider };

            var session = new Session(DateTime.Now, user);

            Assert.NotNull(session);
            Assert.Equal(session.User, user);
        }

        /// <summary>
        ///     The can initial tag.
        /// </summary>
        [Fact]
        public void CanInitialTag()
        {
            var tag = new Tag(this.tagText);

            Assert.NotNull(tag);
            Assert.Equal(this.tagText, tag.Value);
        }

        /// <summary>
        ///     The can initial user.
        /// </summary>
        [Fact]
        public void CanInitialUser()
        {
            var provider = new Provider(this.providerName, this.dateProviderEnd, "d");

            var user = new User(this.userLogin, this.userEmail, this.userInn, provider) { Provider = provider };

            Assert.NotNull(user);
            Assert.Equal(this.userEmail, user.Email);
            Assert.Equal(this.userInn, user.INN);
            Assert.Equal(this.userLogin, user.Login);
            Assert.Equal(user.Provider, provider);
        }

        /// <inheritdoc />
        /// <summary>
        ///     The dispose.
        /// </summary>
        public void Dispose()
        {
        }
    }
}