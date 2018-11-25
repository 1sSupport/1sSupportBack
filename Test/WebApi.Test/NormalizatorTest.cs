using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Test
{
    using WebApi.Tools.Normalizator;

    using Xunit;

    public class NormalizatorTest
    {
        private readonly string saveFodler;
        private readonly string dumpsFolder;

        public NormalizatorTest()
        {
            this.dumpsFolder = @"D:\Загрузки\dumpsNewFormat";
            this.saveFodler = @"D:\Normalizator";
        }

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
