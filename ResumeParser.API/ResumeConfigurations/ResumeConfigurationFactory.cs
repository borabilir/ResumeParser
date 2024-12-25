using System;
using System.Collections.Generic;
using System.Linq;
using ResumeParser.API.Model;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.XObjects;
using static ResumeParser.API.Model.Enums;

namespace ResumeParser.API.ResumeConfigurations
{
    public class ResumeConfigurationFactory
    {
        /// <summary>
        /// Page number, words
        /// </summary>
        private Dictionary<int, List<Word>> _words;

        public ResumeConfigurationFactory(string filePath)
        {
            _words = GetWords(filePath);
        }

        public ResumeConfigurationsBase GetConfiguraton(ResumeType resumeType)
        {
            ResumeConfigurationsBase resumeConfigurations = null;

            switch (resumeType)
            {
                case ResumeType.Linkedin:
                    resumeConfigurations = new LinkedinResumeConfigurations(_words);
                    break;
                case ResumeType.KariyerNet:
                    resumeConfigurations = new KariyerNetResumeConfigurations(_words);
                    break;
            }

            return resumeConfigurations;
        }

        private Dictionary<int, List<Word>> GetWords(string filePath)
        {
            Dictionary<int, List<Word>> wordsDict = new Dictionary<int, List<Word>>(); 

            using (PdfDocument document = PdfDocument.Open(filePath))
            {
                foreach (Page page in document.GetPages())
                {
                    IEnumerable<IPdfImage> images = page.GetImages();
                    var words = new List<Word>();
                    words.AddRange(page.GetWords());
                    wordsDict.Add(page.Number, words);
                }
            }

            return wordsDict;
        }
    }
}
