using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static ResumeParser.API.Model.Enums;

namespace ResumeParser.API.Model.Resume
{
    public class ResumeSection
    {
        public ResumeSection(string keyword, SectionType sectionType) {
            Keyword = keyword;
            SectionType = sectionType;
            DataCollection = new List<ResumeData>();
        }

        public SectionType SectionType { get; set; }
        public string Keyword { get; set; }
        public List<ResumeData> DataCollection { get; set; }
        //public Dictionary<Expression<Func<Line, bool>>, TargetLine> LineRules { get; set; }

    }
}
