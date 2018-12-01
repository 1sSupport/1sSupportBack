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
    using System;
    using System.IO;

    using WebApi.Tools.Normalizator;

    using Xunit;

    /// <summary>
    /// The normalizator test.
    /// </summary>
    public class NormalizatorTest: IDisposable
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
            this.dumpsFolder = @"D:\Загрузки\testDump";
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

        /// <inheritdoc />
        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            var saveDir = new DirectoryInfo(this.saveFodler);
            if (saveDir.Exists)
            {
                saveDir.Delete(true);
            }
        }
    }
}