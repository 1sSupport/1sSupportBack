using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Handler
{
    
   public class ArticlePoolHandler
   {
       private readonly IEnumerable<Article> ArticalPool;
        private readonly Dictionary<string,int> GlobalWordsDictionary = new Dictionary<string, int>();

       public Dictionary<string, int> GlobalDictionary
        {
           
            get => GlobalWordsDictionary;
          
       }

        public ArticlePoolHandler(IEnumerable<Article> articlepool)
        {
            if(articlepool.Any())
            ArticalPool = articlepool;
            else
            {
                throw new ArgumentNullException();
            }
        }

       private void GlobalWordsFreqCounting()
       {
            
            foreach (Article article in ArticalPool)
            {
                foreach (var word in article.Words)
                {
                    if (GlobalWordsDictionary.ContainsKey(word))
                    {
                        var value = article.GetRepeatFrequency(word);
                        GlobalWordsDictionary[word] += value;
                    }
                    else
                    {
                        GlobalWordsDictionary.Add(word, article.GetRepeatFrequency(word));
                    }
                }
            }
        }
       

       public void SetWordsRate()
       {
           GlobalWordsFreqCounting();

           
            foreach (Article article in ArticalPool)
            {
                foreach (var word in article.Words)
                {
                    var value = GlobalWordsDictionary[word] / ArticalPool.Count();
                    article.SetCurrentRate(word, value);

                }
                article.SetTags();
            }
        }
    }
}
