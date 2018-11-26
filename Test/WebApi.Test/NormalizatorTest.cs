// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NormalizatorTest.cs" company="">
//   
// </copyright>
// <summary>
//   The normalizator test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Test
{
    using WebApi.Tools.Normalizator;

    using Xunit;

    /// <summary>
    /// The normalizator test.
    /// </summary>
    public class NormalizatorTest
    {
        /// <summary>
        /// The dumps folder.
        /// </summary>
        private readonly string dumpsFolder;

        /// <summary>
        /// The save fodler.
        /// </summary>
        private readonly string saveFodler;

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizatorTest"/> class.
        /// </summary>
        public NormalizatorTest()
        {
            this.dumpsFolder = @"D:\Загрузки\dumpsNewFormat";
            this.saveFodler = @"D:\Normalizator";
        }

        /// <summary>
        /// The can be normalize.
        /// </summary>
        [Fact]
        public async void CanBeNormalize()
        {
            var normalizer = new Normalizator(this.dumpsFolder, this.saveFodler);

            await normalizer.NormalizeAsync().ConfigureAwait(false);

            Assert.Equal(this.saveFodler, normalizer.SaveFolder);
            Assert.Equal(this.dumpsFolder, normalizer.DumpsFolder);
        }
    }
}