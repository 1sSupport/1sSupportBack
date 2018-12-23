// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagiratorTest.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the TagiratorTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;

    using WebApi.EF.Models;
    using WebApi.Tools.Finder;
    using WebApi.Tools.Tagirator;

    using Xunit;
    

    /// <inheritdoc />
    /// <summary>
    ///     The tagirator test.
    /// </summary>
    public class TagiratorTest : IDisposable
    {
        /// <summary>
        ///     The context.
        /// </summary>
        private readonly EFContext context;

        /// <summary>
        /// The subdir.
        /// </summary>
        private DirectoryInfo Subdir;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TagiratorTest" /> class.
        /// </summary>
        public TagiratorTest()
        {
            this.context = new EFContext(
                new DbContextOptionsBuilder<EFContext>().UseInMemoryDatabase("Tagirator_test_BD").Options);
            this.Initial();
        }

        /// <summary>
        ///     The can get article in query.
        /// </summary>
        [Fact]
        public void CanGetArticleInQuery()
        {
            this.CanInitializeTagirator();
            var query = new ArticleFinder(this.context);
            var articles = query.GetArticlesByQuery("Врата да да я");
            Assert.NotEmpty(articles);
        }

        /// <inheritdoc />
        /// <summary>
        ///     The dispose.
        /// </summary>
        public void Dispose()
        {
            this.context.Dispose();
            this.Subdir.Delete(true);
        }

        /// <summary>
        ///     The can initialize tagirator.
        /// </summary>
        private void CanInitializeTagirator()
        {
            var tagirator = new Tagirator(this.context);
            tagirator.SetTagsInArticle();
            this.context.SaveChanges();

            Assert.NotEmpty(this.context.Articles);
            Assert.NotNull(this.context.Articles.FirstOrDefault());
            Assert.NotEmpty(this.context.Tags);
            Assert.NotEmpty(this.context.ArticleTags);
        }

        /// <summary>
        /// The initial.
        /// </summary>
        private void Initial()
        {
            var dir = new DirectoryInfo(Environment.CurrentDirectory);
            this.Subdir = dir.CreateSubdirectory("TestTagirator");

            var list = new List<SaveArticle>();
            list.Add(
                new SaveArticle(
                    "Да да Заглавие",
                    "Огроманя статья исполльзуящая огромное колличество буков и слов я хз что сюда еще написать для размера"));
            list.Add(
                new SaveArticle(
                    "Да да Заглавие",
                    "Огроманя статья исполльзуящая огромное колличество буков и слов я хз что сюда еще написать для размера"));
            list.Add(
                new SaveArticle(
                    "Да да Заглавие",
                    "Огроманя статья исполльзуящая огромное колличество буков и слов я хз что сюда еще написать для размера"));
            list.Add(
                new SaveArticle("Да да Заглавие1", "Огроманя статья исполльзуящая немного меньшее колличество буков"));
            list.Add(
                new SaveArticle(
                    "8 Врат",
                    @"Восемь Врат (яп. 八門 Хатимон) — восемь особых точек в системе циркуляции чакры, ограничивающих её поток по телу с целью уменьшения её расхода и увеличения «срока службы» тех или иных органов. Открытие этих врат позволяет ускорить поток чакры и таким образом увеличить свои силовые возможности, что, тем не менее, может навредить организму или даже обернуться смертью, в связи с чем эта техника относится к запрещённым.
На данный момент в манге указано, что Рок Ли способен открывать шесть врат[2], а его учитель, Майто Гай и его отец, Майто Дай, открывали восемь врат. Также во время тренировок по скалолазанию и сражения с Какудзу одни врата открывал Какаси Хатакэ, что, возможно, не является для него пределом.
Схема расположения Восьми Врат: 1-е и 2-е расположены в головном мозге, врата с 3-х по 7-е в спинном мозге, 8-е врата в сердце
Ниже приведены названия врат и влияние, которое они оказывают на открывшего их ниндзя:
    Врата Начала (яп. 開門 Каймон) позволяют использовать технику Первичного Лотоса (яп. 表蓮華 Омотэ Рэнгэ), первый этап которой состоит в обездвиживании противника в полёте за счёт оборачивания того с помощью лент. Затем, пользователь захватывает своего врага и устремляется головой вниз, падая на землю, таким образом нанося противнику серьёзные травмы.
    Врата Покоя (яп. 休門 Кю:мон) — с мышц снимается напряжение, таким образом увеличивая выносливость ниндзя.
    Врата Жизни (яп. 生門 Сэймон) — на этом уровне пользователь способен выполнить Обратный Лотос (яп. 裏蓮華 Ура Рэнгэ)[3], являющийся более сложной версией Переднего Лотоса. Эта техника используется в воздухе, где сначала противнику руками и ногами наносятся мощные и быстрые удары, а затем его разбивают о землю.
    Врата Боли (яп. 傷門 Сё:мон) — сила и скорость ниндзя значительно возрастают. Тело работает на пределе своих возможностей — высокие нагрузки начинают разрывать мышцы.
    Врата Предела (яп. 杜門 Томон) — дальнейшее увеличение физических возможностей.
    Врата Вида (яп. 景門 Кэймон) — на этом уровне возможно исполнять дзюцу Утреннего Павлина[4] (яп. 朝孔雀 Аса Кудзяку), благодаря которому удары становятся такими быстрыми и мощными, что вокруг кулака формируется яркая чакра.
    Врата Потрясения (яп. 驚門 Кё:мон) — дальнейшее увеличение силы ударов и скорости перемещения. При открытии семи врат возможно использование т. н. техники Полуденный тигр (яп. 昼虎 Хиру:дора), удар которой фокусирует огромное атмосферное давление, которое приобретает форму тигра, на противнике, а затем взрывает всё в один момент[5].
    Врата Смерти (яп. 死門 Симон) — последние и самые мощные врата, открытие которых провоцирует расход всей энергии тела. При открытии восьмых врат возможно использование техники Вечерний Слон (яп. 夕象 Сэкидзо), которая позволяет нанести по противнику пять сокрушительных ударов (足, Соку), каждый из которых превосходит по скорости предыдущий, а также техники Ночной Гай (яп. 夜ガイ Ягай). Данные техники дают пользователю силу, превышающую даже уровень техник ранга S, благодаря тому, что сердце начинает работать на своём максимуме. Однако этот эффект «большого взрыва» является временным, который, прекратившись, разрушает в теле все мускулы, включая сердечную мышцу. Человека, открывшего Врата Смерти, ждёт верная гибель — тело синоби обращается в пепел."));
            list.Add(
                new SaveArticle(
                    "Да да Заглавие1",
                    "Огроманя статья исполльзуящая немного меньшее колличество буков Огроманя статья исполльзуящая немного меньшее колличество буков"));
            list.Add(
                new SaveArticle(
                    "Да да Заглавие1",
                    "Огроманя статья исполльзуящая немного меньшее колличество буков Огроманя статья исполльзуящая немного меньшее колличество буков Огроманя статья исполльзуящая немного меньшее колличество буков"));
            list.Add(
                new SaveArticle(
                    "Да да Заглавие1",
                    "ывааываукпеукпукфцуц фыавпапкцпцупкцыукуепе фывваолрорлфыварполфывапроауцфакпролнапросывмя олрфвыпарололфвпарлгподукфаагнпшелкфвупапролвам"));
            var count = 0;
            foreach (var newArticle in list)
            {
                var id = count + 1;
                var path = Path.Combine(this.Subdir.FullName, $"{id}.json");
                var file = new FileInfo(path);
                using (var writer = new StringWriter())
                {
                    writer.Write(
                        JsonConvert.SerializeObject(
                            newArticle,
                            Formatting.Indented,
                            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

                    if (!file.Exists) file.Create().Close();

                    using (var stream = new StreamWriter(file.FullName))
                    {
                        stream.WriteLine(writer.ToString());
                    }
                }

                this.context.Articles.Add(new Article(newArticle.Title, path));
                ++count;
            }

            this.context.SaveChanges();
        }
    }
}